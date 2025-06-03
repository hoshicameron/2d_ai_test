# Task ID: 12
# Parent Task ID: None
# Title: AI System (State Machine based)
# Status: completed
# Dependencies: 11
# Priority: high
# Estimated Effort: M
# Assignee: AI

# Description:
Implement the AI System using a State Machine.

# Details:
1. Implement `State.cs` (abstract `ScriptableObject` or class) in `_Project/Scripts/AI/Core/`.
2. Implement `StateMachine.cs` (MonoBehaviour) in `_Project/Scripts/AI/Core/`.
3. Create concrete states (e.g., `PatrolState`, `ChaseState`, `AttackState`) in `_Project/Scripts/AI/States/`.

# Acceptance Criteria:
- `State` and `StateMachine` are implemented correctly.
- Concrete states are functional and handle transitions correctly.

# Test Strategy:
- Unit tests for state machine logic.
- Manual testing of AI behavior in the Unity Editor.

# Notes/Questions:
- Ensure that the AI System is properly integrated with the EnemyBase class.
- Verify that state transitions are handled correctly.
