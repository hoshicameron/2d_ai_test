using UnityEngine;

namespace PetalsOfHope.Gameplay.LevelElements
{
    [RequireComponent(typeof(Collider2D))]
    public class Door : MonoBehaviour
    {
        [SerializeField] private Sprite openSprite;
        [SerializeField] private Sprite closeSprite;
        
        private Collider2D _doorCollider;
        private SpriteRenderer _spriteRenderer; // Optional for visual feedback

        private void Awake()
        {
            _doorCollider = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Open()
        {
            // Disable the collider to allow passage
            _doorCollider.enabled = false;

            // Optional: Provide visual feedback that the door is open
            if (_spriteRenderer != null)
            {
                // For example, change color to indicate it's open
                _spriteRenderer.sprite = openSprite;
            }
        }

        public void Close()
        {
            // Disable the collider to allow passage
            _doorCollider.enabled = true;

            // Optional: Provide visual feedback that the door is open
            if (_spriteRenderer != null)
            {
                // For example, change color to indicate it's open
                _spriteRenderer.sprite = closeSprite;
            }
        }
    }
}
