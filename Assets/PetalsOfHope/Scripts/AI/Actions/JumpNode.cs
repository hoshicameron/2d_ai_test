using PetalsOfHope.AI.Core;
using UnityEngine;

namespace PetalsOfHope.Scripts.AI.Actions
{
    /// <summary>
    /// An action node that triggers a jump command via the AIInputSource.
    /// Instantly returns Success.
    /// </summary>
    [CreateAssetMenu(menuName = "AI/Behavior Tree/Nodes/Actions/Jump")]
    public class JumpNode : ActionNode
    {
        public override Node Clone() => Instantiate(this);
        protected override void OnStart(AIContext context) { }
        protected override void OnStop(AIContext context) { }

        protected override NodeState OnUpdate(AIContext context)
        {
            // The AI must be grounded to be able to jump.
            if (!context.Character.IsGrounded)
            {
                return NodeState.Failure;
            }
            context.InputSource.TriggerJump();
            return NodeState.Success;
        }
    }
}