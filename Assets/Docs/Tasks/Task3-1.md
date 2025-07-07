# Task ID: 3.1
# Parent Task ID: 3
# Title: Implement EnemyBase Class & IDamageable Interface
# Status: pending
# Dependencies: 2.2, 1.3.1, 1.2.8 # AnimationController, EnemyStatsSO, EventSO assets
# Priority: critical
# Estimated Effort: L (aggregate of subtasks)
# Assignee: Unassigned

# Description:
Implement the `IDamageable` interface and the abstract `EnemyBase.cs` MonoBehaviour. `EnemyBase` will implement `IDamageable` and provide core functionalities for enemies like health management, taking damage, and dying.

# Details:
This task involves two main parts:
1.  Defining the `IDamageable` interface.
2.  Implementing the `EnemyBase` abstract class.

Refer to subtasks 3.1.1 and 3.1.2.

# Acceptance Criteria:
- `IDamageable` interface is defined.
- `EnemyBase` abstract class is implemented, requiring core components and managing health, damage, and death.
- `EnemyBase` correctly implements `IDamageable`.
- All subtasks are completed.

# Test Strategy:
- Test `EnemyBase` by creating a simple derived enemy class.
- Call `TakeDamage()` and verify health reduction and death behavior.
- Ensure `EnemyDiedEventSO` is raised.

# Notes/Questions:
- `PlayerHealth` (Task 2.5) should also ideally implement `IDamageable` for consistency in how damage is applied across entities. This can be a follow-up refactor.