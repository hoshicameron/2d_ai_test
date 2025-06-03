# Task ID: 1.3.3
# Parent Task ID: 1.3
# Title: Implement LevelSettingsSO
# Status: completed
# Dependencies: 1.1.2, 1.1.4
# Priority: high
# Estimated Effort: S
# Assignee: Unassigned

# Description:
Implement `LevelSettingsSO.cs`, a ScriptableObject to store level-specific settings like gravity scale and background music.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/Data/Levels/LevelSettingsSO.cs`
2.  **Namespace:** `PetalsOfHope.Data.Levels`
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/Data/Levels/LevelSettingsSO.cs
    namespace PetalsOfHope.Data.Levels
    {
        using UnityEngine;

        [CreateAssetMenu(menuName = "Petals of Hope/Data/Level Settings", fileName = "NewLevelSettingsSO")]
        public class LevelSettingsSO : ScriptableObject
        {
            [Header("Level Configuration")]
            [Tooltip("Gravity scale for this level. Default Unity gravity is -9.81 on Y. This is a multiplier for Rigidbody2D.gravityScale.")]
            public float gravityScale = 1.0f; // Assuming Physics2D.gravity might be changed, or individual RBs

            [Tooltip("Default background music for this level.")]
            public AudioClip backgroundMusic;

            // Add other level-specific settings:
            // e.g., Post-processing profile, tilemap palettes, available enemy types, specific hazards

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
    ```

# Acceptance Criteria:
- `LevelSettingsSO.cs` file is created at the specified location.
- The class inherits from `ScriptableObject`, is in the `PetalsOfHope.Data.Levels` namespace, and has the `[CreateAssetMenu]` attribute.
- It includes fields for `gravityScale` and `backgroundMusic`.
- Optional fields like `minBounds`, `maxBounds`, and `defaultPlayerSpawnPosition` are included for consideration.
- The script compiles without errors.

# Test Strategy:
- Manual Verification:
    - Create an instance of `LevelSettingsSO` using the Asset Menu.
    - Inspect the created asset to ensure all fields are visible and editable.

# Notes/Questions:
- The `gravityScale` is implemented as a multiplier for `Physics2D.gravity` to affect all physics objects in the level.
- Added `minBounds`, `maxBounds`, and `defaultPlayerSpawnPosition` for complete level configuration.
- `backgroundMusic` uses `AudioClip` for simplicity, which can be replaced with an `AudioEventSO` if the audio system is updated later.
- Implementation completed on 2025-06-03. Level settings are now fully configurable through the Unity Editor.