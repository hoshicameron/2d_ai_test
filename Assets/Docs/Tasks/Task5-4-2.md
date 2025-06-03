# Task ID: 5.4.2
# Parent Task ID: 5.4
# Title: Implement/Refine Object Pooling for Key Systems
# Status: pending
# Dependencies: 3.3.4 (Projectile System), 5.2.1 (AudioManager), 5.3.1 (VFXManager), (potentially Enemy spawning logic)
# Priority: high
# Estimated Effort: L
# Assignee: Unassigned

# Description:
Review and implement or refine object pooling for frequently instantiated/destroyed objects. This primarily includes projectiles, particle effects (VFX), and SFX AudioSources. If enemies are spawned frequently in large numbers, consider pooling for them too.

# Details:
1.  **Review Existing Pooling:**
    *   `AudioManager` (Task 5.2.1) should have basic SFX AudioSource pooling. Review its effectiveness and robustness (e.g., what happens if pool is exhausted?).
    *   `VFXManager` (Task 5.3.1, if implemented) should have VFX pooling. Review similarly.
2.  **Projectile Pooling:**
    *   Modify the `Projectile.cs` (Task 3.3.4) and systems that fire projectiles (e.g., `ArcherElfEnemy.cs`) to use a pooling system.
    *   Create a `ProjectilePoolManager.cs` (similar to `VFXManager`) or integrate projectile pooling into a generic `ObjectPoolManager.cs`.
    *   **Pool Manager Logic:**
        *   Pre-instantiate a number of projectile prefabs.
        *   When a projectile is needed, grab one from the pool, activate it, and call its `Launch()` method.
        *   When a projectile hits something or its lifespan ends, instead of `Destroy(gameObject)`, it deactivates itself and returns to the pool.
        *   `Projectile.DestroyProjectile()` method should be changed to notify the pool.
3.  **Enemy Pooling (If Deemed Necessary):**
    *   If levels involve very frequent spawning and despawning of many enemies (e.g., wave-based encounters), consider pooling for common enemy types.
    *   This is more complex as enemies have AI states, health, etc., that need proper reset when reused from a pool.
    *   `EnemyBase.Die()` would need to return the enemy to its pool instead of destroying it. A `ResetForPooling()` method would be needed on `EnemyBase`.
4.  **General `ObjectPoolManager.cs` (Optional Abstraction):**
    *   Consider creating a generic object pooling class that can manage pools for various prefab types, configured by name or prefab reference. `VFXManager` and `AudioManager` could potentially use this generic pooler internally.
    *   Example: `ObjectPoolManager.Instance.SpawnFromPool("Arrow", position, rotation)` and `ObjectPoolManager.Instance.ReturnToPool("Arrow", gameObjectInstance)`.

# Acceptance Criteria:
- Projectiles are managed by an object pooling system. Instantiation/destruction of projectiles during gameplay is minimized.
- VFX pooling (via `VFXManager` or generic pooler) is robust and effectively reuses particle systems.
- SFX AudioSource pooling (via `AudioManager` or generic pooler) is robust.
- (If implemented) Enemy pooling reduces instantiation overhead for frequently spawned enemies.
- Profiling shows a significant reduction in `Instantiate` calls and GC allocations related to these objects during gameplay.

# Test Strategy:
- **Stress Testing:**
    - Create scenarios where many projectiles are fired rapidly.
    - Trigger many VFX and SFX in quick succession.
    - (If enemy pooling) Spawn many enemies.
- **Profiling:**
    - Use Unity Profiler to monitor `Instantiate`/`Destroy` calls and GC allocations before and after pooling implementation.
    - Verify that objects are being correctly reused from pools (e.g., by observing object names or IDs in Hierarchy, or pool counts via debug logs).
- **Functional Testing:** Ensure pooled objects behave correctly when reused (e.g., projectiles fly correctly, VFX play fully, enemies reset state properly).

# Notes/Questions:
- Implementing a robust, generic object pooler can be a significant task but offers long-term benefits.
- Pay close attention to resetting the state of pooled objects when they are reused.
- The plan mentions "Object Pooling: Implement for projectiles, particles, frequent SFX AudioSources, and potentially enemies."