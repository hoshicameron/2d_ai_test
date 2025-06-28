using UnityEngine;

namespace PetalsOfHope.AI.Data
{
    /// <summary>
    /// A ScriptableObject that holds configuration data for an AI's chase behavior.
    /// </summary>
    [CreateAssetMenu(menuName = "AI/Data/Chase Data")]
    public class ChaseDataSO : ScriptableObject
    {
        [Header("Detection Settings")]
        [Tooltip("The maximum distance at which the AI can detect the player.")]
        public float detectionRadius = 10f;

        [Tooltip("The layer(s) that will block the AI's line of sight to the player (e.g., Walls, Ground).")]
        public LayerMask lineOfSightMask;

        [Tooltip("The vertical offset from the agent's pivot for the line-of-sight check.")]
        public float eyeHeight = 0.5f;
    }
}