# Task ID: 1.3.1
# Parent Task ID: 1.3
# Title: Implement Base EntityStatsSO and Derived Stats SOs
# Status: pending
# Dependencies: 1.1.2, 1.1.4
# Priority: critical
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement a base abstract `EntityStatsSO.cs` for common entity statistics and derive concrete ScriptableObjects: `PlayerStatsSO.cs`, `EnemyStatsSO.cs`, and `BossStatsSO.cs`.

# Details:
1.  **Implement `EntityStatsSO.cs`:**
    *   File Location: `Assets/_Project/Scripts/Core/Data/Stats/EntityStatsSO.cs`
    *   Namespace: `PetalsOfHope.Core.Data.Stats`
    *   Implementation:
        ```csharp
        // In Assets/_Project/Scripts/Core/Data/Stats/EntityStatsSO.cs
        namespace PetalsOfHope.Core.Data.Stats
        {
            using UnityEngine;

            public abstract class EntityStatsSO : ScriptableObject
            {
                [Header("Base Stats")]
                [Min(1)]
                [Tooltip("Maximum health of the entity.")]
                public int maxHealth = 100;

                // Add other common stats if any, e.g., defense, resistances in future
            }
        }
        ```

2.  **Implement `PlayerStatsSO.cs`:**
    *   File Location: `Assets/_Project/Scripts/Data/Player/PlayerStatsSO.cs` (Note: Path from plan, not Core)
    *   Namespace: `PetalsOfHope.Data.Player`
    *   Implementation:
        ```csharp
        // In Assets/_Project/Scripts/Data/Player/PlayerStatsSO.cs
        namespace PetalsOfHope.Data.Player
        {
            using UnityEngine;
            using PetalsOfHope.Core.Data.Stats; // For EntityStatsSO

            [CreateAssetMenu(menuName = "Petals of Hope/Data/Stats/Player Stats", fileName = "NewPlayerStatsSO")]
            public class PlayerStatsSO : EntityStatsSO
            {
                [Header("Player Specific Stats")]
                [Min(0f)]
                [Tooltip("Speed at which the player moves horizontally.")]
                public float movementSpeed = 5f;

                [Min(0f)]
                [Tooltip("Force applied to the player when jumping.")]
                public float jumpForce = 10f;

                // Add other player-specific stats: e.g., dashSpeed, dashDuration, wallJumpForce
            }
        }
        ```

3.  **Implement `EnemyStatsSO.cs`:**
    *   File Location: `Assets/_Project/Scripts/Data/Enemies/EnemyStatsSO.cs`
    *   Namespace: `PetalsOfHope.Data.Enemies`
    *   Implementation:
        ```csharp
        // In Assets/_Project/Scripts/Data/Enemies/EnemyStatsSO.cs
        namespace PetalsOfHope.Data.Enemies
        {
            using UnityEngine;
            using PetalsOfHope.Core.Data.Stats; // For EntityStatsSO

            [CreateAssetMenu(menuName = "Petals of Hope/Data/Stats/Enemy Stats", fileName = "NewEnemyStatsSO")]
            public class EnemyStatsSO : EntityStatsSO
            {
                [Header("Enemy Specific Stats")]
                [Min(0f)]
                [Tooltip("Speed at which the enemy patrols.")]
                public float patrolSpeed = 2f;

                [Min(0f)]
                [Tooltip("Speed at which the enemy chases the player.")]
                public float chaseSpeed = 4f;

                [Min(0f)]
                [Tooltip("Range at which the enemy can detect the player.")]
                public float detectionRange = 8f;

                [Min(0)]
                [Tooltip("Amount of damage the enemy deals on contact or attack.")]
                public int damage = 10;

                // Add other enemy-specific stats: e.g., attackRange, attackCooldown
            }
        }
        ```

4.  **Implement `BossStatsSO.cs`:**
    *   File Location: `Assets/_Project/Scripts/Data/Enemies/Bosses/BossStatsSO.cs`
    *   Namespace: `PetalsOfHope.Data.Enemies.Bosses`
    *   Implementation (can inherit from `EnemyStatsSO` or `EntityStatsSO` directly):
        ```csharp
        // In Assets/_Project/Scripts/Data/Enemies/Bosses/BossStatsSO.cs
        namespace PetalsOfHope.Data.Enemies.Bosses
        {
            using UnityEngine;
            // Assuming Bosses share common enemy stats, can add more boss-specific ones.
            // using PetalsOfHope.Data.Enemies; // For EnemyStatsSO

            [CreateAssetMenu(menuName = "Petals of Hope/Data/Stats/Boss Stats", fileName = "NewBossStatsSO")]
            public class BossStatsSO : EnemyStatsSO // Or EntityStatsSO if structure is very different
            {
                [Header("Boss Specific Stats")]
                [Tooltip("Special attack damage, for example.")]
                public int specialAttackDamage = 25;

                // Add other boss-specific stats or phases information
            }
        }
        ```

# Acceptance Criteria:
- `EntityStatsSO.cs` is created, abstract, and contains `maxHealth`.
- `PlayerStatsSO.cs` inherits `EntityStatsSO`, is creatable via asset menu, and contains `movementSpeed`, `jumpForce`.
- `EnemyStatsSO.cs` inherits `EntityStatsSO`, is creatable, and contains `patrolSpeed`, `chaseSpeed`, `detectionRange`, `damage`.
- `BossStatsSO.cs` inherits `EnemyStatsSO` (or `EntityStatsSO`), is creatable, and can hold boss-specific stats.
- All scripts compile without errors and are in their specified namespaces and locations.

# Test Strategy:
- Manual Verification:
    - Create instances of `PlayerStatsSO`, `EnemyStatsSO`, and `BossStatsSO` using the Asset Menu.
    - Inspect the created assets to ensure all fields are visible and editable.
    - Verify inheritance is correctly set up.

# Notes/Questions:
- The plan specifies path `_Project/Scripts/Data/Player/` for `PlayerStatsSO`, etc., which are outside `Core`. This is followed.
- `BossStatsSO` inheriting `EnemyStatsSO` seems reasonable unless boss mechanics diverge significantly.
- Consider adding `[Range]` or `[Min]` attributes for numeric fields to guide designers and prevent invalid data. Added `[Min]`.