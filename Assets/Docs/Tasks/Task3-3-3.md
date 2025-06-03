# Task ID: 3.3.3
# Parent Task ID: 3.3
# Title: Implement ArcherElfEnemy Script and Prefab (and basic Projectile System)
# Status: pending
# Dependencies: 3.1.2, 3.2.6, 1.3.5, 3.3.4 (Projectile System) # EnemyBase, AI State SOs, EnemyStatsSO assets
# Priority: medium
# Estimated Effort: XL (includes projectile system)
# Assignee: Unassigned

# Description:
Implement the `ArcherElfEnemy.cs` script for ranged attack behavior. Create an Archer Elf prefab. This task also includes implementing a basic projectile system required for the Archer Elf's attack.

# Details:
1.  **Create `ArcherElfEnemy.cs` Script:**
    *   File Location: `Assets/_Project/Scripts/Enemies/Types/ArcherElfEnemy.cs`
    *   Namespace: `PetalsOfHope.Enemies.Types`
    *   Implementation:
        ```csharp
        // In Assets/_Project/Scripts/Enemies/Types/ArcherElfEnemy.cs
        namespace PetalsOfHope.Enemies.Types
        {
            using UnityEngine;
            using PetalsOfHope.Enemies.Core;

            public class ArcherElfEnemy : EnemyBase
            {
                [Header("Archer Specifics")]
                public GameObject projectilePrefab; // Assign Arrow prefab here
                public Transform firePoint;         // Point from where arrow is fired
                public float fireRate = 2f;         // Seconds between shots (can be part of AttackState SO)
                // Line of sight detection is crucial for archers, likely handled in AI states.

                // This method would be called by Archer's AttackState
                public void FireProjectile(Vector2 targetDirection)
                {
                    if (projectilePrefab == null || firePoint == null)
                    {
                        Debug.LogWarning("Projectile prefab or fire point not set for Archer Elf.", this);
                        return;
                    }

                    GameObject projectileInstance = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
                    Projectile projectileScript = projectileInstance.GetComponent<Projectile>();
                    if (projectileScript != null)
                    {
                        projectileScript.Launch(targetDirection, Stats.projectileSpeed, Stats.damage); // Assuming projectileSpeed in EnemyStatsSO
                    }
                    else
                    {
                        Debug.LogError("Projectile prefab is missing Projectile script.", projectileInstance);
                    }
                    // Play fire animation/sound
                    _animationController?.Play("Shoot"); // Example
                }
            }
        }
        ```
2.  **Modify/Create Archer Elf AI States:**
    *   `ArcherAttackStateSO`: This state would calculate `targetDirection` towards the player. Instead of melee logic, it would call `ArcherElfEnemy.FireProjectile(targetDirection)`. It needs to manage `fireRate`. It should ensure Line of Sight (LOS) to the player.
    *   `ChaseState` for Archer Elf might try to maintain an optimal distance rather than closing to melee range. Or it could be a `KiteState`.
3.  **Create Archer Elf Animator Controller:**
    *   States: `Idle`, `Walk`, `Aim`, `Shoot`, `Hurt`, `Death`.
4.  **Create Archer Elf Prefab:**
    *   Setup similar to other enemies.
    *   Assign `ArcherElfEnemy.cs`.
    *   Assign `ArcherElfStatsSO` (ensure it has `projectileSpeed`, `damage` for projectile).
    *   Configure AI `StateMachine` with Archer-specific AI State SOs.
    *   Add `firePoint` child Transform. Assign `projectilePrefab` (created in Task 3.3.4).

# Acceptance Criteria:
- `ArcherElfEnemy.cs` script exists and includes `FireProjectile` method.
- `ArcherAttackStateSO` (or modified generic `AttackState`) calls `FireProjectile`.
- Archer Elf prefab is configured.
- Archer Elf detects player, aims (faces player), and fires projectiles at `fireRate`.
- Projectiles travel in the target direction (Task 3.3.4 handles projectile behavior).
- Archer Elf can be damaged/dies. Line of sight detection is functional for attacks.

# Test Strategy:
- Place `ArcherElfPrefab` in a test scene with player and obstacles (for LOS).
- Verify Archer detects player, enters attack state.
- Verify Archer aims at player and fires projectiles from `firePoint`.
- Check `fireRate`.
- Test LOS: Archer should not fire if player is behind a wall.
- Test combat interactions.

# Notes/Questions:
- The projectile system (Task 3.3.4) is a critical dependency.
- Archer AI will be more complex due to ranged combat and LOS requirements. `ChaseState` might become a "RepositionState" or "KiteState".
- `EnemyStatsSO` for Archer Elf needs `projectileSpeed`. `damage` from `EnemyStatsSO` could be used for projectile damage.