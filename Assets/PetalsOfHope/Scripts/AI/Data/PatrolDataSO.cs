using UnityEngine;

namespace PetalsOfHope.Scripts.AI.Data
{
    /// <summary>
    /// A ScriptableObject that holds configuration data for an AI's patrol behavior.
    /// This allows designers to easily create and tweak different patrol styles.
    /// </summary>
    [CreateAssetMenu(fileName ="New PatrolDataSO" ,menuName = "AI/Data/Patrol Data")]
    public class PatrolDataSO : ScriptableObject
    {
        [Header("Layer Masks")]
        [Tooltip("The layer(s) that should be considered a wall.")]
        public LayerMask wallLayer;

        [Tooltip("The layer(s) that should be considered ground.")]
        public LayerMask groundLayer;
        
        [Header("Wall Detection")]
        [Tooltip("The size of the box used to detect walls in front of the agent.")]
        public Vector2 wallCheckBoxSize = new Vector2(0.5f, 1.0f);
        
        [Tooltip("The offset from the agent's pivot where the wall detection box is centered.")]
        public Vector2 wallCheckBoxOffset = new Vector2(0.5f, 0.5f);
        
        [Header("Ledge Detection")]
        [Tooltip("The size of the box used to detect the ground in front of the agent.")]
        public Vector2 ledgeCheckBoxSize = new Vector2(0.5f, 0.1f);
        
        [Tooltip("The offset from the agent's pivot where the ledge detection box is centered.")]
        public Vector2 ledgeCheckBoxOffset = new Vector2(0.5f, 0f);
    }
}
