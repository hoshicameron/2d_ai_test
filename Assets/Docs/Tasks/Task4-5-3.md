# Task ID: 4.5.3
# Parent Task ID: 4.5
# Title: Integrate GameProgressionManager with Save/Load System
# Status: pending
# Dependencies: 4.5.1, 1.4.5 # GameProgressionManager core, SaveLoadManager
# Priority: critical
# Estimated Effort: S (mostly ensuring registration)
# Assignee: Unassigned

# Description:
Ensure that `GameProgressionManager` is correctly registered with the `SaveLoadManager` and that its state (unlocked levels, abilities) is saved and loaded properly.

# Details:
1.  **Ensure `GameProgressionManager` Implements `ISaveable`:**
    *   This was done as part of Task 4.5.1. Verify the `UniqueID`, `CaptureState()`, and `RestoreState(object state)` methods are correctly implemented.
2.  **Register `GameProgressionManager` with `SaveLoadManager`:**
    *   The `SaveLoadManager.FindAllSaveableEntities()` method (Task 1.4.5) should find `MonoBehaviour`s that implement `ISaveable`.
    *   If `GameProgressionManager` is a `MonoBehaviour` (as currently designed), it should be found automatically if it's active in the scene when `FindAllSaveableEntities()` is called.
    *   Alternatively, `GameProgressionManager` (if a Singleton) can explicitly register itself:
        ```csharp
        // In GameProgressionManager.Awake() or Start()
        // SaveLoadManager.Instance?.RegisterSaveable(this);
        ```
        And unregister in `OnDestroy()`:
        ```csharp
        // In GameProgressionManager.OnDestroy()
        // SaveLoadManager.Instance?.UnregisterSaveable(this);
        ```
        The automatic `FindObjectsOfType` in `SaveLoadManager` is generally preferred for persistent singletons like this, assuming `SaveLoadManager` calls its find method at an appropriate time (e.g., after scene load or on demand).

3.  **Test Save/Load Cycle for Progression:**
    *   In a test scene with `SaveLoadManager` and `GameProgressionManager`.
    *   **Step 1: Initial State:** Start game. Verify default unlocked levels/abilities.
    *   **Step 2: Unlock Items:** Use test scripts or game mechanics to unlock a new level and a new ability. Verify `IsLevelUnlocked` and `IsAbilityUnlocked` reflect these changes.
    *   **Step 3: Save Game:** Call `SaveLoadManager.Instance.SaveGame()`.
    *   **Step 4: Reset/Modify State (Simulate Restart):**
        *   Option A: Call `GameProgressionManager.Instance.InitializeDefaultProgression()` to reset its state to defaults.
        *   Option B: Restart the game/scene.
    *   **Step 5: Load Game:** Call `SaveLoadManager.Instance.LoadGame()`.
    *   **Step 6: Verify:** Check that the previously unlocked level and ability (from Step 2) are now reported as unlocked by `GameProgressionManager`.

# Acceptance Criteria:
- `GameProgressionManager` correctly participates in the save/load process.
- Unlocked levels and abilities are persisted when the game is saved.
- Persisted progression data is correctly restored when the game is loaded, overriding defaults.
- If no save data exists, default progression is applied.

# Test Strategy:
- As detailed in "Test Save/Load Cycle for Progression" (Step 3 above).
- Inspect save file content (if using `JsonDataService`) to verify progression data is being written.
- Check console logs for any errors during save/load of progression data.

# Notes/Questions:
- This task primarily focuses on testing the `ISaveable` implementation of `GameProgressionManager` done in Task 4.5.1.
- The robustness of `SaveLoadManager.FindAllSaveableEntities()` or the manual registration process is key.