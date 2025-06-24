using PetalsOfHope.AI.Core;
using UnityEngine;

namespace PetalsOfHope.AI.Actions
{
    /// <summary>
    /// A condition node that checks if the player is within a certain range and
    /// in direct line of sight.
    /// </summary>
    [CreateAssetMenu(menuName = "AI/Behavior Tree/Nodes/Conditions/Is Player Visible")]
    public class IsPlayerVisibleNode : ConditionNode
    {
        protected override NodeState OnUpdate(AIContext context)
        {
            // Fail if the necessary data or references are missing.
            if (context.ChaseData == null)
            {
                Debug.LogWarning("ChaseData is not assigned in the AIContext.", context.Agent);
                return NodeState.Failure;
            }
            if (context.PlayerTransform == null)
            {
                return NodeState.Failure; // Cannot see a player that doesn't exist.
            }

            Vector2 agentPosition = context.AgentTransform.position;
            Vector2 playerPosition = context.PlayerTransform.position;

            // Step 1: Check if the player is within the detection radius.
            var distanceToPlayer = Vector2.Distance(agentPosition, playerPosition);
            if (distanceToPlayer > context.ChaseData.detectionRadius)
            {
                return NodeState.Failure; // Player is too far away.
            }

            // Step 2: Check for line of sight.
            var eyePosition = agentPosition + new Vector2(0, context.ChaseData.eyeHeight);
            var directionToPlayer = (playerPosition - eyePosition).normalized;

            var hit = Physics2D.Raycast(eyePosition, directionToPlayer, distanceToPlayer, context.ChaseData.lineOfSightMask);

            // If the raycast hits something, it means an obstacle is in the way.
            return hit.collider != null ?
                // We could add a check here to see if the thing we hit was the player,
                // but for simplicity, we assume anything on the mask blocks vision.
                NodeState.Failure :
                // If both checks pass, the player is visible.
                NodeState.Success;
        }
        
        public override void DrawGizmos(AIContext context)
        {
            if (context?.ChaseData == null) return;

            var eyePosition = (Vector2)context.AgentTransform.position + new Vector2(0, context.ChaseData.eyeHeight);

            // Draw detection radius
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(context.AgentTransform.position, context.ChaseData.detectionRadius);

            // Draw line of sight to player, if player exists
            if (context.PlayerTransform != null)
            {
                var distanceToPlayer = Vector2.Distance(eyePosition, context.PlayerTransform.position);
                var inRange = distanceToPlayer <= context.ChaseData.detectionRadius;

                if (inRange)
                {
                    // If the actual raycast in OnUpdate hits something, the line will be red, otherwise green.
                    var hit = Physics2D.Raycast(eyePosition, (context.PlayerTransform.position - (Vector3)eyePosition).normalized, distanceToPlayer, context.ChaseData.lineOfSightMask);
                    Gizmos.color = hit.collider != null ? Color.red : Color.green;
                    Gizmos.DrawLine(eyePosition, context.PlayerTransform.position);
                }
            }
        }
    }
}