

using System;
using PetalsOfHope.AI.Core;
using UnityEngine;

namespace PetalsOfHope.AI
{
    [CreateAssetMenu(menuName = "AI/Behavior Tree/Nodes/Conditions/Is Facing Wall")]
    public class IsFacingWallNode : ConditionNode
    {
        private bool _isFacingWall;

        protected override NodeState OnUpdate(AIContext context)
        {
            if (context.PatrolData == null) return NodeState.Failure;
            
            Debug.Log($"Wall Layer Mask: {context.PatrolData.wallLayer}");
            Debug.Log($"Wall Layer Mask Binary: {Convert.ToString(context.PatrolData.wallLayer, 2)}");
            
            // The center of the box is calculated based on the agent's position, patrol direction, and offset.
            Vector2 boxCenter = (Vector2)context.AgentTransform.position + new Vector2(context.PatrolData.wallCheckBoxOffset.x * context.PatrolDirection, context.PatrolData.wallCheckBoxOffset.y);
            
            
            // ADD THIS DEBUG:
            Debug.Log($"Box Center: {boxCenter}, Box Size: {context.PatrolData.wallCheckBoxSize}");
            
            // Perform the OverlapBox check.
            Collider2D hit = Physics2D.OverlapBox(boxCenter, context.PatrolData.wallCheckBoxSize, 0f, context.PatrolData.wallLayer);
            
            Debug.Log($"Hit Result: {(hit != null ? hit.name : "NULL")}");
            
            _isFacingWall = hit;
            return _isFacingWall ? NodeState.Success : NodeState.Failure;
        }

        public override void DrawGizmos(AIContext context)
        {
            if (context?.PatrolData == null) return;

            Vector2 boxCenter = (Vector2)context.AgentTransform.position + new Vector2(context.PatrolData.wallCheckBoxOffset.x * context.PatrolDirection, context.PatrolData.wallCheckBoxOffset.y);
            
            // Set color to GREEN if a wall is detected, RED otherwise.
            Gizmos.color = _isFacingWall ? Color.green : Color.red;
            
            // Draw the wire cube to visualize the detection area.
            Gizmos.DrawWireCube(boxCenter, context.PatrolData.wallCheckBoxSize);
        }
    }

}