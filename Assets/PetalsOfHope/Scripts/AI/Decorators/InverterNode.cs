using PetalsOfHope.AI.Core;
using UnityEngine;

namespace PetalsOfHope.AI.Decorators
{
    // =========================================================================================
    // FILE: InverterNode.cs (Decorator)
    // =========================================================================================
    /// <summary>
    /// The Inverter node inverts the result of its child node.
    /// - If the child returns SUCCESS, the Inverter returns FAILURE.
    /// - If the child returns FAILURE, the Inverter returns SUCCESS.
    /// - If the child is RUNNING, the Inverter returns RUNNING.
    /// </summary>
    [CreateAssetMenu(menuName = "AI/Behavior Tree/Nodes/Decorators/Inverter")]
    public class InverterNode : DecoratorNode
    {
        protected override NodeState OnUpdate(AIContext context)
        {
            if (child == null)
            {
                return NodeState.Failure;
            }

            switch (child.Evaluate(context))
            {
                case NodeState.Running:
                    return NodeState.Running;
                case NodeState.Success:
                    return NodeState.Failure;
                case NodeState.Failure:
                    return NodeState.Success;
            }
            return NodeState.Failure;
        }
    }
}