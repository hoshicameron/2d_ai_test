using PetalsOfHope.AI.Core;
using UnityEngine;

namespace PetalsOfHope.AI.BehaviourTree
{
    /// <summary>
    /// An action node that makes the character move in its current patrol direction.
    /// It continuously returns Running. This node is intended to be a fallback action
    /// that is interrupted by higher-priority nodes (like obstacle detection).
    /// </summary>
    [CreateAssetMenu(menuName = "AI/Behavior Tree/Nodes/Actions/Patrol")]
    public class PatrolNode : ActionNode
    {
        public override Node Clone() => Instantiate(this);

        protected override void OnStart(AIContext context) { }

        /// <summary>
        /// When this node is interrupted, command the character to stop moving.
        /// </summary>
        protected override void OnStop(AIContext context)
        {
            context.InputSource.SetMoveInput(Vector2.zero);
        }

        /// <summary>
        /// Tells the input source to move in the current patrol direction stored in the context.
        /// </summary>
        protected override NodeState OnUpdate(AIContext context)
        {
            var moveVector = new Vector2(context.PatrolDirection, 0);
            context.InputSource.SetMoveInput(moveVector);
            
            // This action never completes on its own; it runs until a parent node
            // interrupts it (e.g., when an obstacle is detected).
            return NodeState.Running;
        }
    }
}