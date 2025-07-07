# Task ID: 1.4.5
# Parent Task ID: 1.4
# Title: Implement SaveLoadManager
# Status: pending
# Dependencies: 1.4.1, 1.4.2, 1.4.3, 1.4.4 # Interfaces, Events, DataServices
# Priority: critical
# Estimated Effort: L
# Assignee: Unassigned

# Description:
Implement `SaveLoadManager.cs`, a MonoBehaviour that finds all `ISaveable` components, manages save/load operations using an active `IDataService`, aggregates state, and raises save/load related events.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/Core/Persistence/SaveLoadManager.cs`
2.  **Namespace:** `PetalsOfHope.Core.Persistence`
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/Core/Persistence/SaveLoadManager.cs
    namespace PetalsOfHope.Core.Persistence
    {
        using UnityEngine;
        using System.Collections.Generic;
        using System.Linq;
        using PetalsOfHope.Core.Persistence.Interfaces;
        using PetalsOfHope.Core.Persistence.Services; // For default service
        using PetalsOfHope.Core.Events; // For GameEventSO

        public class SaveLoadManager : MonoBehaviour // Consider Singleton pattern if globally accessible
        {
            [Header("Data Service Configuration")]
            [Tooltip("The data service to use. If null, will default to JsonDataService.")]
            [SerializeField] private ScriptableObject _dataServiceSO; // To allow assigning SO wrappers if IDataService is an SO
                                                                    // Or just have a direct field for IDataService if it's not an SO.
                                                                    // For simplicity, let's assume we'll instantiate it directly or have a selector.
            private IDataService _activeDataService;

            [Tooltip("Filename for the main save game data.")]
            [SerializeField] private string _saveFileName = "game_save_data";
            [Tooltip("Whether to encrypt the save data.")]
            [SerializeField] private bool _encryptData = false;


            [Header("Save/Load Events")]
            [SerializeField] private GameEventSO _onBeforeSaveGameEventSO;
            [SerializeField] private GameEventSO _onAfterSaveGameEventSO; // Added from 1.4.2
            [SerializeField] private GameEventSO _onAfterLoadGameEventSO;
            [SerializeField] private GameEventSO _onSaveFailedEventSO;
            [SerializeField] private GameEventSO _onLoadFailedEventSO;

            private List<ISaveable> _saveableEntities;

            // Consider a public static instance for easy access (Singleton-like)
            public static SaveLoadManager Instance { get; private set; }

            private void Awake()
            {
                if (Instance != null && Instance != this)
                {
                    Destroy(gameObject);
                    return;
                }
                Instance = this;
                DontDestroyOnLoad(gameObject); // Persist across scenes

                InitializeDataService();
                FindAllSaveableEntities();
            }

            private void InitializeDataService()
            {
                // This part needs refinement based on how IDataService is provided.
                // Option 1: SerializeField for specific service type (e.g., PlayerPrefsDataService playerPrefsService)
                // Option 2: A ScriptableObject that IS an IDataService or provides one.
                // Option 3: Enum to select and instantiate.
                // For now, defaulting to JsonDataService if nothing is explicitly set.
                if (_dataServiceSO != null && _dataServiceSO is IDataService service)
                {
                    _activeDataService = service;
                }
                else
                {
                     // Default to JsonDataService if no specific service is configured or SO is not IDataService
                    _activeDataService = new JsonDataService(); // Or PlayerPrefsDataService
                    Debug.Log("No IDataService configured, defaulting to JsonDataService.");
                }
            }

            public void RegisterSaveable(ISaveable saveable)
            {
                if (_saveableEntities == null) _saveableEntities = new List<ISaveable>();
                if (!_saveableEntities.Contains(saveable))
                {
                    _saveableEntities.Add(saveable);
                }
            }

            public void UnregisterSaveable(ISaveable saveable)
            {
                _saveableEntities?.Remove(saveable);
            }

            // Call this on scene load or when new saveables are dynamically created
            public void FindAllSaveableEntities()
            {
                // Find ISaveable in MonoBehaviours
                _saveableEntities = FindObjectsOfType<MonoBehaviour>(true) // true to include inactive
                                      .OfType<ISaveable>()
                                      .ToList();
                // Potentially find ISaveable in ScriptableObjects if they are managed globally
                // This part needs a strategy for SOs if they need to be saved globally.
                Debug.Log($"Found {_saveableEntities.Count} ISaveable entities.");
            }

            public void SaveGame()
            {
                _onBeforeSaveGameEventSO?.Raise();

                // Re-scan just in case new objects appeared or disappeared without proper registration
                // Or rely on manual registration/unregistration for dynamic objects.
                // For simplicity now, we can re-scan or assume _saveableEntities is up-to-date.
                // FindAllSaveableEntities(); // Optional: re-scan on save

                Dictionary<string, object> gameState = new Dictionary<string, object>();
                foreach (ISaveable saveable in _saveableEntities)
                {
                    if (string.IsNullOrEmpty(saveable.UniqueID))
                    {
                        Debug.LogWarning($"ISaveable object {saveable.GetType().Name} has a null or empty UniqueID and will not be saved.", saveable as MonoBehaviour);
                        continue;
                    }
                    if (gameState.ContainsKey(saveable.UniqueID)) {
                        Debug.LogWarning($"Duplicate UniqueID '{saveable.UniqueID}' found for {saveable.GetType().Name}. Only the first will be saved. Ensure UniqueIDs are truly unique.", saveable as MonoBehaviour);
                        continue;
                    }
                    gameState[saveable.UniqueID] = saveable.CaptureState();
                }

                bool success = _activeDataService.Save(_saveFileName, gameState, _encryptData);

                if (success)
                {
                    Debug.Log("Game saved successfully to: " + _saveFileName);
                    _onAfterSaveGameEventSO?.Raise();
                }
                else
                {
                    Debug.LogError("Failed to save game.");
                    _onSaveFailedEventSO?.Raise();
                }
            }

            public void LoadGame()
            {
                if (!_activeDataService.HasKey(_saveFileName))
                {
                    Debug.LogWarning($"No save file found with name: {_saveFileName}. Cannot load game.");
                    _onLoadFailedEventSO?.Raise(); // Or a specific "NoSaveFileFoundEvent"
                    return;
                }

                Dictionary<string, object> gameState = _activeDataService.Load<Dictionary<string, object>>(_saveFileName, _encryptData);

                if (gameState == null)
                {
                    Debug.LogError("Failed to load game data. File might be corrupt or empty.");
                    _onLoadFailedEventSO?.Raise();
                    return;
                }

                // Re-scan for saveables as the scene might have changed or objects might not be ready on Awake.
                // This ensures we try to restore state to all currently available ISaveables.
                FindAllSaveableEntities();

                foreach (ISaveable saveable in _saveableEntities)
                {
                     if (string.IsNullOrEmpty(saveable.UniqueID))
                    {
                        Debug.LogWarning($"ISaveable object {saveable.GetType().Name} has a null or empty UniqueID and cannot be loaded.", saveable as MonoBehaviour);
                        continue;
                    }
                    if (gameState.TryGetValue(saveable.UniqueID, out object savedState))
                    {
                        saveable.RestoreState(savedState);
                    }
                    else
                    {
                        // Debug.LogWarning($"No saved state found for UniqueID: {saveable.UniqueID} ({saveable.GetType().Name})");
                        // This can be normal if new ISaveable objects are added to the game after a save was made.
                        // Or if an object was removed.
                    }
                }

                Debug.Log("Game loaded successfully from: " + _saveFileName);
                _onAfterLoadGameEventSO?.Raise();
            }

            public void DeleteSave()
            {
                bool deleted = _activeDataService.Delete(_saveFileName);
                if (deleted)
                {
                    Debug.Log($"Save file '{_saveFileName}' deleted successfully.");
                }
                else
                {
                    Debug.LogError($"Failed to delete save file '{_saveFileName}'.");
                }
            }

            public bool HasSaveData()
            {
                return _activeDataService.HasKey(_saveFileName);
            }
        }
    }
    ```

