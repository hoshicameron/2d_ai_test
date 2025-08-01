using UnityEngine;
using System.Collections.Generic;
using PetalsOfHope.Data.Collectibles;
using PetalsOfHope.Core.Events;
using PetalsOfHope.Core.Events.Channels;
using System;
using PetalsOfHope.Data.Levels;
using PetalsOfHope.Interfaces;
using UnityEngine.Serialization;

namespace PetalsOfHope.Systems.Progression
{
    public class GameProgressionSystem : MonoBehaviour, ISaveable
    {
        [Header("ID")]
        [SerializeField] private string uniqueID;

        [Header("Dependencies")]
        [Tooltip("Reference to the database containing all talismans.")]
        [SerializeField] private TalismanDatabaseSO talismanDatabase;

        [Header("Event Listeners")]
        [Tooltip("Listen for when any collectible is acquired.")]
        [SerializeField] private CollectibleEventSO onCollectibleCollected;

        [Header("Function Providers")]
        [Tooltip("The channel that provides the ability check function.")]
        [SerializeField] private AbilityCheckChannelSO abilityCheckChannel;
        [Tooltip("The channel that provides the level check function.")]
        [SerializeField] private LevelCheckChannelSO levelCheckChannel;

        [Header("Event Raisers")]
        [Tooltip("Raised when progression data changes (e.g., a talisman is collected).")]
        [SerializeField] private GameEventSO progressionChangedEvent;
        [Tooltip("Raised when a new level is unlocked.")]
        [SerializeField] private LevelUnlockedEventSO levelUnlockedEvent;
        
        [Header("Manual Registration Events")]
        [Tooltip("Event to request registering a new ISaveable entity.")]
        [SerializeField] private SaveableEventSO registerSaveableEvent;
        [Tooltip("Event to request unregistering an ISaveable entity.")]
        [SerializeField] private SaveableEventSO unregisterSaveableEvent;

        
        [Header("Progression Data (Initial/Default State)")]
        [Tooltip("List of SceneDataSO assets that are unlocked by default at the start of a new game.")]
        [SerializeField] private List<SceneDataSO> defaultUnlockedLevels;

        // Runtime data
        private HashSet<string> _unlockedLevelSceneNames = new();
        private HashSet<string> _collectedTalismanIDs = new();

        public string UniqueID => uniqueID;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            InitializeDefaultProgression();
        }

        private void OnEnable()
        {
            if (onCollectibleCollected != null)
            {
                onCollectibleCollected.RegisterListener(HandleCollectibleCollected);
            }
            if (abilityCheckChannel != null)
            {
                abilityCheckChannel.Function = HasTalisman;
            }
            if (levelCheckChannel != null)
            {
                levelCheckChannel.Function = IsLevelUnlocked;
            }
            
            registerSaveableEvent?.Raise(this);
        }

        private void OnDisable()
        {
            if (onCollectibleCollected != null)
            {
                onCollectibleCollected.UnregisterListener(HandleCollectibleCollected);
            }
            if (abilityCheckChannel != null)
            {
                abilityCheckChannel.Function = null;
            }
            if (levelCheckChannel != null)
            {
                levelCheckChannel.Function = null;
            }
            
            unregisterSaveableEvent?.Raise(this);
        }

        private void HandleCollectibleCollected(ICollectible collectible)
        {
            if (collectible != null && !string.IsNullOrEmpty(collectible.ID))
            {
                if (_collectedTalismanIDs.Add(collectible.ID))
                {
                    progressionChangedEvent?.Raise();
                    // Here you would typically raise a "RequestSaveGame" event
                }
            }
        }

        public bool HasTalisman(string talismanID)
        {
            return !string.IsNullOrEmpty(talismanID) && _collectedTalismanIDs.Contains(talismanID);
        }

        public object CaptureState()
        {
            return new ProgressionSaveData
            {
                UnlockedLevelNames = new List<string>(_unlockedLevelSceneNames),
                CollectedTalismanIDs = new List<string>(_collectedTalismanIDs)
            };
        }

        public void RestoreState(object state)
        {
            if (state is ProgressionSaveData saveData)
            {
                _unlockedLevelSceneNames = new HashSet<string>(saveData.UnlockedLevelNames);
                _collectedTalismanIDs = new HashSet<string>(saveData.CollectedTalismanIDs);
                Debug.Log("Game progression restored.");
            }
            else
            {
                InitializeDefaultProgression();
            }
        }

        private void InitializeDefaultProgression()
        {
            _unlockedLevelSceneNames.Clear();
            foreach(var levelData in defaultUnlockedLevels)
            {
                if (levelData != null) UnlockLevel(levelData, false);
            }
            Debug.Log("Default level progression initialized.");
        }

        public bool IsLevelUnlocked(string sceneName)
        {
             if (string.IsNullOrEmpty(sceneName)) return false;
             return _unlockedLevelSceneNames.Contains(sceneName);
        }

        public void UnlockLevel(ISceneData sceneData, bool raiseEvent = true)
        {
            if (sceneData == null) return;
            string sceneName = sceneData.GetSceneName();
            if (string.IsNullOrEmpty(sceneName)) return;

            if (_unlockedLevelSceneNames.Add(sceneName))
            {
                Debug.Log($"Level Unlocked: {sceneName}");
                if(raiseEvent) levelUnlockedEvent?.Raise(sceneData);
            }
        }

        [Serializable]
        private struct ProgressionSaveData
        {
            public List<string> UnlockedLevelNames;
            public List<string> CollectedTalismanIDs;
        }
        
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(uniqueID))
            {
                uniqueID = Guid.NewGuid().ToString();
            }
        }
        
        /*
        HOW TO USE:

        **Setup:**
        1. Place this component on a persistent GameObject in your initial scene (e.g., a "GameManager").
        2. Assign all required ScriptableObject assets in the Inspector (Event Channels, Databases, etc.).
        3. To set the starting levels, add `SceneDataSO` assets to the `Default Unlocked Levels` list.

        **Unlocking a Level:**
        - To unlock a new level from another script (e.g., after a boss fight), get a reference to the `GameProgressionSystem` instance and call the `UnlockLevel(ISceneData sceneData)` method.
          Example: `gameProgressionSystem.UnlockLevel(level2SceneData);`
        - This will automatically save the progression and raise the `_levelUnlockedEvent` for other systems to listen to.

        **Checking if a Level is Unlocked:**
        - Other systems (like a Level Select UI) can check if a level is available by calling `IsLevelUnlocked(string sceneName)`.
          Example: `bool isLevel2Unlocked = gameProgressionSystem.IsLevelUnlocked("Level_02_Desert");`

        **Listening for Level Unlocks:**
        - Any system that needs to react immediately when a level is unlocked (e.g., a UI manager that updates the level select screen) should listen to the `_levelUnlockedEvent`.
        - Create a listener method: `public void OnLevelUnlocked(ISceneData unlockedLevel) { ... }`
        - Register it in `OnEnable`: `_levelUnlockedEvent.RegisterListener(OnLevelUnlocked);`
        - Unregister it in `OnDisable`: `_levelUnlockedEvent.UnregisterListener(OnLevelUnlocked);`
        */
    }
}
