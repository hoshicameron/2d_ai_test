# Task ID: 4.3.2
# Parent Task ID: 4.3
# Title: Implement InventorySystem
# Status: pending
# Dependencies: 4.3.1, 1.2 # TalismanDataSO, Event Bus (for TalismanAwardedEventSO)
# Priority: high
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement `InventorySystem.cs` to manage the collection of `TalismanDataSO`s acquired by the player. It should provide a method to add talismans and raise an event upon successful addition.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/Systems/Inventory/InventorySystem.cs`
2.  **Namespace:** `PetalsOfHope.Systems.Inventory`
3.  **Create `TalismanAwardedEventSO`:**
    *   Type: `TypedEventSO<TalismanDataSO>` (payload: the awarded talisman data).
    *   Location: `Assets/_Project/ScriptableObjects/Events/Inventory/TalismanAwardedEventSO.asset`
    *   Name: `TalismanAwardedEventSO`.
    *   Developer Description: "Raised when a talisman is successfully added to the player's inventory."
    *   Ensure `TalismanDataSOEventSO : TypedEventSO<TalismanDataSO> {}` concrete class exists as per Task 1.2.3 pattern.

4.  **Implementation of `InventorySystem.cs`:**
    ```csharp
    // In Assets/_Project/Scripts/Systems/Inventory/InventorySystem.cs
    namespace PetalsOfHope.Systems.Inventory
    {
        using UnityEngine;
        using System.Collections.Generic;
        using PetalsOfHope.Data.Collectibles; // For TalismanDataSO
        using PetalsOfHope.Core.Events;       // For TypedEventSO
        using PetalsOfHope.Core.Persistence.Interfaces; // For ISaveable

        // Concrete class for TalismanDataSO event type
        [CreateAssetMenu(menuName = "Petals of Hope/Events/Typed Events/TalismanDataSO Event", fileName = "NewTalismanDataSOEventSO")]
        public class TalismanDataSOEvent : TypedEventSO<TalismanDataSO> {}


        public class InventorySystem : MonoBehaviour, ISaveable // Make it ISaveable for Task 3.4.5
        {
            [Header("Events")]
            [Tooltip("Event raised when a talisman is awarded.")]
            [SerializeField] private TalismanDataSOEvent _talismanAwardedEventSO; // Assign the created SO asset

            private HashSet<TalismanDataSO> _collectedTalismans = new HashSet<TalismanDataSO>();
            // Using HashSet to automatically handle duplicates if TalismanDataSO.Equals/GetHashCode are well-defined.
            // Or List<TalismanDataSO> if order matters or duplicates are allowed/handled differently.

            public static InventorySystem Instance { get; private set; } // Basic Singleton for easy access

            // --- ISaveable Implementation ---
            public string UniqueID => "InventorySystem_Global"; // Fixed ID for a global system

            public object CaptureState()
            {
                // We need to save talisman IDs, not the SO references directly, for robust serialization.
                List<string> talismanIDs = new List<string>();
                foreach (TalismanDataSO talisman in _collectedTalismans)
                {
                    if (talisman != null && !string.IsNullOrEmpty(talisman.talismanID))
                    {
                        talismanIDs.Add(talisman.talismanID);
                    }
                }
                return new InventorySaveData { CollectedTalismanIDs = talismanIDs };
            }

            public void RestoreState(object state)
            {
                if (state is InventorySaveData saveData)
                {
                    _collectedTalismans.Clear();
                    // Need a way to map talisman IDs back to TalismanDataSO assets.
                    // This typically involves a "TalismanDatabase" or loading all TalismanDataSO assets from Resources/Addressables.
                    // For now, placeholder - actual loading of SOs from IDs needs a robust solution.
                    Debug.LogWarning("InventorySystem.RestoreState: TalismanDataSO loading from IDs is not fully implemented. Requires a database/asset loading mechanism.");
                    // Example (requires TalismanDatabaseSO.cs or similar):
                    // foreach (string id in saveData.CollectedTalismanIDs)
                    // {
                    //    TalismanDataSO talismanSO = TalismanDatabaseSO.Instance.GetTalismanByID(id);
                    //    if (talismanSO != null) _collectedTalismans.Add(talismanSO);
                    // }
                }
                // After restoring, could raise an event like "InventoryLoaded" if UI needs to refresh.
            }

            [System.Serializable]
            private struct InventorySaveData
            {
                public List<string> CollectedTalismanIDs;
            }
            // --- End ISaveable Implementation ---


            private void Awake()
            {
                if (Instance == null)
                {
                    Instance = this;
                    DontDestroyOnLoad(gameObject); // Persist across scenes
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            
            /// <summary>
            /// Adds a talisman to the inventory if not already collected.
            /// Raises the TalismanAwardedEventSO on successful addition.
            /// </summary>
            /// <param name="talismanData">The TalismanDataSO to add.</param>
            /// <returns>True if the talisman was newly added, false if already present or null.</returns>
            public bool AddTalisman(TalismanDataSO talismanData)
            {
                if (talismanData == null)
                {
                    Debug.LogWarning("Attempted to add a null talisman.");
                    return false;
                }

                if (_collectedTalismans.Add(talismanData)) // HashSet.Add returns true if item was added, false if already present
                {
                    Debug.Log($"Talisman '{talismanData.displayName}' collected!");
                    _talismanAwardedEventSO?.Raise(talismanData);
                    return true;
                }
                else
                {
                    Debug.Log($"Talisman '{talismanData.displayName}' already collected.");
                    return false;
                }
            }

            /// <summary>
            /// Checks if a specific talisman has been collected.
            /// </summary>
            /// <param name="talismanData">The TalismanDataSO to check for.</param>
            /// <returns>True if collected, false otherwise.</returns>
            public bool HasTalisman(TalismanDataSO talismanData)
            {
                if (talismanData == null) return false;
                return _collectedTalismans.Contains(talismanData);
            }
            
            /// <summary>
            /// Checks if a talisman with the given ID has been collected.
            /// </summary>
            /// <param name="talismanID">The ID of the talisman to check for.</param>
            /// <returns>True if collected, false otherwise.</returns>
            public bool HasTalisman(string talismanID)
            {
                 if (string.IsNullOrEmpty(talismanID)) return false;
                 foreach (var talisman in _collectedTalismans)
                 {
                     if (talisman.talismanID == talismanID) return true;
                 }
                 return false;
            }

            public IReadOnlyCollection<TalismanDataSO> GetCollectedTalismans()
            {
                return _collectedTalismans;
            }
        }
    }
    ```
5.  **Mechanism for Awarding Talismans:**
    *   This task focuses on `InventorySystem`. The actual awarding happens elsewhere.
    *   Example: After defeating a boss, the boss's script or a `GameManager` might call:
        `InventorySystem.Instance.AddTalisman(someBossTalismanSO);`
    *   This will be triggered by events like `BossDefeatedEventSO` (as per plan).

# Acceptance Criteria:
- `TalismanAwardedEventSO` (TypedEventSO<TalismanDataSO>) and its concrete class `TalismanDataSOEvent` are created.
- `InventorySystem.cs` (as a Singleton MonoBehaviour) is implemented.
- `AddTalisman(TalismanDataSO)` method adds talisman to an internal collection (e.g., `HashSet`) and raises `_talismanAwardedEventSO` if successful.
- `HasTalisman(TalismanDataSO)` and `HasTalisman(string talismanID)` methods correctly check for collected talismans.
- Basic `ISaveable` implementation is present for saving/loading talisman IDs (full testing of this part with `SaveLoadManager` in Task 3.4.5).
- Script compiles without errors.

# Test Strategy:
- Manual Testing:
    - Create an `InventorySystem` GameObject in a test scene. Assign the `TalismanAwardedEventSO`.
    - Create a few `TalismanDataSO` assets.
    - Create a test script with a reference to `InventorySystem.Instance` and the talisman SOs.
    - Call `InventorySystem.Instance.AddTalisman()` with a talisman SO.
        - Verify console log for collection.
        - Verify `_talismanAwardedEventSO` is raised (use an EventListener to log it).
    - Call `AddTalisman()` again with the same SO: verify it's not re-added and event not re-raised (or handled as "already collected").
    - Call `HasTalisman()` and verify correct return values.
- The `ISaveable` part requires a "Talisman Database" to map IDs back to SOs during `RestoreState`. This database is not yet defined, so `RestoreState` will be a placeholder. Task 4.3.2.1 can address this.

# Notes/Questions:
- Using `HashSet<TalismanDataSO>` for `_collectedTalismans` is efficient for checking `Contains` and avoiding duplicates, provided `TalismanDataSO.Equals()` and `GetHashCode()` are implemented correctly (e.g., based on `talismanID`).
- **Crucial for Save/Load:** The `RestoreState` method needs a way to get `TalismanDataSO` instances from their saved IDs. This usually involves a "database" ScriptableObject that holds references to all `TalismanDataSO`s in the game, or loading them from a Resources folder / Addressables by ID. This is a missing piece that needs to be addressed for full save/load functionality. I will add a sub-task 4.3.2.1 for a simple Talisman Database.
- `ISaveable` is added as per plan for Game Progression saving (Task 3.4.5).