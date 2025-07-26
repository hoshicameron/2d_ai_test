# Task ID: 4.4.1
# Parent Task ID: 4.4
# Title: Implement PlayerAbilities Component
# Status: competed
# Dependencies: 1.3.2, 4.5 (GameProgressionManager), 4.3.2 (InventorySystem) # AbilitySO definition, GPM, Inventory
# Priority: high
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement `PlayerAbilities.cs`, a MonoBehaviour component to be added to the Player GameObject. This component will track which abilities the player has unlocked and can currently use. It will provide methods to check for ability availability, potentially by querying `GameProgressionManager` or `InventorySystem`.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/Gameplay/Player/Abilities/PlayerAbilities.cs`
2.  **Namespace:** `PetalsOfHope.Gameplay.Player.Abilities`
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/Gameplay/Player/Abilities/PlayerAbilities.cs
    namespace PetalsOfHope.Gameplay.Player.Abilities
    {
        using UnityEngine;
        using PetalsOfHope.Data.Abilities; // For AbilitySO and specific types like DoubleJumpSO, DashSO
        using PetalsOfHope.Systems.Progression; // For GameProgressionManager (once implemented)
        // using PetalsOfHope.Systems.Inventory; // For InventorySystem (if checking talismans directly)

        public class PlayerAbilities : MonoBehaviour
        {
            // References to specific AbilitySO assets for each ability type
            // These define the properties of the abilities (cooldowns, etc.)
            [Header("Ability Data")]
            [SerializeField] private AbilitySO _doubleJumpAbilityData; // Assign DoubleJumpSO asset
            [SerializeField] private AbilitySO _dashAbilityData;       // Assign DashSO asset
            [SerializeField] private AbilitySO _wallJumpAbilityData;   // Assign WallJumpSO asset (if it has data like force/cooldown)
            // WallGrab might not need an AbilitySO if it's a passive skill once unlocked.

            // Runtime state for abilities
            private float _dashCooldownTimer = 0f;
            // Add other cooldown timers or charges if abilities have them

            // Dependencies (can be injected or found)
            private GameProgressionManager _progressionManager;
            // private InventorySystem _inventorySystem;

            // Ability IDs (could be enums or strings matching AbilitySO.abilityID if that exists)
            // For simplicity, we'll use direct AbilitySO references to check unlock status for now.
            public const string DOUBLE_JUMP_ABILITY_ID = "DoubleJump"; // Example ID
            public const string DASH_ABILITY_ID = "Dash";
            public const string WALL_JUMP_ABILITY_ID = "WallJump";
            public const string WALL_GRAB_ABILITY_ID = "WallGrab";


            private void Start()
            {
                // Find or get injected dependencies
                _progressionManager = FindObjectOfType<GameProgressionManager>(); // Simple find; injection is better
                // _inventorySystem = InventorySystem.Instance;
                if (_progressionManager == null) Debug.LogWarning("GameProgressionManager not found by PlayerAbilities.", this);
            }

            private void Update()
            {
                // Update cooldowns
                if (_dashCooldownTimer > 0)
                {
                    _dashCooldownTimer -= Time.deltaTime;
                }
                // Update other cooldowns
            }

            // --- Ability Availability Checks ---

            public bool CanDoubleJump()
            {
                // Check if ability is unlocked via GameProgressionManager
                // And if any other conditions are met (e.g., not on cooldown, has charges)
                return _progressionManager != null && _progressionManager.IsAbilityUnlocked(DOUBLE_JUMP_ABILITY_ID);
                // Could also check: && _doubleJumpAbilityData != null;
            }

            public bool CanDash()
            {
                bool isUnlocked = _progressionManager != null && _progressionManager.IsAbilityUnlocked(DASH_ABILITY_ID);
                bool isOffCooldown = _dashCooldownTimer <= 0f;
                return isUnlocked && isOffCooldown && _dashAbilityData != null;
            }
            
            public bool CanWallJump()
            {
                 return _progressionManager != null && _progressionManager.IsAbilityUnlocked(WALL_JUMP_ABILITY_ID);
            }

            public bool CanWallGrab()
            {
                 return _progressionManager != null && _progressionManager.IsAbilityUnlocked(WALL_GRAB_ABILITY_ID);
            }

            // --- Ability Usage Notification & Cooldown Management ---

            public void NotifyDashUsed()
            {
                if (_dashAbilityData != null)
                {
                    _dashCooldownTimer = _dashAbilityData.cooldown;
                    // Raise DashUsedEventSO if exists, for UI to show cooldown
                }
            }

            // Getters for ability data if states need them (e.g., DashState needs dash speed/duration)
            public AbilitySO GetDashAbilityData() => _dashAbilityData;
            public AbilitySO GetDoubleJumpAbilityData() => _doubleJumpAbilityData;
            public AbilitySO GetWallJumpAbilityData() => _wallJumpAbilityData;


            // ISaveable implementation might be needed if ability states (like cooldowns) persist
            // For now, assuming cooldowns reset on load/scene change unless explicitly saved.
            // Unlocked status is handled by GameProgressionManager's ISaveable.
        }
    }
    ```

# Acceptance Criteria:
- `PlayerAbilities.cs` MonoBehaviour is created.
- It holds references to `AbilitySO` assets for different abilities (Double Jump, Dash, Wall Jump).
- It provides public methods like `CanDoubleJump()`, `CanDash()`, `CanWallJump()`, `CanWallGrab()`.
- These methods check if the respective ability is unlocked by querying `GameProgressionManager.IsAbilityUnlocked(AbilityID)`. (Method name on `GameProgressionManager` TBD).
- It manages cooldowns for abilities like Dash (`_dashCooldownTimer`, `NotifyDashUsed()`).
- Script compiles without errors. `GameProgressionManager` is a dependency that will be implemented in Task 4.5.

# Test Strategy:
- Manual/Integration Testing (once `GameProgressionManager` is mockable/available):
    - Add `PlayerAbilities` to the Player GameObject. Assign `AbilitySO` assets.
    - Create a test script to call `CanDoubleJump()`, `CanDash()`, etc.
    - Mock `GameProgressionManager.IsAbilityUnlocked()` to return true/false for different abilities.
    - Verify the `Can...()` methods return correct values based on mock progression.
    - Test Dash cooldown: Call `CanDash()`, then `NotifyDashUsed()`. Verify `CanDash()` returns false until cooldown expires, then true again.

# Notes/Questions:
- The actual ability IDs (`DOUBLE_JUMP_ABILITY_ID`, etc.) need to be consistent with how `GameProgressionManager` stores/identifies unlocked abilities. Using string constants here is one way; enums or `AbilitySO.abilityID` could also be used.
- `PlayerController` and its states will query `PlayerAbilities` before attempting to perform an ability.
- Cooldowns are managed locally in `PlayerAbilities`. If cooldowns need to persist across game sessions, `PlayerAbilities` would need to implement `ISaveable`. For now, this is not specified for cooldowns.
- Finding `GameProgressionManager` via `FindObjectOfType` is simple but can be fragile. Dependency injection or a service locator pattern is more robust for larger projects.