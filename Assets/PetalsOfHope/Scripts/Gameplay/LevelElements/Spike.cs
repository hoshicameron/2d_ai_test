using UnityEngine;
using PetalsOfHope.Contracts;

namespace PetalsOfHope.Gameplay.LevelElements
{
    [RequireComponent(typeof(Collider2D))]
    public class Spike : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The amount of damage this spike deals on contact.")]
        private int damage = 1;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Check if the colliding object can take damage
            IDamageable damageable = collision.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }
    }
}
