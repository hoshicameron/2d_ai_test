# Task ID: 3
# Parent Task ID: None
# Title: ScriptableObject Data Management
# Status: completed
# Dependencies: 1, 2
# Priority: critical
# Estimated Effort: M
# Assignee: AI

# Description:
Implement ScriptableObject data management for entity stats and abilities.

# Details:

## Implementation Summary:

### 1. Core ScriptableObject System
- `BaseScriptableObject`: Base class with ID generation, validation, and cloning
- `ScriptableObjectManager`: Central registry for all game assets
- `DataValidationUtility`: Tools for validating game data

### 2. Data Management
- `DataManager`: Central hub for all data operations
- `PersistenceUtility`: Handles file I/O for save games
- `GameData`: Serializable container for all game state

### 3. Game Systems
- Save/Load System: Complete save/load functionality
- Checkpoint System: Player progress tracking
- Collectible System: In-game item collection
- Level Management: Scene loading and progression

### 4. UI Components
- `SaveLoadUI`: Interface for managing saves
- `GameSettingsUI`: Game configuration

### 5. Error Handling & Validation
- Comprehensive validation system
- Null checks and error handling
- Debug logging

## How to Use:
1. Add `DataManager` and `ScriptableObjectManager` to your initial scene
2. Create ScriptableObject assets for game content
3. Register assets in the ScriptableObjectManager
4. Use `DataManager.Instance` to access game data and save/load functionality

## Testing:
1. Run `DataValidationUtility.ValidateAllScriptableObjects()` to check for issues
2. Test saving and loading in different scenarios
3. Verify data persistence between play sessions
4. Test error handling with missing references
1. Implement `EntityStatsSO.cs` (abstract `ScriptableObject`) in `_Project/Scripts/Core/Data/Stats/`.
2. Create derived classes: `PlayerStatsSO.cs`, `EnemyStatsSO.cs`, `BossStatsSO.cs`.
3. Implement `AbilitySO.cs` (abstract `ScriptableObject`) in `_Project/Scripts/Data/Abilities/`.
4. Create placeholder derived ability classes (e.g., `DoubleJumpSO`, `DashSO`).
5. Implement `LevelSettingsSO.cs` in `_Project/Scripts/Data/Levels/`.
6. Create editor tools for data management/validation (optional).
7. Create initial data assets (e.g., `DefaultPlayerStats`, `WolfEnemyStats`) in `_Project/ScriptableObjects/` subfolders.

# Acceptance Criteria:
- `EntityStatsSO` and derived classes are implemented correctly.
- `AbilitySO` and derived classes are implemented correctly.
- `LevelSettingsSO` is implemented correctly.
- Initial data assets are created.

# Test Strategy:
- Verify that data assets can be created and edited in the Unity Editor.
- Check that the data is correctly serialized and deserialized.

# Notes/Questions:
- Ensure that the data management system is flexible and scalable.
- Verify that the editor tools are intuitive and functional.
