# Task ID: 4.5
# Parent Task ID: 4
# Title: Game Progression System Implementation
# Status: pending
# Dependencies: 4.3.2 (InventorySystem), 1.4 (Save/Load System), 1.2 (Event Bus)
# Priority: critical
# Estimated Effort: L (aggregate of subtasks)
# Assignee: Unassigned

# Description:
Implement `GameProgressionManager.cs` to manage overall game progression, including unlocked levels and abilities. This system will reference `InventorySystem` (for talismans that might unlock abilities) and implement `ISaveable` to persist progression data.

# Details:
This system is central to tracking what the player has achieved and what content is available to them.
- `GameProgressionManager.cs`: A Singleton or service that tracks unlocked levels and abilities.
- It will listen to events like `TalismanAwardedEventSO` to potentially update progression (e.g., a specific talisman unlocks an ability).
- It provides methods like `IsLevelUnlocked(SceneDataSO)` and `IsAbilityUnlocked(AbilityID)`.
- It implements `ISaveable` to save/load its state.
- It raises events like `LevelUnlockedEventSO`, `AbilityUnlockedEventSO`.

Refer to subtasks 4.5.1 through 4.5.4.

# Acceptance Criteria:
- All subtasks (4.5.1 - 4.5.4) are completed.
- `GameProgressionManager` can track and report unlocked levels and abilities.
- Unlocking logic (e.g., via talismans) is functional.
- Progression data (unlocked levels/abilities) is saved and loaded correctly.
- Events are raised when levels/abilities are unlocked.
- Other systems (like `PlayerAbilities`, UI for level selection) can query `GameProgressionManager`.

# Test Strategy:
- Test unlocking levels and abilities through various mock triggers (e.g., collecting a specific talisman).
- Verify `IsLevelUnlocked` and `IsAbilityUnlocked` return correct values.
- Test save/load functionality: unlock items, save, restart/load, verify items are still unlocked.
- Verify events are raised upon unlocking.

# Notes/Questions:
- The definition of `AbilityID` (string, enum, etc.) needs to be consistent with its usage in `PlayerAbilities` (Task 4.4.1).
- `LevelUnlockConditionSO` is mentioned as optional in the plan; this initial implementation might use simpler direct logic.