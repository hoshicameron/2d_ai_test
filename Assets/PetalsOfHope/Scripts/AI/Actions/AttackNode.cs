using PetalsOfHope.AI.Core;
using UnityEngine;

namespace PetalsOfHope.AI.BehaviourTree
{
    /// <summary>
    /// An action node that triggers an attack command via the AIInputSource.
    /// It immediately returns Success.
    /// </summary>
    [CreateAssetMenu(menuName = "AI/Behavior Tree/Nodes/Actions/Attack")]
    public class AttackNode : ActionNode
    {
        public override Node Clone() => Instantiate(this);

        protected override void OnStart(AIContext context) { }
        protected override void OnStop(AIContext context) { }

        protected override NodeState OnUpdate(AIContext context)
        {
            // Send the attack command.
            context.InputSource.TriggerAttack();
            
            // Immediately return Success as requested.
            return NodeState.Success;
        }
    }
}