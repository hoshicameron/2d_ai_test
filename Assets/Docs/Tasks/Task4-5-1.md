# Task ID: 4.5.1
# Parent Task ID: 4.5
# Title: Implement GameProgressionManager Core Logic
# Status: pending
# Dependencies: 4.3.2 (InventorySystem), 1.2.8 (EventSO assets)
# Priority: critical
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement the core logic for `GameProgressionManager.cs`. This includes data structures to store unlocked levels and abilities, methods to check their status, and listeners for events that might trigger progression changes (e.g., `TalismanAwardedEventSO`).

# Details:
1.  **File Location:** `Assets/_Project/Scripts/Systems/Progression/GameProgressionManager.cs`
2.  **Namespace:** `PetalsOfHope.Systems.Progression`
3.  **Define `AbilityID` convention:**
    *   Use string constants as defined in `PlayerAbilities` (Task 4.4.1) e.g., `PlayerAbilities.DOUBLE_JUMP_ABILITY_ID`.
4.  **Define Event SOs for Unlocking:**
    *   `LevelUnlockedEventSO`: `TypedEventSO<SceneDataSO>` (or `string` scene name).
        *   Asset: `Assets/_Project/ScriptableObjects/Events/Progression/LevelUnlockedEventSO.asset`
        *   Concrete class: `SceneDataSOEvent : TypedEventSO<SceneDataSO> {}` (or `StringEventSO` if string payload).
    *   `AbilityUnlockedEventSO`: `TypedEventSO<string>` (payload: ability ID).
        *   Asset: `Assets/_Project/ScriptableObjects/Events/Progression/AbilityUnlockedEventSO.asset`
        *   Concrete class: `StringEventSO : TypedEventSO<string> {}` (ensure this exists from Task 1.2.3).

5.  **Implementation of `GameProgressionManager.cs`:**
    ```csharp
    // In Assets/_Project/Scripts/Systems/Progression/GameProgressionManager.cs
    namespace PetalsOfHope.Systems.Progression
    {
        using UnityEngine;
        using System.Collections.Generic;
        using PetalsOfHope.Systems.SceneManagement; // For SceneDataSO
        using PetalsOfHope.Data.Collectibles;    // For TalismanDataSO
        using PetalsOfHope.Systems.Inventory;     // For InventorySystem and TalismanDataSOEvent
        using PetalsOfHope.Core.Events;           // For TypedEventSO derivatives
        using PetalsOfHope.Core.Persistence.Interfaces; // For ISaveable

        // Concrete class for SceneDataSO event type if not already globally defined
        // [CreateAssetMenu(menuName = "Petals of Hope/Events/Typed Events/SceneDataSO Event", fileName = "NewSceneDataSOEventSO")]
        // public class SceneDataSOEvent : TypedEventSO<SceneDataSO> {} // Already handled this in InventorySystem for TalismanDataSOEvent, assuming pattern is followed

        public class GameProgressionManager : MonoBehaviour, ISaveable
        {
            [Header("Event Listeners")]
            [Tooltip("Event raised when a talisman is awarded. Used to check for ability unlocks.")]
            [SerializeField] private TalismanDataSOEvent _talismanAwardedEventSO; // Listen

            [Header("Event Raisers")]
            [Tooltip("Event raised when a new level is unlocked.")]
            [SerializeField] private SceneDataSOEvent _levelUnlockedEventSO; // Raise (ensure SceneDataSOEvent exists)
            [Tooltip("Event raised when a new ability is unlocked.")]
            [SerializeField] private StringEventSO _abilityUnlockedEventSO;  // Raise (ensure StringEventSO exists)
            
            [Header("Progression Data (Initial/Default State)")]
            [Tooltip("List of SceneDataSO assets that are unlocked by default at the start of a new game.")]
            [SerializeField] private List<SceneDataSO> _defaultUnlockedLevels;
            [Tooltip("List of Ability IDs (strings) that are unlocked by default at the start of a new game.")]
            [SerializeField] private List<string> _defaultUnlockedAbilities;

            // Runtime data
            private HashSet<string> _unlockedLevelSceneNames = new HashSet<string>();
            private HashSet<string> _unlockedAbilityIDs = new HashSet<string>();

            // Dependencies
            // private InventorySystem _inventorySystem; // Not directly needed if listening to event

            public static GameProgressionManager Instance { get; private set; }

            // --- ISaveable Implementation ---
            public string UniqueID => "GameProgressionManager_Global";

            public object CaptureState()
            {
                return new ProgressionSaveData
                {
                    UnlockedLevelNames = new List<string>(_unlockedLevelSceneNames),
                    UnlockedAbilityIDs = new List<string>(_unlockedAbilityIDs)
                };
            }

            public void RestoreState(object state)
            {
                if (state is ProgressionSaveData saveData)
                {
                    _unlockedLevelSceneNames = new HashSet<string>(saveData.UnlockedLevelNames);
                    _unlockedAbilityIDs = new HashSet<string>(saveData.UnlockedAbilityIDs);
                    Debug.Log("Game progression restored.");
                }
                else // New game or no save data
                {
                    InitializeDefaultProgression();
                }
                // Refresh any UI or game state based on loaded progression
                // e.g., re-raise unlock events for already unlocked items if systems need to re-initialize
            }
            
            private void InitializeDefaultProgression()
            {
                _unlockedLevelSceneNames.Clear();
                _unlockedAbilityIDs.Clear();

                foreach(var levelData in _defaultUnlockedLevels)
                {
                    if (levelData != null) UnlockLevel(levelData, false); // Don't raise event for defaults initially
                }
                foreach(var abilityID in _defaultUnlockedAbilities)
                {
                    UnlockAbility(abilityID, false); // Don't raise event for defaults initially
                }
                Debug.Log("Default game progression initialized.");
            }


            [System.Serializable]
            private struct ProgressionSaveData
            {
                public List<string> UnlockedLevelNames;
                public List<string> UnlockedAbilityIDs;
            }
            // --- End ISaveable Implementation ---

            private void Awake()
            {
                if (Instance == null)
                {
                    Instance = this;
                    DontDestroyOnLoad(gameObject);
                    // If not loading save data immediately in Awake (e.g. SaveLoadManager handles it later),
                    // then InitializeDefaultProgression() might be called here or on first access.
                    // For ISaveable, RestoreState will be called by SaveLoadManager.
                    // If no save data, RestoreState should call InitializeDefaultProgression.
                }
                else
                {
                    Destroy(gameObject);
                }
            }

            private void OnEnable()
            {
                _talismanAwardedEventSO?.RegisterListener(OnTalismanAwarded);
            }

            private void OnDisable()
            {
                _talismanAwardedEventSO?.UnregisterListener(OnTalismanAwarded);
            }

            private void OnTalismanAwarded(TalismanDataSO talisman)
            {
                // Example: Unlock abilities based on specific talisman IDs
                // This mapping (talisman ID to ability ID) could be data-driven (e.g., in TalismanDataSO itself or a separate mapping SO)
                if (talisman.talismanID == "TALISMAN_DOUBLE_JUMP") // Example ID from TalismanDataSO
                {
                    UnlockAbility(PlayerAbilities.PlayerAbilities.DOUBLE_JUMP_ABILITY_ID);
                }
                else if (talisman.talismanID == "TALISMAN_DASH")
                {
                    UnlockAbility(PlayerAbilities.PlayerAbilities.DASH_ABILITY_ID);
                }
                // Add more mappings for other talismans and abilities/levels
            }

            public bool IsLevelUnlocked(SceneDataSO sceneData)
            {
                if (sceneData == null) return false;
                return _unlockedLevelSceneNames.Contains(sceneData.GetSceneName());
            }
            
            public bool IsLevelUnlocked(string sceneName)
            {
                 if (string.IsNullOrEmpty(sceneName)) return false;
                 return _unlockedLevelSceneNames.Contains(sceneName);
            }

            public void UnlockLevel(SceneDataSO sceneData, bool raiseEvent = true)
            {
                if (sceneData == null) return;
                string sceneName = sceneData.GetSceneName();
                if (string.IsNullOrEmpty(sceneName)) return;

                if (_unlockedLevelSceneNames.Add(sceneName))
                {
                    Debug.Log($"Level Unlocked: {sceneData.displayName} ({sceneName})");
                    if(raiseEvent) _levelUnlockedEventSO?.Raise(sceneData);
                }
            }

            public bool IsAbilityUnlocked(string abilityID)
            {
                if (string.IsNullOrEmpty(abilityID)) return false;
                return _unlockedAbilityIDs.Contains(abilityID);
            }

            public void UnlockAbility(string abilityID, bool raiseEvent = true)
            {
                if (string.IsNullOrEmpty(abilityID)) return;

                if (_unlockedAbilityIDs.Add(abilityID))
                {
                    Debug.Log($"Ability Unlocked: {abilityID}");
                    if(raiseEvent) _abilityUnlockedEventSO?.Raise(abilityID);
                }
            }
        }
    }
    ```

