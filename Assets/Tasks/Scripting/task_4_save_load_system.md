# Task ID: 4
# Parent Task ID: None
# Title: Save/Load System
# Status: completed
# Dependencies: 1, 2, 3
# Priority: critical
# Estimated Effort: M
# Assignee: AI

# Description:
Implement the Save/Load System to manage game data persistence.

# Details:
1. Define `IDataService.cs` and `ISaveable.cs` interfaces in `_Project/Scripts/Core/Persistence/Interfaces/`.
2. Implement `PlayerPrefsDataService.cs` and `JsonDataService.cs` in `_Project/Scripts/Core/Persistence/Services/`.
3. Implement `SaveLoadManager.cs` (MonoBehaviour) in `_Project/Scripts/Core/Persistence/`.
4. Integrate Save/Load System with the Event Bus for save/load events.
5. Unit test data serialization and deserialization logic.

# Acceptance Criteria:
- `IDataService` and `ISaveable` interfaces are defined correctly.
- `PlayerPrefsDataService` and `JsonDataService` are implemented correctly.
- `SaveLoadManager` is functional and manages save/load operations.
- Save/Load System is integrated with the Event Bus.
- Unit tests for data serialization and deserialization pass.

# Test Strategy:
- Unit tests for save/load logic.
- Manual testing of save/load functionality in the Unity Editor.

# Notes/Questions:
- Ensure that the Save/Load System is robust and handles edge cases.
- Verify that the Event Bus integration is correct and functional.
