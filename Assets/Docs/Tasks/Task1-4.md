# Task ID: 1.4
# Parent Task ID: 1
# Title: Save/Load System Implementation
# Status: completed
# Dependencies: 1.2 # Event Bus (for save/load events)
# Priority: critical
# Estimated Effort: XL (aggregate of subtasks)
# Assignee: Unassigned

# Description:
Implement a system for saving and loading game state. This includes defining interfaces for saveable objects and data services, implementing concrete data services (PlayerPrefs and JSON), and a manager to orchestrate the save/load process.

# Details:
This system is crucial for persisting player progress and game state. It will reside in `PetalsOfHope.Core.Persistence`. Key components:
- `IDataService` interface for different storage backends.
- `ISaveable` interface for objects that need to persist their state.
- `PlayerPrefsDataService` and `JsonDataService` implementations.
- `SaveLoadManager` to coordinate saving and loading across all `ISaveable` objects.
- Integration with the Event Bus for notifications (e.g., `OnBeforeSave`, `OnAfterLoad`).

Refer to subtasks 1.4.1 through 1.4.6 for specific implementation details.

# Acceptance Criteria:
- All subtasks (1.4.1 - 1.4.6) are completed.
- The game can save its state to a persistent medium (PlayerPrefs or JSON file).
- The game can load a previously saved state, restoring relevant game objects and data.
- The system is flexible enough to support different data storage methods via `IDataService`.
- `ISaveable` objects correctly capture and restore their state.
- Save/load operations trigger appropriate events.

# Test Strategy:
- Integration Testing:
    - Implement `ISaveable` on a few simple components/SOs.
    - Modify their state, save the game, close and reopen (or reload), and verify the state is restored.
    - Test both `PlayerPrefsDataService` and `JsonDataService`.
    - Verify that save/load events are raised and can be listened to.
- Test edge cases: no save file found, corrupted save file (manual simulation if `JsonDataService` is used).

# Notes/Questions:
- This is a critical system for player experience. Robustness and error handling are important.