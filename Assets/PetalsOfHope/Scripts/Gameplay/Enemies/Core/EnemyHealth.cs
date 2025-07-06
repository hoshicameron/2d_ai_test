using UnityEngine;
using System;
using PetalsOfHope.Data.Player;
using PetalsOfHope.Interfaces;

namespace PetalsOfHope.Gameplay.Enemies.Core
{
    /// <summary>
    /// Handles health management for enemies, implementing IDamageable interface.
    /// </summary>
    public class EnemyHealth : MonoBehaviour, IDamageable
    {
        
        [SerializeField] private CharacterStatsSo stats;
        private int _currentHealth;
        private bool _isDead;

        public event Action<int> OnHealthChanged;
        public event Action OnDeath;

        public int CurrentHealth => _currentHealth;
        public int MaxHealth { get; set; }
        public bool IsDead => _isDead;

        private void Awake()
        {
            MaxHealth = stats.maxHealth;
            ResetHealth();
        }

        /// <summary>
        /// Resets the enemy's health to maximum.
        /// </summary>
        public void ResetHealth()
        {
            _currentHealth = MaxHealth;
            _isDead = false;
            OnHealthChanged?.Invoke(_currentHealth);
        }

        /// <summary>
        /// Applies damage to the enemy.
        /// </summary>
        /// <param name="amount">Amount of damage to apply.</param>
        public void TakeDamage(int amount)
        {
            if (_isDead || amount <= 0) return;

            _currentHealth = Mathf.Max(0, _currentHealth - amount);
            OnHealthChanged?.Invoke(_currentHealth);

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Handles the enemy's death.
        /// </summary>
        private void Die()
        {
            if (_isDead) return;

            _isDead = true;
            OnDeath?.Invoke();
        }
    }
}
