using UnityEngine;

namespace PetalsOfHope.AI.Core
{
    /// <summary>
    /// The Selector node evaluates its children from top to bottom.
    /// It returns SUCCESS as soon as one of its children returns SUCCESS.
    /// If all children return FAILURE, it returns FAILURE.
    /// </summary>
    [CreateAssetMenu(menuName = "AI/Behavior Tree/Nodes/Composites/Selector")]
    public class SelectorNode : CompositeNode
    {
        // A Sequence needs to remember its child, but a Selector should always restart.
        // The currentChildIndex has been removed from this script.

        protected override void OnStart(AIContext context) { }
        protected override void OnStop(AIContext context) { }

        /// <summary>
        /// Evaluates the children of the Selector node.
        /// The loop now correctly starts from the first child (index 0) every time.
        /// </summary>
        protected override NodeState OnUpdate(AIContext context)
        {
            // The loop MUST start from 0 to re-evaluate priorities every frame.
            for (int i = 0; i < children.Count; ++i)
            {
                var child = children[i];

                switch (child.Evaluate(context))
                {
                    // If any child is running or succeeds, the Selector's job is done.
                    case NodeState.Running:
                        return NodeState.Running;
                    case NodeState.Success:
                        return NodeState.Success;
                    // If a child fails, we simply continue to the next one.
                    case NodeState.Failure:
                        continue;
                }
            }

            // If the loop completes, all children failed.
            return NodeState.Failure;
        }
    }
}