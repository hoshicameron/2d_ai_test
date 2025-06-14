using UnityEngine;

namespace PetalsOfHope.AI.Core
{
    // =========================================================================================
    // FILE: SequenceNode.cs
    // =========================================================================================
    [CreateAssetMenu(menuName = "AI/Behavior Tree/Nodes/Composites/Sequence")]
    public class SequenceNode : CompositeNode
    {
        [System.NonSerialized] private int currentChildIndex = 0;
        protected override void OnStart(AIContext context) => currentChildIndex = 0;

        protected override NodeState OnUpdate(AIContext context)
        {
            for (int i = currentChildIndex; i < children.Count; ++i)
            {
                currentChildIndex = i;
                var child = children[currentChildIndex];
                switch (child.Evaluate(context))
                {
                    case NodeState.Running: return NodeState.Running;
                    case NodeState.Failure: return NodeState.Failure;
                    case NodeState.Success: continue;
                }
            }
            return NodeState.Success;
        }
    }
}