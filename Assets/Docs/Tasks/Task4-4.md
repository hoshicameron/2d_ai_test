# Task ID: 4.4
# Parent Task ID: 4
# Title: Player Ability System & Advanced States Implementation
# Status: competed
# Dependencies: 2.4, 1.3.2, 4.5 (GameProgressionManager) # PlayerController, AbilitySO, GameProgressionManager
# Priority: high
# Estimated Effort: XL (aggregate of subtasks)
# Assignee: Unassigned

# Description:
Implement a system for managing player abilities (`PlayerAbilities.cs`) and integrate advanced player states (Double Jump, Wall Grab, Wall Jump, Dash) into the `PlayerController`'s state machine. These abilities may be unlocked via game progression.

# Details:
This task expands the player's capabilities with new movement options.
- `PlayerAbilities.cs`: A component on the Player that manages which abilities are currently available/unlocked, potentially checking against `InventorySystem` or `GameProgressionManager`.
- Advanced Player States:
    - `DoubleJumpState.cs`
    - `WallGrabState.cs`
    - `WallJumpState.cs`
    - `DashState.cs`
- These states will be triggered by input if the corresponding ability is available/unlocked.
- Abilities might be unlocked via Talismans, with `GameProgressionManager` mediating this.

Refer to subtasks 4.4.1 through 4.4.6.

# Acceptance Criteria:
- All subtasks (4.4.1 - 4.4.6) are completed.
- `PlayerAbilities.cs` component is implemented and can report ability availability.
- Advanced player states (Double Jump, Wall Grab, Wall Jump, Dash) are functional.
- Player can perform these actions if the ability is unlocked and input is provided.
- Abilities are integrated with `PlayerController`'s state machine.
- (Later) Ability unlocking is tied to `GameProgressionManager`.

# Test Strategy:
- Test each ability in a dedicated test level with appropriate geometry (e.g., walls for wall jump, gaps for double jump/dash).
- Verify state transitions into and out of ability states.
- Test ability usage with both keyboard and gamepad.
- Test ability availability checks (e.g., try to use an ability before it's "unlocked").

# Notes/Questions:
- This phase significantly enhances player mobility and depth of gameplay.
- Tuning of ability parameters (forces, durations, cooldowns from `AbilitySO`s) will be important.
- Visual and audio feedback for abilities (animations, VFX, SFX) will be part of Phase 5 polish but placeholders can be used.