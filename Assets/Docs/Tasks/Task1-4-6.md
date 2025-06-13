# Task ID: 1.4.6
# Parent Task ID: 1.4
# Title: (Placeholder) Implement ISaveable on Key Game Systems
# Status: completed
# Dependencies: 1.4.5 # SaveLoadManager and ISaveable interface
# Priority: high # Will become critical when those systems exist
# Estimated Effort: M (per system)
# Assignee: Unassigned

# Description:
This is a placeholder task to track the future work of implementing the `ISaveable` interface on key game systems as they are developed. The actual implementation will occur within the tasks for those systems (e.g., PlayerHealth, InventorySystem, GameProgressionManager).

# Details:
As new systems that require their state to be persisted are implemented, they must:
1.  Implement the `PetalsOfHope.Core.Persistence.Interfaces.ISaveable` interface.
2.  Provide a robust `UniqueID` property.
    *   For scene `MonoBehaviour`s, this often involves a utility to generate and serialize a persistent GUID.
    *   For global managers (often singletons or ScriptableObjects), a fixed string ID can be used.
3.  Implement `CaptureState()`:
    *   Create a serializable data structure (class or struct) to hold the state.
    *   Populate and return this structure.
    *   Example: `PlayerHealth` might save `_currentHealth`.
4.  Implement `RestoreState(object state)`:
    *   Cast the `state` object back to the expected data structure.
    *   Apply the loaded values to the component's fields.
    *   Update any dependent UI or game logic if necessary (often done by listening to an `OnAfterLoad` event or specific data changed events).

**Systems identified in the Implementation Plan that will likely need to be `ISaveable`:**
-   `PlayerHealth.cs` (Phase 2.2.5) - to save current health.
-   `InventorySystem.cs` (Phase 3.4.3) - to save collected talismans/items.
-   `GameProgressionManager.cs` (Phase 3.4.5) - to save unlocked levels, abilities, and other progression flags.
-   Potentially `PlayerController.cs` or a dedicated `PlayerData.cs` for player position (if saving mid-level is required, though often games save at checkpoints/level start).
-   Any system managing currency, experience points, quest status, etc.

# Acceptance Criteria:
- (For each relevant system) The system correctly implements `ISaveable`.
- (For each relevant system) Its state is successfully saved by `SaveLoadManager`.
- (For each relevant system) Its state is successfully restored by `SaveLoadManager` upon loading a game.
- Data integrity is maintained across save/load cycles for these systems.

# Test Strategy:
- This will be tested as part of the integration testing for each system that implements `ISaveable`.
- Steps:
    1. Progress game to change the state of the `ISaveable` system.
    2. Save the game.
    3. Reload the game (or restart and load).
    4. Verify the system's state is correctly restored.

# Notes/Questions:
- This task itself doesn't involve coding but serves as a reminder and a link to where `ISaveable` will be applied.
- The `UniqueID` strategy is paramount. For scene objects, consider using a pre-made asset from the Asset Store for GUID generation/management or implement a custom one that handles prefab instances correctly.
- Data versioning within the `CaptureState` objects might become necessary if the structure of saved data changes significantly during development to maintain compatibility with older saves (more advanced topic).