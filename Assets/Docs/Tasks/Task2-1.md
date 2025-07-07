# Task ID: 2.1
# Parent Task ID: 2
# Title: Input System Implementation
# Status: completed
# Dependencies: 1.1.3, 1.2 # Unity Input System Package, Event Bus
# Priority: critical
# Estimated Effort: L (aggregate of subtasks)
# Assignee: Unassigned

# Description:
Implement the player input system using Unity's new Input System package. This involves creating input actions, an input reader ScriptableObject to broadcast input events, and testing with keyboard and gamepad.

# Details:
This system will reside in `PetalsOfHope.Core.Input`. It includes:
- Creating `PlayerInputActions.inputactions` asset.
- Defining Action Maps (`Gameplay`, `UI`) and Actions (`Move`, `Jump`, `Dash`, `Interact`, `Navigate`, `Submit`, `Cancel`).
- Implementing `InputReader.cs` ScriptableObject to handle input callbacks and raise events.
- Creating and configuring the `InputReader` SO asset.

Refer to subtasks 2.1.1, 2.1.2, and 2.1.3 for specific implementation details.

# Acceptance Criteria:
- All subtasks (2.1.1 - 2.1.3) are completed.
- Player input actions for gameplay and UI are defined.
- `InputReader` SO correctly processes raw input and raises corresponding events.
- Input can be switched between Gameplay and UI action maps.
- Input is verified with both keyboard and gamepad.

# Test Strategy:
- Manual testing: Use a test scene with listeners for all input events raised by `InputReader`.
- Verify correct events are raised with correct payloads (e.g., `Vector2` for Move) for keyboard and gamepad inputs.
- Test action map switching.

# Notes/Questions:
- This system decouples input processing from the player controller logic.