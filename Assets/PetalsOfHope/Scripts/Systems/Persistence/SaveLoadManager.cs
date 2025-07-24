using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using PetalsOfHope.Systems.Persistence.Services;
using PetalsOfHope.Core.Events;
using System;
using PetalsOfHope.Interfaces;

namespace PetalsOfHope.Systems.Persistence
{
    /// <summary>
    /// Manages saving and loading game state using the active IDataService.
    /// </summary>
    public class SaveLoadManager : MonoBehaviour
    {
        [Header("Data Service Configuration")]
        [Tooltip("The data service to use. If null, will default to creating a JsonDataServiceSO at runtime.")]
        [SerializeField] private IDataServiceSO _dataServiceSO;
        
        [Tooltip("Filename for the main save game data.")]
        [SerializeField] private string _saveFileName = "game_save_data";
        
        [Tooltip("Whether to encrypt the save data.")]
        [SerializeField] private bool _encryptData = false;

        [Header("Save/Load Events")]
        [SerializeField] private OnBeforeSaveGameEventSO _onBeforeSaveGameEventSO;
        [SerializeField] private OnAfterSaveGameEventSO _onAfterSaveGameEventSO;
        [SerializeField] private OnAfterLoadGameEventSO _onAfterLoadGameEventSO;
        [SerializeField] private OnSaveFailedEventSO _onSaveFailedEventSO;
        [SerializeField] private OnLoadFailedEventSO _onLoadFailedEventSO;

        private IDataService _activeDataService;
        private List<ISaveable> _saveableEntities;

        // Singleton instance
        public static SaveLoadManager Instance { get; private set; }

        private void Awake()
        {
            // Singleton pattern
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
            if (_dataServiceSO != null)
            {
                _activeDataService = _dataServiceSO;
                Debug.Log("Using configured IDataServiceSO.");
            }
            else
            {
                // Fallback: Create a default JsonDataService at runtime
                _activeDataService = new JsonDataService();
                Debug.LogWarning("No IDataServiceSO configured, falling back to default JsonDataService.");
            }
        }

        /// <summary>
        /// Registers a saveable entity with the manager.
        /// </summary>
        public void RegisterSaveable(ISaveable saveable)
        {
            if (_saveableEntities == null) 
                _saveableEntities = new List<ISaveable>();
                
            if (!_saveableEntities.Contains(saveable))
            {
                _saveableEntities.Add(saveable);
            }
        }

        /// <summary>
        /// Unregisters a saveable entity from the manager.
        /// </summary>
        public void UnregisterSaveable(ISaveable saveable)
        {
            _saveableEntities?.Remove(saveable);
        }

        /// <summary>
        /// Finds all active ISaveable components in the current scene.
        /// </summary>
        public void FindAllSaveableEntities()
        {
            _saveableEntities = FindObjectsOfType<MonoBehaviour>(true)
                .OfType<ISaveable>()
                .ToList();
                
            Debug.Log($"Found {_saveableEntities.Count} saveable entities in the scene.");
        }

        /// <summary>
        /// Saves the game state.
        /// </summary>
        /// <returns>True if the save was successful, false otherwise.</returns>
        public bool SaveGame()
        {
            try
            {
                // Notify listeners that we're about to save
                _onBeforeSaveGameEventSO?.Raise();

                // Create a dictionary to hold all save data
                var gameState = new Dictionary<string, object>();

                // Capture state from all saveable entities
                foreach (var saveable in _saveableEntities)
                {
                    if (saveable == null) continue;
                    
                    string key = saveable.UniqueID;
                    if (string.IsNullOrEmpty(key))
                    {
                        Debug.LogWarning("Saveable entity has empty or null UniqueID. Skipping.");
                        continue;
                    }

                    try
                    {
                        object state = saveable.CaptureState();
                        if (state != null)
                        {
                            gameState[key] = state;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Error capturing state for {saveable}: {e.Message}");
                    }
                }

                // Save the game state
                bool success = _activeDataService.Save(_saveFileName, gameState, _encryptData);

                if (success)
                {
                    _onAfterSaveGameEventSO?.Raise();
                    Debug.Log("Game saved successfully.");
                }
                else
                {
                    _onSaveFailedEventSO?.Raise();
                    Debug.LogError("Failed to save game.");
                }

                return success;
            }
            catch (Exception e)
            {
                _onSaveFailedEventSO?.Raise();
                Debug.LogError($"Error during save: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Loads the game state.
        /// </summary>
        /// <returns>True if the load was successful, false otherwise.</returns>
        public bool LoadGame()
        {
            try
            {
                if (!_activeDataService.HasKey(_saveFileName))
                {
                    Debug.Log("No save file found.");
                    return false;
                }

                // Load the game state
                var gameState = _activeDataService.Load<Dictionary<string, object>>(_saveFileName, _encryptData);
                
                if (gameState == null)
                {
                    _onLoadFailedEventSO?.Raise();
                    Debug.LogError("Failed to load game state: Invalid save data.");
                    return false;
                }

                // Restore state for all saveable entities
                foreach (var saveable in _saveableEntities)
                {
                    if (saveable == null) continue;
                    
                    string key = saveable.UniqueID;
                    if (string.IsNullOrEmpty(key)) continue;

                    if (gameState.TryGetValue(key, out object state))
                    {
                        try
                        {
                            saveable.RestoreState(state);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError($"Error restoring state for {saveable}: {e.Message}");
                        }
                    }
                }

                // Notify listeners that loading is complete
                _onAfterLoadGameEventSO?.Raise();
                Debug.Log("Game loaded successfully.");
                return true;
            }
            catch (Exception e)
            {
                _onLoadFailedEventSO?.Raise();
                Debug.LogError($"Error during load: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Deletes the current save file.
        /// </summary>
        public bool DeleteSave()
        {
            bool success = _activeDataService.Delete(_saveFileName);
            if (success)
            {
                Debug.Log("Save file deleted successfully.");
            }
            else
            {
                Debug.LogWarning("Failed to delete save file or file did not exist.");
            }
            return success;
        }

        /// <summary>
        /// Checks if a save file exists.
        /// </summary>
        public bool HasSave()
        {
            return _activeDataService.HasKey(_saveFileName);
        }
    }
}
