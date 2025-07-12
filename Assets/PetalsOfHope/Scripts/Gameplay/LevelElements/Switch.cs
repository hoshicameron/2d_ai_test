using UnityEngine;
using PetalsOfHope.Core.Events;

namespace PetalsOfHope.Gameplay.LevelElements
{
    [RequireComponent(typeof(Collider2D))]
    public class Switch : MonoBehaviour
    {
        [Tooltip("The event to raise when this switch is activated.")]
        [SerializeField] private GameEventSO onSwitchActivated;
        [Tooltip("The event to raise when this switch is deactivated.")]
        [SerializeField] private GameEventSO onSwitchDeactivated;

        [SerializeField] private Sprite activeSprite;
        [SerializeField] private Sprite deactiveSprite;

        [SerializeField]
        [Tooltip("Should the switch only activate once?")]
        private bool activateOnce = true;

        private bool _hasBeenActivated;
        private SpriteRenderer _spriteRenderer;
        

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                ActivateSwitch();
            }
        }

        private void ActivateSwitch()
        {
            if (activateOnce && _hasBeenActivated)
            {
                return;
            }

            if (_hasBeenActivated)
            {
                if (onSwitchDeactivated != null) onSwitchDeactivated.Raise();
                _spriteRenderer.sprite = deactiveSprite;
                _hasBeenActivated = false;
                return;
            }

            if (onSwitchActivated != null)
            {
                if (onSwitchActivated != null) onSwitchActivated.Raise();
                _hasBeenActivated = true;
                _spriteRenderer.sprite = activeSprite;
            }
        }
    }
}
