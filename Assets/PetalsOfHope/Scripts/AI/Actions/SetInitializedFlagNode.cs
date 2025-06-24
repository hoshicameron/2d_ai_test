using PetalsOfHope.AI.Core;
using UnityEngine;

namespace PetalsOfHope.AI.Actions
{
    // =========================================================================================
    // FILE: SetInitializedFlagNode.cs (Action)
    // =========================================================================================
    /// <summary>
    /// An action node that sets the 'IsInitialized' flag in the AIContext to true.
    /// This is used to ensure the initial spawn sequence only runs once.
    /// It instantly returns Success.
    /// </summary>
    [CreateAssetMenu(menuName = "AI/Behavior Tree/Nodes/Actions/Set Initialized Flag")]
    public class SetInitializedFlagNode : ActionNode
    {
        public override Node Clone() => Instantiate(this);

        protected override void OnStart(AIContext context) { }
        protected override void OnStop(AIContext context) { }

        protected override NodeState OnUpdate(AIContext context)
        {
            context.IsInitialized = true;
            return NodeState.Success;
        }
    }
}