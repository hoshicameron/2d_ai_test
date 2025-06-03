using UnityEngine;

namespace PetalsOfHope.Data.Levels
{
    [CreateAssetMenu(menuName = "Petals of Hope/Data/Level Settings", fileName = "NewLevelSettingsSO")]
    public class LevelSettingsSO : ScriptableObject
    {
        [Header("Level Configuration")]
        [Tooltip("Gravity scale for this level. Default Unity gravity is -9.81 on Y. This is a multiplier for Rigidbody2D.gravityScale.")]
        public float gravityScale = 1.0f;

        [Tooltip("Default background music for this level.")]
        public AudioClip backgroundMusic;

        [Header("Level Boundaries (Optional)")]
        [Tooltip("Minimum X and Y coordinates for camera and player confines.")]
        public Vector2 minBounds = new Vector2(-100, -100);
        
        [Tooltip("Maximum X and Y coordinates for camera and player confines.")]
        public Vector2 maxBounds = new Vector2(100, 100);

        [Header("Player Spawn")]
        [Tooltip("Default spawn position for the player in this level. Can be overridden by in-scene spawn points.")]
        public Vector2 defaultPlayerSpawnPosition = Vector2.zero;
    }
}
