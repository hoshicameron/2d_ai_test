using PetalsOfHope.AI.Core;
using UnityEngine;

namespace PetalsOfHope.AI.BehaviourTree
{
    /// <summary>
    /// An action node that flips the agent's patrol direction.
    /// It succeeds instantly.
    /// </summary>
    [CreateAssetMenu(menuName = "AI/Behavior Tree/Nodes/Actions/Turn Around")]
    public class TurnAroundNode : ActionNode
    {
        public override Node Clone() => Instantiate(this);

        protected override void OnStart(AIContext context) { }
        protected override void OnStop(AIContext context) { }

        /// <summary>
        /// Flips the PatrolDirection in the AIContext from 1 to -1, or vice versa.
        /// </summary>
        protected override NodeState OnUpdate(AIContext context)
        {
            context.CurrentFacingDirection *= -1;
            return NodeState.Success;
        }
    }
}