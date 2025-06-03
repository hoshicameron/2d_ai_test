# Task ID: 4.5.2
# Parent Task ID: 4.5
# Title: (Optional) Implement LevelUnlockConditionSO
# Status: pending
# Dependencies: 4.5.1 # GameProgressionManager
# Priority: low
# Estimated Effort: M
# Assignee: Unassigned

# Description:
(Optional) Implement `LevelUnlockConditionSO.cs` (or a more general `UnlockConditionSO`) to define complex unlock logic for levels or abilities in a data-driven way, instead of hardcoding in `GameProgressionManager`.

# Details:
This task is optional as per the Implementation Plan. If undertaken:
1.  **Define `UnlockConditionSO.cs` (Abstract Base):**
    *   File Location: `Assets/_Project/Scripts/Systems/Progression/Conditions/UnlockConditionSO.cs`
    *   Namespace: `PetalsOfHope.Systems.Progression.Conditions`
    *   Abstract `ScriptableObject` with a method like `public abstract bool IsMet(GameProgressionManager progressionManager);`
2.  **Define Concrete Condition Types:**
    *   `TalismanRequirementSO : UnlockConditionSO`: Checks if specific talismans are collected (via `InventorySystem` or `GameProgressionManager` querying `InventorySystem`).
    *   `LevelCompletedRequirementSO : UnlockConditionSO`: Checks if another level/quest is completed.
    *   `SpecificEventOccurredSO : UnlockConditionSO`: Listens for a specific `GameEventSO`.
3.  **Modify `GameProgressionManager`:**
    *   Instead of hardcoded `OnTalismanAwarded` logic, `GameProgressionManager` could iterate through a list of `UnlockableItemSO`s.
    *   Each `UnlockableItemSO` (e.g., `UnlockableAbilitySO`, `UnlockableLevelSO`) would contain a reference to the item to unlock (e.g., Ability ID, `SceneDataSO`) and a list of `UnlockConditionSO` assets.
    *   When relevant game events occur (like `TalismanAwarded`), `GameProgressionManager` re-evaluates conditions for all not-yet-unlocked items.

# Acceptance Criteria:
- (If implemented) `UnlockConditionSO` base class and concrete condition types are created.
- (If implemented) `GameProgressionManager` can use these condition SOs to determine unlocks in a data-driven way.
- (If implemented) Unlock logic is more flexible and configurable by designers.

# Test Strategy:
- Manual Testing:
    - Create SO assets for unlock conditions and link them to unlockable items.
    - Trigger game events or collect items that satisfy these conditions.
    - Verify `GameProgressionManager` correctly unlocks the associated levels/abilities.

# Notes/Questions:
- This adds significant flexibility but also complexity.
- For the initial implementation, the simpler hardcoded or event-driven logic in `GameProgressionManager.OnTalismanAwarded` (Task 4.5.1) might be sufficient.
- This task will be skipped unless explicitly requested or deemed necessary for managing complex unlock requirements.