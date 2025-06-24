using PetalsOfHope.AI.Core;
using UnityEngine;

namespace PetalsOfHope.AI
{
    [CreateAssetMenu(menuName = "AI/Behavior Tree/Nodes/Conditions/Is At Ledge")]
    public class IsAtLedgeNode : ConditionNode
    {
        protected override NodeState OnUpdate(AIContext context)
        {
            // Read all configuration from the central PatrolData asset
            if (!context.PatrolData)
            {
                Debug.LogWarning("PatrolData is not assigned in the AIContext.", context.Agent);
                return NodeState.Failure;
            }

            var origin = (Vector2)context.AgentTransform.position + new Vector2(context.PatrolData.ledgeCheckForwardDistance * context.PatrolDirection, 0);
            
            var hit = Physics2D.Raycast(origin, Vector2.down, context.PatrolData.ledgeCheckDownwardDistance, context.PatrolData.groundLayer);
            
            return !hit.collider ? NodeState.Success : NodeState.Failure;
        }
        
        public override void DrawGizmos(AIContext context)
        {
            if (context?.PatrolData == null) return;
            
            var forwardPoint = (Vector2)context.AgentTransform.position + new Vector2(context.PatrolData.ledgeCheckForwardDistance * context.PatrolDirection, 0);
            var endPoint = forwardPoint + (Vector2.down * context.PatrolData.ledgeCheckDownwardDistance);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(forwardPoint, endPoint);
        }
    }
}