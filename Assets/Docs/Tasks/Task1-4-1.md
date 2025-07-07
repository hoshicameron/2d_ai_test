# Task ID: 1.4.1
# Parent Task ID: 1.4
# Title: Define IDataService and ISaveable Interfaces
# Status: completed
# Dependencies: 1.1.2, 1.1.4 # Folder structure and namespace
# Priority: critical
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Define the `IDataService.cs` interface for data storage operations and the `ISaveable.cs` interface for objects that can persist their state.

# Details:
1.  **Define `IDataService.cs`:**
    *   File Location: `Assets/_Project/Scripts/Core/Persistence/Interfaces/IDataService.cs`
    *   Namespace: `PetalsOfHope.Core.Persistence.Interfaces`
    *   Implementation:
        ```csharp
        // In Assets/_Project/Scripts/Core/Persistence/Interfaces/IDataService.cs
        namespace PetalsOfHope.Core.Persistence.Interfaces
        {
            /// <summary>
            /// Interface for data persistence services.
            /// Defines methods for saving, loading, and deleting data.
            /// </summary>
            public interface IDataService
            {
                /// <summary>
                /// Saves data to a persistent storage.
                /// </summary>
                /// <typeparam name="T">The type of data to save.</typeparam>
                /// <param name="key">A unique key to identify the data.</param>
                /// <param name="data">The data object to save.</param>
                /// <param name="encrypt">Whether to encrypt the data before saving.</param>
                /// <returns>True if saving was successful, false otherwise.</returns>
                bool Save<T>(string key, T data, bool encrypt = false);

                /// <summary>
                /// Loads data from persistent storage.
                /// </summary>
                /// <typeparam name="T">The type of data to load.</typeparam>
                /// <param name="key">The unique key identifying the data.</param>
                /// <param name="decrypt">Whether to decrypt the data after loading.</param>
                /// <returns>The loaded data object, or default(T) if not found or an error occurs.</returns>
                T Load<T>(string key, bool decrypt = false);

                /// <summary>
                /// Deletes specific data associated with a key.
                /// </summary>
                /// <param name="key">The key of the data to delete.</param>
                /// <returns>True if deletion was successful or key did not exist, false on error.</returns>
                bool Delete(string key);

                /// <summary>
                /// Deletes all saved data managed by this service.
                /// </summary>
                /// <returns>True if deletion was successful, false on error.</returns>
                bool DeleteAll();

                /// <summary>
                /// Checks if data exists for a given key.
                /// </summary>
                /// <param name="key">The key to check.</param>
                /// <returns>True if data exists, false otherwise.</returns>
                bool HasKey(string key);
            }
        }
        ```

2.  **Define `ISaveable.cs`:**
    *   File Location: `Assets/_Project/Scripts/Core/Persistence/Interfaces/ISaveable.cs`
    *   Namespace: `PetalsOfHope.Core.Persistence.Interfaces`
    *   Implementation:
        ```csharp
        // In Assets/_Project/Scripts/Core/Persistence/Interfaces/ISaveable.cs
        namespace PetalsOfHope.Core.Persistence.Interfaces
        {
            /// <summary>
            /// Interface for objects that can have their state saved and loaded.
            /// </summary>
            public interface ISaveable
            {
                /// <summary>
                /// A unique identifier for this saveable object/entity.
                /// Used to map saved data back to the correct object.
                /// For scene objects, this should be persistent across sessions.
                /// </summary>
                string UniqueID { get; }

                /// <summary>
                /// Captures the current state of the object to be saved.
                /// </summary>
                /// <returns>An object representing the state (e.g., a custom data class or dictionary).</returns>
                object CaptureState();

                /// <summary>
                /// Restores the state of the object from the provided data.
                /// </summary>
                /// <param name="state">The state object previously captured by CaptureState().</param>
                void RestoreState(object state);
            }
        }
        ```

# Acceptance Criteria:
- `IDataService.cs` interface is created with `Save<T>`, `Load<T>`, `Delete`, `DeleteAll`, and `HasKey` methods defined.
- `ISaveable.cs` interface is created with `UniqueID { get; }`, `CaptureState()`, and `RestoreState(object state)` members defined.
- Both interfaces are in the correct namespace and file location.
- Scripts compile without errors.
- Method signatures include optional encryption/decryption parameters for `IDataService`.

# Test Strategy:
- Manual Verification: Review interface definitions for completeness and correctness.
- These interfaces will be tested implicitly when their implementing classes and the `SaveLoadManager` are tested.

# Notes/Questions:
- The `UniqueID` in `ISaveable` is crucial. For `MonoBehaviour`s in scenes, a robust way to generate and persist these IDs is needed (e.g., a custom editor tool to assign GUIDs or use a pre-existing asset like "UniqueIdGenerator"). For ScriptableObjects or other data, the asset path or a manually assigned ID can be used.
- The `encrypt` and `decrypt` parameters in `IDataService` are placeholders for future potential encryption implementation. The core services might not implement encryption initially but the interface supports it.