using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using PetalsOfHope.Systems.Persistence.Services;
using PetalsOfHope.Core.Events;
using System;
using PetalsOfHope.Interfaces;
using UnityEngine.Serialization;

namespace PetalsOfHope.Systems.Persistence
{
    /// <summary>
    /// Manages saving and loading game state using the active IDataService.
    /// </summary>
    public class SaveLoadSystem : MonoBehaviour
    {
        [Header("Data Service Configuration")]
        [Tooltip("The data service to use. If null, will default to creating a JsonDataServiceSO at runtime.")]
        [SerializeField] private IDataServiceSO dataServiceSo;
        
        [Tooltip("Filename for the main save game data.")]
        [SerializeField] private string saveFileName = "game_save_data";
        
        [Tooltip("Whether to encrypt the save data.")]
        [SerializeField] private bool encryptData = false;

        [Header("Event Channels for Requesting Actions")]
        [Tooltip("Event to request saving the game.")]
        [SerializeField] private GameEventSO saveGameRequestEvent;
        [Tooltip("Event to request loading the game.")]
        [SerializeField] private GameEventSO loadGameRequestEvent;
        [Tooltip("Event to request deleting the save data.")]
        [SerializeField] private GameEventSO deleteSaveRequestEvent;

        [Header("Manual Registration Events")]
        [Tooltip("Event to request registering a new ISaveable entity.")]
        [SerializeField] private SaveableEventSO registerSaveableEvent;
        [Tooltip("Event to request unregistering an ISaveable entity.")]
        [SerializeField] private SaveableEventSO unregisterSaveableEvent;

        [Header("Save/Load Events")]
        [SerializeField] private OnBeforeSaveGameEventSO onBeforeSaveGameEventSo;
        [SerializeField] private OnAfterSaveGameEventSO onAfterSaveGameEventSo;
        [SerializeField] private OnAfterLoadGameEventSO onAfterLoadGameEventSo;
        [SerializeField] private OnSaveFailedEventSO onSaveFailedEventSo;
        [SerializeField] private OnLoadFailedEventSO onLoadFailedEventSo;

        private IDataService _activeDataService;
        private List<ISaveable> _saveableEntities = new();

        private void Awake()
        {
            // This component should exist on a persistent object that is not destroyed on scene loads.
            // The responsibility for this is moved outside the manager itself.
            InitializeDataService();
            FindAllSaveableEntities();
        }

        private void OnEnable()
        {
            // Subscribe to request events
            saveGameRequestEvent?.RegisterListener(SaveGame);
            loadGameRequestEvent?.RegisterListener(LoadGame);
            deleteSaveRequestEvent?.RegisterListener(DeleteSave);

            // Subscribe to manual registration events
            registerSaveableEvent?.RegisterListener(RegisterSaveable);
            unregisterSaveableEvent?.RegisterListener(UnregisterSaveable);
        }

        private void OnDisable()
        {
            // Unsubscribe from request events
            saveGameRequestEvent?.UnregisterListener(SaveGame);
            loadGameRequestEvent?.UnregisterListener(LoadGame);
            deleteSaveRequestEvent?.UnregisterListener(DeleteSave);

            // Unsubscribe from manual registration events
            registerSaveableEvent?.UnregisterListener(UnregisterSaveable);
            unregisterSaveableEvent?.UnregisterListener(UnregisterSaveable);
        }

        private void InitializeDataService()
        {
            if (dataServiceSo != null)
            {
                _activeDataService = dataServiceSo;
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
            _saveableEntities = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .OfType<ISaveable>()
                .ToList();
                
            Debug.Log($"Found {_saveableEntities.Count} saveable entities in the scene.");
        }

        /// <summary>
        /// Saves the game state. Triggered by an event.
        /// </summary>
        public void SaveGame()
        {
            bool success = PerformSave();
            if (success)
            {
                onAfterSaveGameEventSo?.Raise();
                Debug.Log("Game saved successfully.");
            }
            else
            {
                onSaveFailedEventSo?.Raise();
                Debug.LogError("Failed to save game.");
            }
        }

        private bool PerformSave()
        {
            try
            {
                // Notify listeners that we're about to save
                onBeforeSaveGameEventSo?.Raise();

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
                return _activeDataService.Save(saveFileName, gameState, encryptData);
            }
            catch (Exception e)
            {
                onSaveFailedEventSo?.Raise();
                Debug.LogError($"Error during save: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Loads the game state. Triggered by an event.
        /// </summary>
        public void LoadGame()
        {
            bool success = PerformLoad();
            if (success)
            {
                // Notify listeners that loading is complete
                onAfterLoadGameEventSo?.Raise();
                Debug.Log("Game loaded successfully.");
            }
            else
            {
                onLoadFailedEventSo?.Raise();
                Debug.LogError("Failed to load game state.");
            }
        }

        private bool PerformLoad()
        {
            try
            {
                if (!_activeDataService.HasKey(saveFileName))
                {
                    Debug.Log("No save file found.");
                    return false;
                }

                // Load the game state
                var gameState = _activeDataService.Load<Dictionary<string, object>>(saveFileName, encryptData);
                
                if (gameState == null)
                {
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
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error during load: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Deletes the current save file. Triggered by an event.
        /// </summary>
        public void DeleteSave()
        {
            bool success = _activeDataService.Delete(saveFileName);
            if (success)
            {
                Debug.Log("Save file deleted successfully.");
            }
            else
            {
                Debug.LogWarning("Failed to delete save file or file did not exist.");
            }
        }

        /// <summary>
        /// Checks if a save file exists.
        /// </summary>
        public bool HasSave()
        {
            return _activeDataService.HasKey(saveFileName);
        }
    }
}
