using System;
using UnityEngine;
using PetalsOfHope.Data.Player;
using PetalsOfHope.Core.Events;
using PetalsOfHope.Interfaces;
using CoreAnimation = PetalsOfHope.Gameplay.Animation.AnimationController; // Alias

// In Assets/_Project/Scripts/Gameplay/Player/PlayerHealth.cs
namespace PetalsOfHope.Gameplay.Player
{
    // It's good practice for health systems to also implement IDamageable if that interface exists (Task 3.3.1)
    // For now, implementing as described in Phase 2.2.5
    // public class PlayerHealth : MonoBehaviour, PetalsOfHope.Interfaces.IDamageable 
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        [Header("Data Dependencies")]
        [Tooltip("Player's statistics, including max health.")]
        [SerializeField] private CharacterStatsSo _stats;

        [Header("Health Events")]
        [Tooltip("Event raised when the player dies.")]
        [SerializeField] private GameEventSO _playerDiedEventSO;
        [Tooltip("Event raised when player's health changes. Payload: current health (int).")]
        [SerializeField] private IntEventSO _playerHealthChangedEventSO; // Assuming IntEventSO exists from Task 1.2.3
        [SerializeField] private GameEventSO onPlayerRespawnEventSo;

        private int _currentHealth;
        private CoreAnimation _animationController; // To trigger death/hurt animation
        private bool _isDead = false;

        public int CurrentHealth => _currentHealth;
        public int MaxHealth => _stats != null ? _stats.maxHealth : 100; // Default if stats not loaded
        public bool IsDead => _isDead;

        private void Awake()
        {
            _animationController = GetComponent<CoreAnimation>();
            if (_animationController == null)
            {
                // PlayerController also requires CoreAnimation, so this might be redundant if PlayerHealth is always on Player obj
                // but good for robustness if PlayerHealth could be separated.
                Debug.LogWarning("PlayerHealth: AnimationController not found. Death/hurt animations might not play.", this);
            }
        }

        private void OnEnable()
        {
            onPlayerRespawnEventSo.RegisterListener(HandlePlayerRespawn);
        }

        private void OnDisable()
        {
            onPlayerRespawnEventSo.UnregisterListener(HandlePlayerRespawn);
        }

        private void HandlePlayerRespawn()
        {
            ResetHealth();
        }


        private void Start()
        {
            if (_stats == null)
            {
                Debug.LogError("PlayerStatsSO not assigned to PlayerHealth component!", this);
                _currentHealth = 100; // Fallback
            }
            else
            {
                _currentHealth = _stats.maxHealth;
            }
            _playerHealthChangedEventSO?.Raise(_currentHealth);
            _isDead = false;
        }

        /// <summary>
        /// Reduces player's health by the given amount.
        /// Triggers health changed event and death logic if health drops to zero or below.
        /// </summary>
        /// <param name="amount">The amount of damage to take.</param>
        public void TakeDamage(int amount)
        {
            if (_isDead || amount <= 0) return; // Cannot take damage if already dead or damage is non-positive

            _currentHealth -= amount;
            _currentHealth = Mathf.Max(_currentHealth, 0); // Health cannot go below zero

            _playerHealthChangedEventSO?.Raise(_currentHealth);
            // Debug.Log($"Player took {amount} damage. Current health: {_currentHealth}/{MaxHealth}");

            if (_currentHealth <= 0)
            {
                Die();
            }
            else
            {
                // Optionally, play a "hurt" animation or sound
                // _animationController?.Play("Player_Hurt_Anim"); // Example
            }
        }

        /// <summary>
        /// Heals the player by the given amount, up to max health.
        /// </summary>
        /// <param name="amount">The amount of health to restore.</param>
        public void Heal(int amount)
        {
            if (_isDead || amount <= 0) return;

            _currentHealth += amount;
            _currentHealth = Mathf.Min(_currentHealth, MaxHealth);

            _playerHealthChangedEventSO?.Raise(_currentHealth);
            // Debug.Log($"Player healed {amount}. Current health: {_currentHealth}/{MaxHealth}");
        }

        private void Die()
        {
            if (_isDead) return; // Ensure Die logic only runs once

            _isDead = true;
            Debug.Log("Player has died.");
            _playerDiedEventSO?.Raise();
            
            // The actual death handling (animation, disabling controls, etc.) 
            // is now handled by the DeathState in the PlayerController
        }

        /// <summary>
        /// Resets the player's health to maximum and revives them.
        /// Called by the PlayerRespawnSystem.
        /// </summary>
        public void ResetHealth()
        {
            _currentHealth = MaxHealth;
            _isDead = false;
            _playerHealthChangedEventSO?.Raise(_currentHealth);
        }

        // This method would be part of IDamageable if implemented
        // public void TakeDamage(int amount, GameObject instigator) { TakeDamage(amount); }
    }
}
