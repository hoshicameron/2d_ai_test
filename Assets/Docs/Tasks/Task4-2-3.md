# Task ID: 4.2.3
# Parent Task ID: 4.2
# Title: Implement Level Transition System
# Status: pending
# Dependencies: 4.2.1, 4.2.2 # SceneLoader, SceneDataSO
# Priority: high
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement a system for transitioning between levels. This typically involves a physical trigger in the scene (e.g., end-of-level portal) or a UI selection (e.g., from a world map or level select menu, though menus are Phase 5).

# Details:
**Physical Trigger Approach (for end-of-level):**

1.  **Create `LevelExit.cs` Script:**
    *   File Location: `Assets/_Project/Scripts/Gameplay/LevelElements/LevelExit.cs`
    *   Namespace: `PetalsOfHope.Gameplay.LevelElements`
    *   Implementation:
        ```csharp
        // In Assets/_Project/Scripts/Gameplay/LevelElements/LevelExit.cs
        namespace PetalsOfHope.Gameplay.LevelElements
        {
            using UnityEngine;
            using PetalsOfHope.Systems.SceneManagement; // For SceneDataSO and SceneLoader

            public class LevelExit : MonoBehaviour
            {
                [Tooltip("SceneDataSO representing the next level to load.")]
                [SerializeField] private SceneDataSO _nextLevelData;

                [Tooltip("Should the transition happen automatically on player enter?")]
                [SerializeField] private bool _autoTransition = true;
                
                // [Tooltip("Input action required if not auto transitioning (e.g., 'Interact' event from InputReader).")]
                // [SerializeField] private GameEventSO _interactEvent; // If manual trigger is needed

                private bool _playerInRange = false;

                private void OnTriggerEnter2D(Collider2D collision)
                {
                    if (collision.CompareTag("Player"))
                    {
                        if (_autoTransition)
                        {
                            TransitionToNextLevel();
                        }
                        else
                        {
                            _playerInRange = true;
                            // Show UI prompt e.g., "Press E to Enter Next Level"
                            // This would involve UI systems from Phase 5 or a simple debug log for now.
                            Debug.Log("Player in LevelExit range. Waiting for interact input (if configured).");
                        }
                    }
                }

                private void OnTriggerExit2D(Collider2D collision)
                {
                    if (collision.CompareTag("Player"))
                    {
                        _playerInRange = false;
                        // Hide UI prompt
                    }
                }

                // Example for handling interact input if not auto transitioning
                // private void OnEnable() { if (!_autoTransition && _interactEvent != null) _interactEvent.RegisterListener(HandleInteract); }
                // private void OnDisable() { if (!_autoTransition && _interactEvent != null) _interactEvent.UnregisterListener(HandleInteract); }
                // private void HandleInteract() { if (_playerInRange) TransitionToNextLevel(); }

                private void TransitionToNextLevel()
                {
                    if (_nextLevelData == null)
                    {
                        Debug.LogError("NextLevelData not assigned to LevelExit trigger!", this);
                        return;
                    }

                    string sceneToLoad = _nextLevelData.GetSceneName();
                    if (!string.IsNullOrEmpty(sceneToLoad))
                    {
                        Debug.Log($"Transitioning to level: {sceneToLoad}");
                        // Optionally, save game progress here before loading next level
                        // SaveLoadManager.Instance?.SaveGame(); 
                        SceneLoader.Instance?.LoadScene(sceneToLoad);
                    }
                    else
                    {
                        Debug.LogError($"Scene name for next level is empty in SceneDataSO: {_nextLevelData.name}", this);
                    }
                }
            }
        }
        ```
2.  **Create Level Exit Prefab:**
    *   Location: `Assets/_Project/Prefabs/LevelElements/Interactables/LevelExitPrefab.prefab`
    *   Create a GameObject with a visual (e.g., a portal sprite, an archway).
    *   Add `BoxCollider2D`, set `Is Trigger` to true.
    *   Attach `LevelExit.cs` script.
    *   In the Inspector for the prefab instance in a level:
        *   Assign the `SceneDataSO` asset for the *next* level to the `_nextLevelData` field.
        *   Set `_autoTransition` as desired.

**UI Selection Approach (e.g., for a World Map - partial implementation, full UI is Phase 5):**

*   This would involve UI buttons. Each button, when clicked, would reference a `SceneDataSO` and call `SceneLoader.Instance.LoadScene(sceneData.GetSceneName())`.
*   This part will be more fully developed in Phase 5 (UI Systems). For now, the physical trigger is the primary focus for level-to-level flow.

# Acceptance Criteria:
- `LevelExit.cs` script is implemented.
- Player entering a `LevelExit` trigger (with `_autoTransition = true`) successfully loads the scene specified in its `_nextLevelData` field, using `SceneLoader`.
- `LevelExitPrefab` is created and can be placed in levels.
- The system gracefully handles missing `_nextLevelData` or empty scene names with error logs.

# Test Strategy:
- Manual Testing:
    - Create two scenes, "Level1" and "Level2". Ensure they are in Build Settings.
    - Create `SceneDataSO` assets for both: `Level1_DataSO` and `Level2_DataSO`.
    - In "Level1", place a `LevelExitPrefab`. Assign `Level2_DataSO` to its `_nextLevelData` field.
    - Start game in "Level1". Move player into the `LevelExit` trigger.
    - Verify "Level2" loads with the fade transition.
    - (Optional) Test `_autoTransition = false` with a temporary debug key press to call `TransitionToNextLevel()` when player is in range.

# Notes/Questions:
- The `LevelExit` script currently assumes auto-transition. If manual interaction (e.g., press "Interact" button) is needed, the script would need to listen to the interact input event from `InputReader` (Task 2.1.2) when the player is in range. This is commented out as an example.
- Saving game progress (`SaveLoadManager.Instance?.SaveGame();`) before loading the next level is a common pattern and should be considered here or as part of the `GameProgressionManager` logic.