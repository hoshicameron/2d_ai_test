using UnityEngine;

namespace PetalsOfHope.AI.Core
{
    // =========================================================================================
    // FILE: BehaviorTree.cs
    // =========================================================================================
    [CreateAssetMenu(menuName = "AI/Behavior Tree/Behavior Tree")]
    public class BehaviorTree : ScriptableObject
    {
        public Node rootNode;
        public Node Clone() => rootNode.Clone();
    }
}