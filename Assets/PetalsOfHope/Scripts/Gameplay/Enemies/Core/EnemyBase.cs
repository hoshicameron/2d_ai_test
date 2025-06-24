using UnityEngine;
using PetalsOfHope.Data.Enemies;
using PetalsOfHope.Core.Animation;
using CoreAnimation = PetalsOfHope.Core.Animation.AnimationController;

namespace PetalsOfHope.Gameplay.Enemies.Core
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(EnemyHealth))]
    [RequireComponent(typeof(AnimationController))]
    public abstract class EnemyBase : MonoBehaviour
    {
        [Header("Dependencies")]
        [Tooltip("ScriptableObject containing this enemy's statistics.")]
        [SerializeField] protected EnemyStatsSO _stats;
        public EnemyStatsSO Stats => _stats;

        // Cached Components
        protected Rigidbody2D _rigidbody;
        protected CoreAnimation _animationController;
        protected EnemyHealth _enemyHealth;
        
        // State
        public bool IsDead => _enemyHealth != null && _enemyHealth.IsDead;

        protected virtual void Awake()
        {
            CacheComponents();
            
            if (_stats == null)
            {
                Debug.LogError($"{name}: EnemyStatsSO not assigned to EnemyBase!", this);
                enabled = false;
                return;
            }
            
            InitializeEnemy();
        }
        
        protected virtual void CacheComponents()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animationController = GetComponent<CoreAnimation>();
            _enemyHealth = GetComponent<EnemyHealth>();
            
            if (_enemyHealth == null)
            {
                Debug.LogError($"{name}: EnemyHealth component is missing! Adding it automatically.", this);
                _enemyHealth = gameObject.AddComponent<EnemyHealth>();
            }
        }


        protected virtual void Start()
        {
            // Subscribe to health events
            if (_enemyHealth != null)
            {
                _enemyHealth.OnDeath += HandleDeath;
                _enemyHealth.OnHealthChanged += HandleHealthChanged;
            }
        }

        protected virtual void InitializeEnemy()
        {
            // Initialize any enemy-specific logic here
            // Health is now managed by the EnemyHealth component
        }

        /// <summary>
        /// Handles damage taken by the enemy.
        /// </summary>
        public virtual void TakeDamage(int amount)
        {
            if (_enemyHealth != null)
            {
                _enemyHealth.TakeDamage(amount);
            }
        }


        /// <summary>
        /// Called when the enemy's health changes.
        /// </summary>
        protected virtual void HandleHealthChanged(int currentHealth)
        {
            // Can be overridden by derived classes to react to health changes
        }


        /// <summary>
        /// Handles the enemy's death.
        /// </summary>
        protected virtual void HandleDeath()
        {
            if (_rigidbody != null)
            {
                _rigidbody.simulated = false;
            }

            HandleDeathVisualsAndCleanup();
        }
        
        /// <summary>
        /// Handles death visuals and cleanup.
        /// </summary>
        protected virtual void HandleDeathVisualsAndCleanup()
        {
            // Play death animation
            _animationController?.Play("Death");
            
            // Default: Destroy after a delay
            float destroyDelay = _stats != null && _stats.deathAnimationDuration > 0 
                ? _stats.deathAnimationDuration 
                : 2f;
                
            Destroy(gameObject, destroyDelay);
        }
        
        protected virtual void OnDestroy()
        {
            // Unsubscribe from events
            if (_enemyHealth != null)
            {
                _enemyHealth.OnDeath -= HandleDeath;
                _enemyHealth.OnHealthChanged -= HandleHealthChanged;
            }
        }
    }
}
