using UnityEngine;

namespace PetalsOfHope.AI.Core
{
    // =========================================================================================
    // FILE: SequenceNode.cs
    // =========================================================================================
    [CreateAssetMenu(menuName = "AI/Behavior Tree/Nodes/Composites/Sequence")]
    public class SequenceNode : CompositeNode
    {
        // A stateful index is not needed here because the sequence must
        // re-validate all preceding conditions every frame.
        
        protected override void OnStart(AIContext context) { }
        protected override void OnStop(AIContext context) { }

        /// <summary>
        /// Evaluates the children of the Sequence node.
        /// The loop now correctly starts from the first child (index 0) every time
        // to ensure all conditions are still valid before continuing an action.
        /// </summary>
        protected override NodeState OnUpdate(AIContext context)
        {
            // The loop MUST start from 0 to re-validate the entire sequence.
            for (int i = 0; i < children.Count; ++i)
            {
                var child = children[i];
                switch (child.Evaluate(context))
                {
                    case NodeState.Running:
                        // If a child is running, the sequence is also running.
                        return NodeState.Running;
                    case NodeState.Failure:
                        // If any child fails, the entire sequence fails immediately.
                        return NodeState.Failure;
                    case NodeState.Success:
                        // If a child succeeds, continue to the next one.
                        continue;
                }
            }
            
            // If the loop completes, it means all children succeeded.
            return NodeState.Success;
        }
    }
}