# Acceptance Criteria:
- `SaveLoadManager.cs` is a MonoBehaviour, potentially a singleton.
- It can hold a reference to an active `IDataService`. Defaults to `JsonDataService` or `PlayerPrefsDataService` if none specified.
- `FindAllSaveableEntities()` correctly discovers components implementing `ISaveable` in the current scene.
- `SaveGame()`:
    - Raises `_onBeforeSaveGameEventSO`.
    - Iterates through `ISaveable` entities, calls `CaptureState()`, and aggregates data into a dictionary.
    - Uses the active `IDataService` to save the aggregated state.
    - Raises `_onAfterSaveGameEventSO` on success or `_onSaveFailedEventSO` on failure.
- `LoadGame()`:
    - Uses active `IDataService` to load the aggregated state.
    - Iterates through `ISaveable` entities and calls `RestoreState(object state)` with the appropriate data.
    - Raises `_onAfterLoadGameEventSO` on successful load and restoration.
    - Raises `_onLoadFailedEventSO` if loading fails or no save data is found.
- `DeleteSave()` uses the active `IDataService` to delete the specified save file.
- `HasSaveData()` correctly checks for the existence of save data.
- Handles cases where `UniqueID`s are missing or duplicated with warnings.

# Test Strategy:
- Integration Testing:
    - Create a test scene with a `SaveLoadManager` and several GameObjects with simple `ISaveable` components.
    - Test `SaveGame()`: Verify a save file is created (if using `JsonDataService`) and events are raised.
    - Modify the state of `ISaveable` components.
    - Test `LoadGame()`: Verify components' states are restored to saved values and events are raised.
    - Test `DeleteSave()` and confirm with `HasSaveData()`.
    - Test with both `PlayerPrefsDataService` and `JsonDataService` (by configuring the manager).
    - Test behavior when no save file exists.
    - Test behavior with dynamic registration/unregistration of `ISaveable` objects (if that feature is fully implemented).

# Notes/Questions:
- **Singleton Pattern**: Implemented a basic singleton pattern for easy global access.
- **`IDataService` Configuration**: The `InitializeDataService` method has a placeholder for how the service is chosen. This needs to be refined. Using an enum in the inspector to pick between `PlayerPrefsDataService` and `JsonDataService` and then instantiating it could be a simple approach. Or a `ScriptableObject` that *is* an `IDataService` could be assigned. The current implementation instantiates `JsonDataService` as a default.
- **`ISaveable` ScriptableObjects**: The current `FindAllSaveableEntities` only finds `MonoBehaviour`s. If `ScriptableObject` assets also need to implement `ISaveable` and be part of the global save, a strategy to find and manage them is required (e.g., a central registry or loading all SOs of a certain type from Resources/Addressables). For now, focused on `MonoBehaviour`s as per typical entity state.
- **`UniqueID` Management**: This is critical. A separate system or tool (e.g., `UniqueId.cs` component that auto-generates and stores a GUID, ensuring it's stable in prefabs and scene instances) is often needed for `MonoBehaviour` `ISaveable`s in scenes. This task assumes such IDs will be available.
- **Dynamic `ISaveable`s**: Added `RegisterSaveable`/`UnregisterSaveable` for objects created/destroyed at runtime. `FindAllSaveableEntities` should be called strategically (e.g., on scene load) or these methods must be used diligently.
- The plan states the manager "Finds all ISaveable components." - initial implementation focuses on this.