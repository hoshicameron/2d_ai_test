using PetalsOfHope.AI.Core;
using UnityEngine;

namespace PetalsOfHope.AI
{
    [CreateAssetMenu(menuName = "AI/Behavior Tree/Nodes/Conditions/Is At Ledge")]
    public class IsAtLedgeNode : ConditionNode
    {
        private bool _isAtLedge;
        protected override NodeState OnUpdate(AIContext context)
        {
            if (context.PatrolData == null) return NodeState.Failure;

            Vector2 boxCenter = (Vector2)context.AgentTransform.position + new Vector2(context.PatrolData.ledgeCheckBoxOffset.x * context.CurrentFacingDirection, context.PatrolData.ledgeCheckBoxOffset.y);
            
            Collider2D hit = Physics2D.OverlapBox(boxCenter, context.PatrolData.ledgeCheckBoxSize, 0f, context.PatrolData.groundLayer);
            _isAtLedge = !hit; 
            return _isAtLedge ? NodeState.Success : NodeState.Failure;
        }
        
        public override void DrawGizmos(AIContext context)
        {
            if (context?.PatrolData == null) return;
            
            Vector2 boxCenter = (Vector2)context.AgentTransform.position + new Vector2(context.PatrolData.ledgeCheckBoxOffset.x * context.CurrentFacingDirection, context.PatrolData.ledgeCheckBoxOffset.y);

            Gizmos.color = _isAtLedge ? Color.green : Color.yellow;
            
            Gizmos.DrawWireCube(boxCenter, context.PatrolData.ledgeCheckBoxSize);
        }
    }
}