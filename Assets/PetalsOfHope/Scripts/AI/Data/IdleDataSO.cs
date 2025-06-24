using UnityEngine;

namespace PetalsOfHope.AI.Data
{
    /// <summary>
    /// A ScriptableObject that holds configuration data for an AI's idle behaviors.
    /// </summary>
    [CreateAssetMenu(menuName = "AI/Data/Idle Data")]
    public class IdleDataSO : ScriptableObject
    {
        [Header("Idle Timings")]
        [Tooltip("The time in seconds for the AI to idle when it first spawns.")]
        public float initialIdleDuration = 2.0f;
        
    }
}