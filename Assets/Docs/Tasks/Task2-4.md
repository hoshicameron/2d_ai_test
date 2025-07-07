# Task ID: 2.4
# Parent Task ID: 2
# Title: Player Controller & States Implementation
# Status: completed
# Dependencies: 2.1, 2.2, 2.3, 1.3.1 # Input System, AnimationController, StateMachine System, PlayerStatsSO
# Priority: critical
# Estimated Effort: XL (aggregate of subtasks)
# Assignee: Unassigned

# Description:
Implement the `PlayerController.cs` MonoBehaviour and the associated player states (Idle, Moving, Jumping, Falling). This system will handle player movement, actions, and animation based on input and game physics.

# Details:
This is a major gameplay component residing in `PetalsOfHope.Gameplay.Player`. It involves:
- `PlayerController.cs`: The main MonoBehaviour managing player components, data, input handling, and state machine interaction.
- Player States (`IdleState.cs`, `MovingState.cs`, `JumpingState.cs`, `FallingState.cs`): Concrete `BaseState` implementations for player behaviors.
- Integration with `InputReader`, `AnimationController`, `StateMachine`, and `PlayerStatsSO`.
- Ground checking mechanism.
- Raising events for significant player actions.

Refer to subtasks 2.4.1 through 2.4.6 for specific implementation details.

# Acceptance Criteria:
- All subtasks (2.4.1 - 2.4.6) are completed.
- Player character can move left/right and jump in response to input.
- Player character transitions between Idle, Moving, Jumping, and Falling states correctly.
- Animations for these states are triggered via `AnimationController`.
- Player movement uses values from `PlayerStatsSO`.
- Ground check mechanism functions reliably.
- Player feels responsive and controllable.

# Test Strategy:
- Extensive playtesting in a test level with basic platforms.
- Verify smooth state transitions and animations.
- Check movement speed and jump height against `PlayerStatsSO` values.
- Test ground check on various surfaces and edges.
- Debug state transitions and input handling using console logs and the Inspector.

# Notes/Questions:
- This system defines the core feel of the platforming gameplay.
- Close collaboration with design and animation will be needed for tuning.