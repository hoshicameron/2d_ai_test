using UnityEngine;
using System.Collections.Generic;
using PetalsOfHope.Core.Persistence.Interfaces;
using PetalsOfHope.Data.Collectibles;
using PetalsOfHope.Core.Events;
using PetalsOfHope.Core.Events.Channels;
using System;
using PetalsOfHope.Interfaces;

namespace PetalsOfHope.Systems
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

        [Header("Event Raisers")]
        [Tooltip("Raised when progression data changes (e.g., a talisman is collected).")]
        [SerializeField] private GameEventSO progressionChangedEvent;

        private HashSet<string> _collectedTalismanIDs = new();

        public string UniqueID => uniqueID;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            if (onCollectibleCollected != null)
            {
                onCollectibleCollected.RegisterListener(HandleCollectibleCollected);
            }
            if (abilityCheckChannel != null)
            {
                abilityCheckChannel.IsUnlocked = HasTalisman;
            }
        }

        private void OnDisable()
        {
            if (onCollectibleCollected != null)
            {
                onCollectibleCollected.UnregisterListener(HandleCollectibleCollected);
            }
            if (abilityCheckChannel != null)
            {
                abilityCheckChannel.IsUnlocked = null;
            }
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
            return new List<string>(_collectedTalismanIDs);
        }

        public void RestoreState(object state)
        {
            if (state is List<string> savedIDs)
            {
                _collectedTalismanIDs = new HashSet<string>(savedIDs);
            }
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
        1. Place this component on a persistent GameObject in your initial scene (e.g., a "GameManager").
        2. Assign the "TalismanDatabase" asset to its corresponding field.
        3. Assign the "OnTalismanCollected" event channel asset.
        4. Assign the "AbilityCheckChannel" asset.
        5. This system will now automatically manage talisman progression and provide the ability check function to other systems.
        6. Ensure this object is found by the SaveLoadManager to save and load its data.
        */
    }
}
