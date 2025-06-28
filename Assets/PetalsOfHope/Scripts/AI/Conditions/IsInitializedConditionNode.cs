using PetalsOfHope.AI.Core;
using UnityEngine;

namespace PetalsOfHope.AI.Conditions
{
    // =========================================================================================
    // FILE: IsInitializedConditionNode.cs (Condition)
    // =========================================================================================
    /// <summary>
    /// A condition node that checks if the AI has completed its initial startup sequence
    /// by checking the 'IsInitialized' flag in the AIContext.
    /// </summary>
    [CreateAssetMenu(menuName = "AI/Behavior Tree/Nodes/Conditions/Is Initialized")]
    public class IsInitializedConditionNode : ConditionNode
    {
        protected override NodeState OnUpdate(AIContext context)
        {
            return context.IsInitialized ? NodeState.Success : NodeState.Failure;
        }
    }
}