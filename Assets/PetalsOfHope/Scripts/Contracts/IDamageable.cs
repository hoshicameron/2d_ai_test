namespace PetalsOfHope.Contracts
{
    using UnityEngine;

    /// <summary>
    /// Interface for entities that can take damage.
    /// </summary>
    public interface IDamageable
    {
        /// <summary>
        /// Applies damage to the entity.
        /// </summary>
        /// <param name="amount">The amount of damage to apply.</param>
        void TakeDamage(int amount);

        // Optional: Extend with more context if needed by gameplay systems
        // void TakeDamage(int amount, GameObject instigator);
        // void TakeDamage(int amount, Vector3 hitPoint, Vector3 hitDirection);
        // bool IsDead { get; } // Could be useful
    }
}
