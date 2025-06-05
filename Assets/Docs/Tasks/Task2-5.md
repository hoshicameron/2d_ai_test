# Task ID: 2.5
# Parent Task ID: 2
# Title: Player Health System Implementation
# Status: completed
# Dependencies: 2.4, 1.3.1, 1.2.3, 1.2.8 # PlayerController, PlayerStatsSO, TypedEventSO, GameEventSO
# Priority: high
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement `PlayerHealth.cs`, a MonoBehaviour responsible for managing the player's current health, handling damage intake, and triggering death events and animations.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/Gameplay/Player/PlayerHealth.cs`
2.  **Namespace:** `PetalsOfHope.Gameplay.Player`
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/Gameplay/Player/PlayerHealth.cs
    namespace PetalsOfHope.Gameplay.Player
    {
        using UnityEngine;
        using PetalsOfHope.Data.Player;
        using PetalsOfHope.Core.Events;
        using CoreAnimation = PetalsOfHope.Core.Animation.AnimationController; // Alias

        // It's good practice for health systems to also implement IDamageable if that interface exists (Task 3.3.1)
        // For now, implementing as described in Phase 2.2.5
        // public class PlayerHealth : MonoBehaviour, PetalsOfHope.Interfaces.IDamageable 
        public class PlayerHealth : MonoBehaviour
        {
            [Header("Data Dependencies")]
            [Tooltip("Player's statistics, including max health.")]
            [SerializeField] private PlayerStatsSO _stats;

            [Header("Health Events")]
            [Tooltip("Event raised when the player dies.")]
            [SerializeField] private GameEventSO _playerDiedEventSO;
            [Tooltip("Event raised when player's health changes. Payload: current health (int).")]
            [SerializeField] private IntEventSO _playerHealthChangedEventSO; // Assuming IntEventSO exists from Task 1.2.3

            private int _currentHealth;
            public int CurrentHealth => _currentHealth;
            public int MaxHealth => _stats != null ? _stats.maxHealth : 100; // Default if stats not loaded

            private CoreAnimation _animationController; // To trigger death/hurt animation
            private bool _isDead = false;

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

                // Trigger death animation
                // _animationController?.Play("Player_Death_Anim"); // Example

                // Disable player controls, physics, etc.
                // This could be handled by PlayerController listening to PlayerDiedEventSO
                // or by direct calls if PlayerHealth has a reference to PlayerController.
                // For now, keeping it simple:
                var playerController = GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.enabled = false; // Disable player controller script
                    if(playerController.InputReader != null) playerController.InputReader.DisableAllInput();
                }
                var playerRigidbody = GetComponent<Rigidbody2D>();
                if (playerRigidbody != null)
                {
                    playerRigidbody.velocity = Vector2.zero;
                    playerRigidbody.isKinematic = true; // Stop responding to physics
                }
                // Potentially disable collider after death animation or delay
            }

            // This method would be part of IDamageable if implemented
            // public void TakeDamage(int amount, GameObject instigator) { TakeDamage(amount); }
        }
    }
    ```

# Acceptance Criteria:
- `PlayerHealth.cs` MonoBehaviour is created in the specified location and namespace.
- It has fields for `PlayerStatsSO`, `_playerDiedEventSO` (GameEventSO), and `_playerHealthChangedEventSO` (TypedEventSO<int>).
- `Start()` initializes `_currentHealth` from `_stats.maxHealth` and raises `_playerHealthChangedEventSO`.
- `TakeDamage(int amount)`:
    - Reduces `_currentHealth`.
    - Clamps health to be non-negative.
    - Raises `_playerHealthChangedEventSO`.
    - Calls `Die()` if health is <= 0.
    - Does nothing if player is already dead or damage is non-positive.
- `Die()`:
    - Sets `_isDead` flag to true.
    - Raises `_playerDiedEventSO`.
    - Triggers a death animation via `_animationController` (placeholder name).
    - Disables player controls/physics (e.g., disables `PlayerController` script, sets Rigidbody to kinematic).
    - Ensures `Die()` logic only executes once.
- `Heal(int amount)` method is implemented to restore health up to max and raise `_playerHealthChangedEventSO`.
- Script compiles without errors.

# Test Strategy:
- Manual/Integration Testing:
    - Add `PlayerHealth.cs` to the Player GameObject.
    - Assign `PlayerStatsSO`, `_playerDiedEventSO`, and `_playerHealthChangedEventSO`.
    - Create simple UI elements (e.g., a TextMeshProUGUI) that listen to `_playerHealthChangedEventSO` to display current health.
    - Create a test script or UI button that calls `PlayerHealth.TakeDamage(10)` on the player.
    - In Play Mode:
        - Verify initial health is displayed correctly.
        - Click damage button: verify health display updates, `_playerHealthChangedEventSO` is raised.
        - Reduce health to 0: verify `_playerDiedEventSO` is raised, death animation plays (if set up), player controls are disabled.
        - Test `Heal()` method.
        - Test taking damage when already dead (should do nothing).
        - Test taking 0 or negative damage (should do nothing).

# Notes/Questions:
- The plan mentions `AnimationController` for death animation. `PlayerHealth` should get a reference to it (e.g., `GetComponent<CoreAnimation>()`).
- The interaction with `IDamageable` (from Phase 3.3.1) will need to be integrated later. For now, `TakeDamage(int amount)` is standalone.
- Disabling player controls and physics in `Die()` is a basic approach. A more robust game might have a "DeadState" for the player or a `GameManager` handling post-death sequences (respawn, game over screen).
- The `IntEventSO` for health change should be created as per Task 1.2.3 / 1.2.8 (e.g., `PlayerHealthChangedEventSO` of type `IntEventSO`).
- Added a `Heal(int amount)` method as it's a common counterpart to `TakeDamage`.