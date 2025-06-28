using System.Collections.Generic;

namespace PetalsOfHope.AI.Core
{
    // =========================================================================================
    // FILE: CompositeNode.cs
    // =========================================================================================
    public abstract class CompositeNode : Node
    {
        public List<Node> children = new();

        public override Node Clone()
        {
            CompositeNode node = Instantiate(this);
            node.children = new List<Node>();
            foreach (var child in children)
            {
                node.children.Add(child.Clone());
            }
            return node;
        }
    }

}