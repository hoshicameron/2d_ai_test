using UnityEngine;
using PetalsOfHope.Interfaces;

namespace PetalsOfHope.Gameplay.LevelElements
{
    [RequireComponent(typeof(Collider2D))]
    public class DeathZone : MonoBehaviour
    {
        private void Awake()
        {
            // Ensure the collider is a trigger
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Check if the object that entered the trigger can take damage
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                // Apply an amount of damage high enough to be lethal
                damageable.TakeDamage(9999);
            }
        }
    }
}
