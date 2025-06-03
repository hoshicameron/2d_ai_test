# Task ID: 2.3
# Parent Task ID: 2
# Title: State Machine System Implementation
# Status: pending
# Dependencies: 1.1.2, 1.1.4 # Folder structure and namespace
# Priority: critical
# Estimated Effort: L (aggregate of subtasks)
# Assignee: Unassigned

# Description:
Implement a generic, reusable state machine system. This includes an abstract `BaseState` class and a `StateMachine` MonoBehaviour to manage and transition between states.

# Details:
This system will reside in `PetalsOfHope.Core.StateMachine`. It includes:
- `BaseState.cs`: An abstract class defining the interface for all states (`Enter`, `Exit`, `Update`, `FixedUpdate`).
- `StateMachine.cs`: A MonoBehaviour that holds the current state, calls its lifecycle methods, and handles state transitions.
- (Optional) `Transition.cs` base class for more complex transition logic.

Refer to subtasks 2.3.1, 2.3.2, and 2.3.3 (optional transition system) for specific implementation details.

# Acceptance Criteria:
- All core subtasks (2.3.1, 2.3.2) are completed.
- The state machine system allows for defining custom states.
- The `StateMachine` component can initialize with a starting state and transition between states.
- `Enter`, `Exit`, `Update`, and `FixedUpdate` methods of the current state are correctly called by the `StateMachine`.
- The system is unit tested.

# Test Strategy:
- Unit Testing: Test `StateMachine` logic for initialization, state transitions, and correct calling of state lifecycle methods.
- Manual/Integration Testing: Create a simple GameObject with a `StateMachine` and a few test states (e.g., logging messages in their lifecycle methods). Trigger transitions and verify behavior.

# Notes/Questions:
- This system will be the backbone for player character behavior and AI.