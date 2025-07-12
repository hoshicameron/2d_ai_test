using UnityEngine;
using PetalsOfHope.Interfaces;
using PetalsOfHope.Core.Events;

namespace PetalsOfHope.Gameplay.LevelElements
{
    public class BreakableBlock : MonoBehaviour, IDamageable
    {
        [SerializeField]
        [Tooltip("The amount of health this block has.")]
        private int health = 1;

        [SerializeField]
        [Tooltip("Optional: Event to raise when the block is destroyed.")]
        private GameEventSO onBlockDestroyed;

        public void TakeDamage(int amount)
        {
            health -= amount;
            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (onBlockDestroyed != null)
            {
                onBlockDestroyed.Raise();
            }
            // Here you could also trigger particle effects or sounds
            Destroy(gameObject);
        }
    }
}
