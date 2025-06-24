using UnityEngine;

namespace PetalsOfHope.AI.Core
{
    // =========================================================================================
    // FILE: InverterNode.cs
    // =========================================================================================
    [CreateAssetMenu(menuName = "AI/Behavior Tree/Nodes/Decorators/Inverter")]
    public class InverterNode : DecoratorNode
    {
        protected override NodeState OnUpdate(AIContext context)
        {
            if (child == null) return NodeState.Failure;
            switch (child.Evaluate(context))
            {
                case NodeState.Running: return NodeState.Running;
                case NodeState.Success: return NodeState.Failure;
                case NodeState.Failure: return NodeState.Success;
            }
            return NodeState.Failure;
        }
    }
}