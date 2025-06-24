using UnityEngine;
using System;
using PetalsOfHope.Interfaces;
using PetalsOfHope.Data.Enemies;
using PetalsOfHope.Core.Events;

namespace PetalsOfHope.Gameplay.Enemies.Core
{
    /// <summary>
    /// Handles health management for enemies, implementing IDamageable interface.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class EnemyHealth : MonoBehaviour, IDamageable
    {
        [Header("Dependencies")]
        [Tooltip("Enemy stats containing max health and other attributes.")]
        [SerializeField] private EnemyStatsSO _stats;

        [Header("Events")]
        [Tooltip("Event raised when this enemy dies.")]
        [SerializeField] private GameEventSO _onDeathEvent;

        [Header("State")]
        [SerializeField] private int _currentHealth;
        private bool _isDead;

        public event Action<int> OnHealthChanged;
        public event Action OnDeath;

        public int CurrentHealth => _currentHealth;
        public int MaxHealth => _stats != null ? _stats.maxHealth : 100;
        public bool IsDead => _isDead;

        private void Awake()
        {
            if (_stats == null)
            {
                Debug.LogError($"{name}: EnemyStatsSO not assigned to EnemyHealth!", this);
                enabled = false;
                return;
            }
            
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
            _onDeathEvent?.Raise();
            
            // Disable collider to prevent further interactions
            var collider = GetComponent<Collider2D>();
            if (collider != null) collider.enabled = false;
        }
    }
}
