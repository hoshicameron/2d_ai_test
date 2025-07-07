# Task ID: 11
# Parent Task ID: None
# Title: EnemyBase Class & IDamageable Interface
# Status: completed
# Dependencies: 1, 7
# Priority: high
# Estimated Effort: M
# Assignee: AI

# Description:
Implement the EnemyBase class and IDamageable interface.

# Details:
1. Implement `IDamageable.cs` interface in `_Project/Scripts/Interfaces/`.
2. Implement `EnemyBase.cs` (abstract MonoBehaviour) in `_Project/Scripts/Enemies/Core/`.

# Acceptance Criteria:
- `IDamageable` interface is defined correctly.
- `EnemyBase` is implemented correctly and implements `IDamageable`.

# Test Strategy:
- Unit tests for damage handling logic.
- Manual testing of enemy damage and death in the Unity Editor.

# Notes/Questions:
- Ensure that the EnemyBase class is properly integrated with the Event Bus.
- Verify that damage handling is correctly implemented.
