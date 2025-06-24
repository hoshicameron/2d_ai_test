

using PetalsOfHope.AI.Core;
using UnityEngine;

namespace PetalsOfHope.AI
{
    [CreateAssetMenu(menuName = "AI/Behavior Tree/Nodes/Conditions/Is Facing Wall")]
    public class IsFacingWallNode : ConditionNode
    {
        [Tooltip("Optional vertical offset from the agent's pivot to cast the check from.")]
        public float yOffset = 0.5f;

        protected override NodeState OnUpdate(AIContext context)
        {
            // Read all configuration from the central PatrolData asset
            if (!context.PatrolData)
            {
                Debug.LogWarning("PatrolData is not assigned in the AIContext.", context.Agent);
                return NodeState.Failure;
            }
            
            var origin = (Vector2)context.AgentTransform.position + new Vector2(0, yOffset);
            var hit = Physics2D.Raycast(origin, new Vector2(context.PatrolDirection, 0), context.PatrolData.wallCheckDistance, context.PatrolData.wallLayer);

            return hit.collider != null ? NodeState.Success : NodeState.Failure;
        }
        
        public override void DrawGizmos(AIContext context)
        {
            if (context?.PatrolData == null) return;

            var origin = (Vector2)context.AgentTransform.position + new Vector2(0, yOffset);
            var direction = new Vector2(context.PatrolDirection, 0);
            var distance = context.PatrolData.wallCheckDistance;
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(origin, origin + (direction * distance));
        }
    }

}