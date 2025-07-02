using PetalsOfHope.AI.Core;
using UnityEngine;

namespace PetalsOfHope.Scripts.AI.Conditions
{
    [CreateAssetMenu(menuName = "AI/Behavior Tree/Nodes/Conditions/Has Lost Player")]
    public class HasLostPlayerNode : ConditionNode
    {
        [Tooltip("How long the AI will remember the player's last position before giving up.")]
        public float memoryDuration = 3.0f;

        protected override NodeState OnUpdate(AIContext context)
        {
            // First, check if the player is currently visible. If so, we haven't lost them.
            if (IsPlayerVisible(context))
            {
                return NodeState.Failure;
            }

            // Next, check if we have a recent memory of the player.
            bool hasRecentMemory = Time.time < context.TimeLastSeenPlayer + memoryDuration;
            
            return hasRecentMemory ? NodeState.Success : NodeState.Failure;
        }

        // We can reuse the line-of-sight logic here.
        private bool IsPlayerVisible(AIContext context)
        {
            if (context.PlayerTransform == null || context.ChaseData == null) return false;
            float distance = Vector2.Distance(context.AgentTransform.position, context.PlayerTransform.position);
            if (distance > context.ChaseData.detectionRadius) return false;
            Vector2 eyePos = (Vector2)context.AgentTransform.position + new Vector2(0, context.ChaseData.eyeHeight);
            RaycastHit2D hit = Physics2D.Raycast(eyePos, (context.PlayerTransform.position - (Vector3)eyePos).normalized, distance, context.ChaseData.lineOfSightMask);
            return hit.collider == null;
        }
    }
}