# Acceptance Criteria:
- `GameProgressionManager.cs` (as a Singleton MonoBehaviour) is implemented.
- It uses `HashSet<string>` to store unlocked level scene names and ability IDs.
- `IsLevelUnlocked(SceneDataSO/string)` and `IsAbilityUnlocked(string)` methods correctly report status.
- `UnlockLevel(SceneDataSO, bool raiseEvent)` and `UnlockAbility(string, bool raiseEvent)` methods add items to sets and optionally raise `_levelUnlockedEventSO` or `_abilityUnlockedEventSO`.
- `OnTalismanAwarded(TalismanDataSO)` method implements example logic to unlock abilities based on talisman IDs.
- Default unlocked levels/abilities can be configured in the Inspector and are applied if no save data is found.
- Basic `ISaveable` structure is in place (CaptureState, RestoreState, ProgressionSaveData struct). `RestoreState` calls `InitializeDefaultProgression` if no data.
- Script compiles without errors. `SceneDataSOEvent` and `StringEventSO` concrete classes for events are defined and used.

# Test Strategy:
- Manual Testing:
    - Add `GameProgressionManager` to a persistent GameObject. Assign event SOs and default unlocks.
    - Use test scripts to call `IsLevelUnlocked`/`IsAbilityUnlocked` before and after calling `UnlockLevel`/`UnlockAbility`. Verify results.
    - Set up listeners for `_levelUnlockedEventSO` and `_abilityUnlockedEventSO` to log when they are raised. Verify they are raised correctly.
    - Mock `_talismanAwardedEventSO.Raise(someTalismanSO)` and verify that the corresponding ability gets unlocked.
- Save/Load testing will be part of Task 4.5.3.

# Notes/Questions:
- The mapping between Talisman IDs and what they unlock (abilities, levels) is currently hardcoded in `OnTalismanAwarded`. This could be made more data-driven using ScriptableObjects for defining unlock conditions (the "Optional `LevelUnlockConditionSO`" from plan).
- `PlayerAbilities.PlayerAbilities.ABILITY_ID_CONSTANTS` are used for consistency.
- `RestoreState` needs to handle the case where there's no save data, in which case it should apply default progression.