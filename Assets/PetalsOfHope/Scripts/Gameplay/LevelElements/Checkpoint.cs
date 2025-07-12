using UnityEngine;
using PetalsOfHope.Core.Events;

namespace PetalsOfHope.Gameplay.LevelElements
{
    [RequireComponent(typeof(Collider2D))]
    public class Checkpoint : MonoBehaviour
    {
        [Tooltip("Event raised when player activates this checkpoint. Payload is checkpoint's position.")]
        [SerializeField] private Vector3EventSO onPlayerReachedCheckpointEventSo;

        [Tooltip("Visual to indicate checkpoint is active (optional).")]
        [SerializeField] private GameObject activeVisual;
        
        private bool _isActivated;
        private static Checkpoint _currentActiveCheckpoint;

        private void Awake()
        {
            GetComponent<Collider2D>().isTrigger = true;
            SetVisuals(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_isActivated && collision.CompareTag("Player"))
            {
                ActivateCheckpoint();
            }
        }

        private void ActivateCheckpoint()
        {
            if (_currentActiveCheckpoint != null && _currentActiveCheckpoint != this)
            {
                _currentActiveCheckpoint.DeactivateVisuals();
            }

            _isActivated = true;
            _currentActiveCheckpoint = this;
            SetVisuals(true);
            
            if (onPlayerReachedCheckpointEventSo != null)
            {
                onPlayerReachedCheckpointEventSo.Raise(transform.position);
            }
        }

        public void DeactivateVisuals()
        {
            _isActivated = false;
            SetVisuals(false);
        }

        private void SetVisuals(bool isActive)
        {
            if (activeVisual != null) activeVisual.SetActive(isActive);
        }
        
        public static void ResetAllCheckpoints()
        {
            _currentActiveCheckpoint = null;
        }
    }
}
