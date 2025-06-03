# Task ID: 4.5.4
# Parent Task ID: 4.5
# Title: Integrate GameProgressionManager with PlayerAbilities and SceneLoader/UI
# Status: pending
# Dependencies: 4.5.1, 4.4.1, 4.2.1 # GameProgressionManager, PlayerAbilities, SceneLoader
# Priority: high
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Integrate `GameProgressionManager` with other systems that depend on progression data:
- `PlayerAbilities` should use `GameProgressionManager.IsAbilityUnlocked()`.
- `SceneLoader` or UI systems (for level selection) should use `GameProgressionManager.IsLevelUnlocked()`.

# Details:
1.  **Integrate with `PlayerAbilities`:**
    *   Ensure `PlayerAbilities.Can...()` methods (Task 4.4.1) correctly call `GameProgressionManager.Instance.IsAbilityUnlocked(abilityID)`.
    *   Test that player abilities are only usable if `GameProgressionManager` reports them as unlocked.

2.  **Integrate with Level Selection (Conceptual for now, UI is Phase 5):**
    *   When a Level Selection UI is implemented (Phase 5), UI elements representing levels (e.g., buttons) will need to check `GameProgressionManager.Instance.IsLevelUnlocked(correspondingSceneDataSO)`.
    *   Locked levels might be greyed out, display a lock icon, or be unclickable.
    *   Clicking an unlocked level button would call `SceneLoader.Instance.LoadScene(sceneData.GetSceneName())`.

3.  **Integrate with `LevelExit` (Optional Enhancement):**
    *   `LevelExit.cs` (Task 4.2.3) could optionally check if the `_nextLevelData` is unlocked via `GameProgressionManager` before allowing transition, or `SceneLoader` itself could have a pre-load check.
    *   For now, assume `LevelExit` always attempts to load the configured next level if the progression is linear and implied.

# Acceptance Criteria:
- `PlayerAbilities` component correctly queries `GameProgressionManager` to determine if abilities like Double Jump, Dash, etc., can be used.
- Player cannot perform abilities that `GameProgressionManager` reports as locked.
- (Conceptual for UI) Level selection mechanisms will be able to query `GameProgressionManager` to determine which levels are accessible.

# Test Strategy:
- **PlayerAbilities Integration:**
    - Start game with certain abilities locked (via `GameProgressionManager`'s initial state or loaded save).
    - Attempt to use these abilities; they should fail.
    - Use a test script to call `GameProgressionManager.Instance.UnlockAbility("SomeAbilityID")`.
    - Verify the player can now use "SomeAbilityID".
    - Test with save/load: lock ability, save, load, verify still locked. Unlock, save, load, verify still unlocked.
- **Level Selection Integration:**
    - This will be tested more thoroughly in Phase 5 with actual UI.
    - For now, can create a test script that lists all `SceneDataSO`s and prints their locked/unlocked status based on `GameProgressionManager`.

# Notes/Questions:
- This task ensures that the progression data managed by `GameProgressionManager` actually influences gameplay and access to content.
- Full UI integration for level selection is deferred to Phase 5.