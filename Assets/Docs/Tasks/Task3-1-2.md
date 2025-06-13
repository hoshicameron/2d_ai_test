# Task ID: 3.1.2
# Parent Task ID: 3.1
# Title: Implement EnemyBase Abstract Class
# Status: completed
# Dependencies: 3.1.1, 2.2, 1.3.1, 1.2.8 # IDamageable, AnimationController, EnemyStatsSO, EventSO assets
# Priority: critical
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement `EnemyBase.cs`, an abstract MonoBehaviour class that serves as the foundation for all enemy types. It will manage health, implement `IDamageable`, handle taking damage, and process death.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/Enemies/Core/EnemyBase.cs`
2.  **Namespace:** `PetalsOfHope.Enemies.Core`
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/Enemies/Core/EnemyBase.cs
    namespace PetalsOfHope.Enemies.Core
    {
        using UnityEngine;
        using PetalsOfHope.Interfaces; // For IDamageable
        using PetalsOfHope.Data.Enemies; // For EnemyStatsSO
        using PetalsOfHope.Core.Events; // For GameEventSO
        using CoreAnimation = PetalsOfHope.Core.Animation.AnimationController; // Alias

        [RequireComponent(typeof(Rigidbody2D))]
        [RequireComponent(typeof(Collider2D))] // Could be BoxCollider2D, CapsuleCollider2D etc.
        [RequireComponent(typeof(CoreAnimation))]
        // Optionally require AI StateMachine if all enemies will use it:
        // [RequireComponent(typeof(PetalsOfHope.AI.StateMachine))] 
        public abstract class EnemyBase : MonoBehaviour, IDamageable
        {
            [Header("Stats & Data")]
            [Tooltip("ScriptableObject containing this enemy's statistics.")]
            [SerializeField] protected EnemyStatsSO _stats;
            public EnemyStatsSO Stats => _stats;

            [Header("Events")]
            [Tooltip("Event raised when this enemy dies.")]
            [SerializeField] protected GameEventSO _enemyDiedEventSO; // Specific to this enemy instance or a general one?
                                                                    // For now, assume a general event. Can be TypedEventSO<EnemyBase> too.

            [Header("State")]
            [SerializeField] // For debugging in inspector
            protected int _currentHealth;
            public int CurrentHealth => _currentHealth;
            public bool IsDead { get; protected set; } = false;


            // Cached Components
            protected Rigidbody2D _rigidbody;
            protected Collider2D _collider; // Use Collider2D to be generic for Box, Capsule, etc.
            protected CoreAnimation _animationController;
            // protected PetalsOfHope.AI.StateMachine _aiStateMachine; // If AI state machine is common


            protected virtual void Awake()
            {
                _rigidbody = GetComponent<Rigidbody2D>();
                _collider = GetComponent<Collider2D>();
                _animationController = GetComponent<CoreAnimation>();
                // _aiStateMachine = GetComponent<PetalsOfHope.AI.StateMachine>();

                if (_stats == null)
                {
                    Debug.LogError("EnemyStatsSO not assigned to EnemyBase on " + gameObject.name, this);
                    // Potentially disable enemy or use default stats
                    enabled = false; 
                    return;
                }
                InitializeHealth();
            }

            protected virtual void Start()
            {
                // Initialization logic that might depend on other components being ready
            }

            protected virtual void InitializeHealth()
            {
                _currentHealth = _stats.maxHealth;
                IsDead = false;
            }

            public virtual void TakeDamage(int amount)
            {
                if (IsDead || amount <= 0) return;

                _currentHealth -= amount;
                _currentHealth = Mathf.Max(_currentHealth, 0);

                // Debug.Log($"{gameObject.name} took {amount} damage. Health: {_currentHealth}/{_stats.maxHealth}");

                if (_currentHealth <= 0)
                {
                    Die();
                }
                else
                {
                    // Play hurt animation or other feedback
                    // _animationController?.Play("Hurt"); // Example hash/name
                    OnDamaged();
                }
            }

            protected virtual void OnDamaged()
            {
                // Placeholder for damage feedback logic (e.g. flashing sprite, sound)
                // Could trigger a specific hurt state in AI
            }

            protected virtual void Die()
            {
                if (IsDead) return; // Ensure Die logic only runs once

                IsDead = true;
                // Debug.Log($"{gameObject.name} has died.");

                _enemyDiedEventSO?.Raise(); // Raise a general enemy died event.
                // If a more specific event is needed:
                // EnemyDiedData data = new EnemyDiedData { EnemyType = _stats.enemyName, Position = transform.position };
                // _specificEnemyDiedEventSO?.Raise(data);

                // Trigger death animation via AnimationController
                // _animationController?.Play("Death"); // Example name/hash

                // Disable components
                _collider.enabled = false;
                _rigidbody.simulated = false; // Stops physics interaction
                // if (_aiStateMachine != null) _aiStateMachine.enabled = false; // Disable AI

                // Handle object destruction or pooling
                // For now, just disable. Actual destruction might be delayed for animation.
                // Destroy(gameObject, 2f); // Example: destroy after 2 seconds
                HandleDeathVisualsAndCleanup();
            }
            
            protected virtual void HandleDeathVisualsAndCleanup()
            {
                // Default: Destroy after a delay. Override for pooling or specific death effects.
                // For example, if death animation is played via Animator, it can have an event at the end to call a cleanup method.
                // This method itself can be overridden by derived classes.
                 _animationController?.Play("Death"); // Assuming "Death" is a generic animation name.
                Destroy(gameObject, _stats.deathAnimationDuration > 0 ? _stats.deathAnimationDuration : 2f); // Add deathAnimationDuration to EnemyStatsSO
            }

            // Abstract methods for derived classes to implement specific behaviors
            // protected abstract void Patrol();
            // protected abstract void Chase();
            // protected abstract void Attack();
        }
    }
    ```

