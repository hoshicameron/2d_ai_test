# Task ID: 3.1.1
# Parent Task ID: 3.1
# Title: Define IDamageable Interface
# Status: completed
# Dependencies: 1.1.2, 1.1.4 # Folder structure and namespace
# Priority: critical
# Estimated Effort: S
# Assignee: Unassigned

# Description:
Define an `IDamageable.cs` interface that outlines the contract for any game entity that can receive damage.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/Interfaces/IDamageable.cs`
2.  **Namespace:** `PetalsOfHope.Interfaces`
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/Interfaces/IDamageable.cs
    namespace PetalsOfHope.Interfaces
    {
        using UnityEngine; // Might be needed if methods pass GameObject or Vector3 for hit direction

        /// <summary>
        /// Interface for entities that can take damage.
        /// </summary>
        public interface IDamageable
        {
            /// <summary>
            /// Applies damage to the entity.
            /// </summary>
            /// <param name="amount">The amount of damage to apply.</param>
            void TakeDamage(int amount);

            // Optional: Extend with more context if needed by gameplay systems
            // void TakeDamage(int amount, GameObject instigator);
            // void TakeDamage(int amount, Vector3 hitPoint, Vector3 hitDirection);
            // bool IsDead { get; } // Could be useful
        }
    }
    ```

# Acceptance Criteria:
- `IDamageable.cs` interface is created in the specified location and namespace.
- It defines a method `void TakeDamage(int amount)`.
- (Optional) Considerations for extended `TakeDamage` signatures (with instigator, hit details) are noted.
- The script compiles without errors.

# Test Strategy:
- This interface will be tested via its implementations (`EnemyBase`, potentially `PlayerHealth`).
- Code review for clarity and completeness of the contract.

# Notes/Questions:
- The plan specifies `void TakeDamage(int amount)`. More complex versions with `instigator` or hit details can be added later if combat mechanics require them. For now, keeping it simple.
- Consider adding a property like `bool IsImmune { get; }` or `bool IsDead { get; }` if frequently needed by damage-dealing systems to check before applying damage, but this can also be handled by implementations.