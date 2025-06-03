# Task ID: 1.3.2
# Parent Task ID: 1.3
# Title: Implement AbilitySO and Placeholder Derived Abilities
# Status: completed
# Dependencies: 1.1.2, 1.1.4
# Priority: high
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement an abstract `AbilitySO.cs` ScriptableObject for common ability properties and create placeholder derived classes for specific abilities like `DoubleJumpSO` and `DashSO`.

# Details:
1.  **Implement `AbilitySO.cs`:**
    *   File Location: `Assets/_Project/Scripts/Data/Abilities/AbilitySO.cs`
    *   Namespace: `PetalsOfHope.Data.Abilities`
    *   Implementation:
        ```csharp
        // In Assets/_Project/Scripts/Data/Abilities/AbilitySO.cs
        namespace PetalsOfHope.Data.Abilities
        {
            using UnityEngine;

            public abstract class AbilitySO : ScriptableObject
            {
                [Header("Ability Info")]
                [Tooltip("Display name of the ability.")]
                public string abilityName = "New Ability";

                [Tooltip("Icon for the ability (e.g., for UI).")]
                public Sprite icon; // Assignable in Inspector

                [Min(0f)]
                [Tooltip("Cooldown time in seconds before the ability can be used again.")]
                public float cooldown = 1f;

                [TextArea(3,5)]
                [Tooltip("Description of the ability.")]
                public string description = "";

                // Abstract methods for ability activation logic, if shared behavior is needed
                // public abstract void Activate(GameObject owner);
                // public abstract void Deactivate(GameObject owner); // If applicable
            }
        }
        ```

2.  **Implement Placeholder Derived Ability Classes:**
    *   These will store specific parameters for each ability. The actual logic for abilities will be in Player States or Ability Controller systems.
    *   **`DoubleJumpSO.cs`:**
        *   File Location: `Assets/_Project/Scripts/Data/Abilities/Types/DoubleJumpSO.cs`
        *   Namespace: `PetalsOfHope.Data.Abilities.Types`
        *   Implementation:
            ```csharp
            // In Assets/_Project/Scripts/Data/Abilities/Types/DoubleJumpSO.cs
            namespace PetalsOfHope.Data.Abilities.Types
            {
                using UnityEngine;
                // using PetalsOfHope.Data.Abilities; // For AbilitySO, if not in same namespace

                [CreateAssetMenu(menuName = "Petals of Hope/Data/Abilities/Double Jump", fileName = "NewDoubleJumpAbilitySO")]
                public class DoubleJumpSO : AbilitySO
                {
                    [Header("Double Jump Specifics")]
                    [Min(0f)]
                    [Tooltip("Additional jump force for the double jump, relative to normal jump or absolute.")]
                    public float doubleJumpForceMultiplier = 1.0f; // Or a fixed force value

                    // Initialize base properties if needed
                    public DoubleJumpSO()
                    {
                        abilityName = "Double Jump";
                        cooldown = 0f; // Often no cooldown or handled by jump state
                        description = "Allows the player to perform an additional jump in mid-air.";
                    }
                }
            }
            ```
    *   **`DashSO.cs`:**
        *   File Location: `Assets/_Project/Scripts/Data/Abilities/Types/DashSO.cs`
        *   Namespace: `PetalsOfHope.Data.Abilities.Types`
        *   Implementation:
            ```csharp
            // In Assets/_Project/Scripts/Data/Abilities/Types/DashSO.cs
            namespace PetalsOfHope.Data.Abilities.Types
            {
                using UnityEngine;
                // using PetalsOfHope.Data.Abilities; // For AbilitySO

                [CreateAssetMenu(menuName = "Petals of Hope/Data/Abilities/Dash", fileName = "NewDashAbilitySO")]
                public class DashSO : AbilitySO
                {
                    [Header("Dash Specifics")]
                    [Min(0.1f)]
                    [Tooltip("Speed of the dash movement.")]
                    public float dashSpeed = 20f;

                    [Min(0.05f)]
                    [Tooltip("Duration of the dash in seconds.")]
                    public float dashDuration = 0.2f;

                    // Initialize base properties
                    public DashSO()
                    {
                        abilityName = "Dash";
                        cooldown = 1.5f;
                        description = "A quick burst of speed in the facing direction.";
                    }
                }
            }
            ```

# Acceptance Criteria:
- `AbilitySO.cs` is created, abstract, and contains `abilityName`, `icon`, `cooldown`, `description`.
- `DoubleJumpSO.cs` inherits `AbilitySO`, is creatable via asset menu, and has placeholder fields like `doubleJumpForceMultiplier`.
- `DashSO.cs` inherits `AbilitySO`, is creatable, and has placeholder fields like `dashSpeed`, `dashDuration`.
- All scripts compile without errors and are in their specified namespaces/locations.
- Base properties in derived SOs are pre-filled with sensible defaults via constructor or directly.

# Test Strategy:
- Manual Verification:
    - Create instances of `DoubleJumpSO` and `DashSO` using the Asset Menu.
    - Inspect the created assets to ensure all fields (inherited and specific) are visible and editable.
    - Verify default values are set.

# Notes/Questions:
- The actual execution logic for abilities is not part of these SOs; they are data containers. Logic will be in player states or an ability component.
- The constructors in derived SOs are used to set default values for `abilityName`, `description`, etc.
- Kept `AbilitySO` as a pure data container with logic handled by other components.
- Implementation completed on 2025-06-03. All ability classes have been implemented with proper inheritance and default values.