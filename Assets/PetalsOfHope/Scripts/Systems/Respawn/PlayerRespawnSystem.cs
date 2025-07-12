using System;
using PetalsOfHope.Core.Events;
using PetalsOfHope.Core.Persistence.Interfaces;
using UnityEngine;

namespace PetalsOfHope.Gameplay.Respawn
{
    public class PlayerRespawnSystem : MonoBehaviour, ISaveable
    {
        [Header("ID")]
        [SerializeField] private string uniqueID;
        
        [Header("Event Listeners")]
        [SerializeField] private Vector3EventSO playerReachedCheckpointEventSo;
        [SerializeField] private GameEventSO playerDiedEventSo;

        [Header("Event Raisers")]
        [SerializeField] private GameEventSO onPlayerRespawnEventSo;

        private Vector3 _lastCheckpointPosition;
        private bool _hasCheckpoint;
        private GameObject _playerInstance;

        private void Awake()
        {
            _playerInstance = GameObject.FindGameObjectWithTag("Player");
            if (_playerInstance != null)
            {
                _lastCheckpointPosition = _playerInstance.transform.position;
            }
        }

        private void OnEnable()
        {
            playerReachedCheckpointEventSo?.RegisterListener(OnPlayerReachedCheckpoint);
            playerDiedEventSo?.RegisterListener(OnPlayerDied);
        }

        private void OnDisable()
        {
            playerReachedCheckpointEventSo?.UnregisterListener(OnPlayerReachedCheckpoint);
            playerDiedEventSo?.UnregisterListener(OnPlayerDied);
        }

        private void OnPlayerReachedCheckpoint(Vector3 checkpointPosition)
        {
            _lastCheckpointPosition = checkpointPosition;
            _hasCheckpoint = true;
        }

        private void OnPlayerDied()
        {
            RespawnPlayer();
        }

        private void RespawnPlayer()
        {
            if (_playerInstance == null)
            {
                _playerInstance = GameObject.FindGameObjectWithTag("Player");
                if (_playerInstance == null) return;
            }

            _playerInstance.transform.position = _lastCheckpointPosition;
            onPlayerRespawnEventSo?.Raise();
        }

        public string UniqueID => uniqueID;
        
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(uniqueID))
            {
                uniqueID = Guid.NewGuid().ToString();
            }
        }

        public object CaptureState()
        {
            return new SaveData { checkpointPosition = _lastCheckpointPosition, hasCheckpoint = _hasCheckpoint };
        }

        public void RestoreState(object state)
        {
            if (state is not SaveData saveData) return;
            _lastCheckpointPosition = saveData.checkpointPosition;
            _hasCheckpoint = saveData.hasCheckpoint;
            if (_hasCheckpoint && _playerInstance != null)
            {
                _playerInstance.transform.position = _lastCheckpointPosition;
            }
        }

        [System.Serializable]
        private struct SaveData
        {
            public Vector3 checkpointPosition;
            public bool hasCheckpoint;
        }
    }
}