# Acceptance Criteria:
- `EnemyBase.cs` abstract class is created, inherits `MonoBehaviour`, implements `IDamageable`.
- Requires `Rigidbody2D`, `Collider2D`, `CoreAnimation` components.
- Has fields for `EnemyStatsSO` and an `EnemyDiedEventSO`.
- `Awake()` caches components and initializes health from `_stats.maxHealth`.
- `TakeDamage(int amount)` correctly reduces health, handles non-positive health, calls `Die()` if health <= 0, and optionally triggers `OnDamaged()` for feedback.
- `Die()` sets `IsDead`, raises `_enemyDiedEventSO`, triggers a death animation (placeholder), disables relevant components (collider, rigidbody, AI), and handles object cleanup (e.g., `Destroy` after a delay). Ensures it only runs once.
- `OnDamaged()` and `HandleDeathVisualsAndCleanup()` are virtual methods for derived classes to override.
- Script compiles without errors.

# Test Strategy:
- Create a simple derived class `TestEnemy.cs` that inherits from `EnemyBase`.
- Create a prefab for `TestEnemy` with necessary components and assign an `EnemyStatsSO` and `EnemyDiedEventSO`.
- In a test scene, instantiate `TestEnemy`.
- Create a test script to call `testEnemy.TakeDamage(damageAmount)`.
- Verify:
    - Health reduction.
    - `OnDamaged()` behavior (e.g., log message).
    - `Die()` behavior: `IsDead` flag, event raised, components disabled, object destroyed after delay.
    - Death animation is triggered (if a placeholder "Death" animation is set up in Animator).

# Notes/Questions:
- The `_enemyDiedEventSO` could be a generic `GameEventSO` or a `TypedEventSO<EnemyBase>` (or `TypedEventSO<EnemyData>`) to pass information about the specific enemy that died. The plan implies `GameEventSO`.
- Cleanup in `Die()` (e.g., `Destroy(gameObject, 2f)`) is a simple approach. More advanced games might use object pooling. `HandleDeathVisualsAndCleanup` provides a hook for this.
- Added `deathAnimationDuration` to `EnemyStatsSO` (in thought) to control destroy delay.
- The requirement for `PetalsOfHope.AI.StateMachine` on `EnemyBase` can be added if all enemies are guaranteed to use it. Otherwise, derived classes or specific AI handler components would manage it.