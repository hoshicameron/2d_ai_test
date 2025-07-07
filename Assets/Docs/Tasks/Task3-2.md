# Task ID: 3.2
# Parent Task ID: 3
# Title: AI System (State Machine based) Implementation
# Status: pending
# Dependencies: 2.3, 3.1.2, 1.2 # StateMachine System, EnemyBase, Event Bus
# Priority: critical
# Estimated Effort: XL (aggregate of subtasks)
# Assignee: Unassigned

# Description:
Implement an AI system for enemies based on the generic `StateMachine` system (from Phase 2.3). This includes defining an AI-specific `State` base (if different from `Core.StateMachine.BaseState` or if using ScriptableObjects for states) and an AI `StateMachine` MonoBehaviour.

# Details:
This task focuses on setting up the structure for AI behaviors.
- Define `AI.Core.State.cs`: An abstract class or ScriptableObject for AI states (`EnterState`, `ExecuteState`, `ExitState`).
- Define `AI.Core.StateMachine.cs`: A MonoBehaviour tailored for AI, holding references to `EnemyBase` and `AnimationController`, and managing AI states.
- Implement concrete AI states: `PatrolState`, `ChaseState`, `AttackState`.
- Configure states as ScriptableObjects if that pattern is chosen.
- Integrate AI with Event Bus for communication (e.g., `EnemyDetectedPlayerEventSO`).

Refer to subtasks 3.2.1 through 3.2.5.

# Acceptance Criteria:
- All subtasks (3.2.1 - 3.2.5) are completed.
- The AI system allows for defining distinct behaviors as states.
- The AI `StateMachine` can manage these states and transition between them.
- Concrete states for patrol, chase, and attack are implemented.
- AI behaviors can be configured (e.g., via ScriptableObject state assets).

# Test Strategy:
- Test each AI state's logic in isolation and in sequence.
- Use debug logs and Gizmos to visualize AI decisions (e.g., detection ranges, pathing).
- Verify state transitions based on environmental or player-triggered conditions.

# Notes/Questions:
- The plan specifies AI states as `ScriptableObject` or class. `ScriptableObject` states are good for designer-configurable parameters per state instance.
- The `AI.StateMachine` will be similar to the `Core.StateMachine` but might have specific references (like `EnemyBase`) useful for AI states. The plan has a separate `StateMachine.cs` under AI.