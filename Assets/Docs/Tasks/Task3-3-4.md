# Task ID: 3.3.4
# Parent Task ID: 3.3.3 # Directly supports ArcherElfEnemy
# Title: Implement Basic Projectile System
# Status: pending
# Dependencies: 3.1.1 # IDamageable interface (for projectile to damage player)
# Priority: high (for ArcherElfEnemy)
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement a basic `Projectile.cs` script and a projectile prefab (e.g., an arrow) that can be launched, travel in a direction, and deal damage on impact with an `IDamageable` entity or be destroyed on collision with the environment.

# Details:
1.  **Create `Projectile.cs` Script:**
    *   File Location: `Assets/_Project/Scripts/Gameplay/Projectiles/Projectile.cs` (Create `Projectiles` folder)
    *   Namespace: `PetalsOfHope.Gameplay.Projectiles`
    *   Implementation:
        ```csharp
        // In Assets/_Project/Scripts/Gameplay/Projectiles/Projectile.cs
        namespace PetalsOfHope.Gameplay.Projectiles
        {
            using UnityEngine;
            using PetalsOfHope.Interfaces; // For IDamageable

            [RequireComponent(typeof(Rigidbody2D))]
            [RequireComponent(typeof(Collider2D))] // For collision detection
            public class Projectile : MonoBehaviour
            {
                private Rigidbody2D _rb;
                private Collider2D _collider;
                private float _speed;
                private int _damage;
                private Vector2 _direction;
                private bool _isLaunched = false;

                // Optional: lifespan to self-destruct if it travels too far/long
                public float maxLifespan = 5f; 
                private float _lifespanTimer;

                // Optional: layers it collides with vs. passes through
                [Tooltip("Layers this projectile will be destroyed by upon collision.")]
                public LayerMask collisionLayers; // e.g., Ground, Walls
                [Tooltip("Layers this projectile will attempt to damage upon collision.")]
                public LayerMask damageableLayers; // e.g., Player, other Enemies (if friendly fire is off)
                
                // Optional: Particle effect on impact
                public GameObject impactEffectPrefab;

                void Awake()
                {
                    _rb = GetComponent<Rigidbody2D>();
                    _collider = GetComponent<Collider2D>();
                    _collider.isTrigger = true; // Use Trigger for non-physical collision response
                    _rb.gravityScale = 0; // Most projectiles don't use gravity unless it's e.g. a grenade
                }

                public void Launch(Vector2 direction, float speed, int damage)
                {
                    _direction = direction.normalized;
                    _speed = speed;
                    _damage = damage;
                    _isLaunched = true;
                    _lifespanTimer = maxLifespan;

                    // Rotate projectile to face direction of travel (if sprite is oriented along X axis)
                    float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    
                    _rb.velocity = _direction * _speed;
                }

                void Update()
                {
                    if (!_isLaunched) return;

                    _lifespanTimer -= Time.deltaTime;
                    if (_lifespanTimer <= 0f)
                    {
                        DestroyProjectile();
                    }
                }

                private void OnTriggerEnter2D(Collider2D other)
                {
                    if (!_isLaunched) return;

                    // Check if the layer of the collided object is in our damageableLayers
                    if (((1 << other.gameObject.layer) & damageableLayers) != 0)
                    {
                        IDamageable damageable = other.GetComponent<IDamageable>();
                        if (damageable != null)
                        {
                            damageable.TakeDamage(_damage);
                            // Debug.Log($"Projectile hit {other.name} for {_damage} damage.");
                        }
                    }
                    
                    // Check if the layer of the collided object is in our collisionLayers (for destruction)
                    // Or, simply destroy on any hit that isn't explicitly ignored.
                    // If it's a damageable layer, we might still want to destroy the projectile.
                    if (((1 << other.gameObject.layer) & collisionLayers) != 0 || 
                        ((1 << other.gameObject.layer) & damageableLayers) != 0) // Destroy if it hits a damageable or a collision layer
                    {
                        DestroyProjectile();
                    }
                }
                
                private void DestroyProjectile()
                {
                    if (impactEffectPrefab != null)
                    {
                        Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
                    }
                    Destroy(gameObject);
                }
            }
        }
        ```
2.  **Create Projectile Prefab (e.g., Arrow):**
    *   Location: `Assets/_Project/Prefabs/Gameplay/Projectiles/ArrowPrefab.prefab`
    *   Create a new 2D Sprite GameObject. Assign an arrow sprite. Orient it to point right (along positive X-axis).
    *   Add `Rigidbody2D`: `Body Type` = `Kinematic` (if moved by setting velocity directly as in example) or `Dynamic` with `gravityScale = 0`. `Collision Detection` = `Continuous Speculative` or `Continuous Dynamic`.
    *   Add `BoxCollider2D` (or `CapsuleCollider2D`): Adjust size. Set `Is Trigger` to true.
    *   Attach `Projectile.cs` script.
    *   Configure `Projectile` script fields:
        *   `Collision Layers`: Select layers like "Ground", "Walls".
        *   `Damageable Layers`: Select layer for "Player".
        *   `Impact Effect Prefab`: (Optional) Assign a particle effect for impact.
        *   `Max Lifespan`: e.g., 5s.

# Acceptance Criteria:
- `Projectile.cs` script is created and functional.
- Projectile prefab (`ArrowPrefab`) is created and configured with `Projectile.cs`, Rigidbody2D, Collider2D.
- `Launch(direction, speed, damage)` method initializes projectile's movement and properties.
- Projectile moves in the specified direction at the given speed.
- Projectile correctly rotates to face its direction of travel.
- Projectile is destroyed and (optionally) plays an impact effect on collision with objects on `collisionLayers`.
- Projectile deals `_damage` to `IDamageable` entities on `damageableLayers` upon collision and is then destroyed.
- Projectile is destroyed after `maxLifespan` if it doesn't hit anything.

# Test Strategy:
- Create a test scene.
- Write a simple "Launcher" script that instantiates `ArrowPrefab` and calls its `Launch()` method on a key press, targeting a specific point or direction.
- Place target objects (some with `IDamageable` like a mock player/enemy, some just static environment colliders) on appropriate layers.
- Test:
    - Projectile launching, movement, speed, rotation.
    - Collision with environment (should be destroyed).
    - Collision with `IDamageable` target (target takes damage, projectile destroyed).
    - Lifespan self-destruction.
    - Layer-based collision filtering.

# Notes/Questions:
- Projectile pooling should be considered for performance if many projectiles are fired (part of Phase 5.4 Object Pooling). For now, `Instantiate`/`Destroy` is fine.
- The choice of `Rigidbody2D.isKinematic = false` (dynamic) vs `true` (kinematic) for projectiles depends on how movement is handled. If setting `velocity` directly, dynamic is fine. If using `transform.Translate`, kinematic might be preferred to avoid unwanted physics interactions. The example uses `velocity` on a dynamic Rigidbody with 0 gravity.
- `_collider.isTrigger = true` is used to avoid physical pushback from projectiles and rely on `OnTriggerEnter2D` for hit detection.