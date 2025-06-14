using UnityEngine;

namespace PetalsOfHope.AI.Core
{
    // =========================================================================================
    // FILE: SelectorNode.cs
    // =========================================================================================
    [CreateAssetMenu(menuName = "AI/Behavior Tree/Nodes/Composites/Selector")]
    public class SelectorNode : CompositeNode
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
                    case NodeState.Success: return NodeState.Success;
                    case NodeState.Failure: continue;
                }
            }
            return NodeState.Failure;
        }
    }
}