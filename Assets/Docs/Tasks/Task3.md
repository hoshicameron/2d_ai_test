# Task ID: 3
# Parent Task ID: None
# Title: Phase 3: Enemy Systems Implementation
# Status: pending
# Dependencies: 2 # Core Gameplay Systems (esp. State Machine, AnimationController), 1.3.1 (EnemyStatsSO)
# Priority: critical
# Estimated Effort: XL (aggregate of subtasks)
# Assignee: Unassigned

# Description:
This task is a parent epic for implementing enemy systems. This includes a base class for enemies, an AI system (state machine-based), and concrete enemy types with basic behaviors and prefabs.

# Details:
This phase introduces non-player characters that challenge the player. Key components:
- `IDamageable` interface for entities that can take damage.
- `EnemyBase.cs`: An abstract base class for all enemies, handling common enemy logic like health, taking damage, and death.
- AI System: Utilizing the `StateMachine` system (from Phase 2.3) for enemy AI behaviors.
- Concrete AI States: `PatrolState`, `ChaseState`, `AttackState`.
- Basic Enemy Types: Implementing specific enemies like Wolf, Spider, Archer Elf with distinct behaviors and prefabs.

Refer to subtasks for specific implementation details.

# Acceptance Criteria:
- All subtasks under Phase 3 (3.1.x, 3.2.x, 3.3.x) are completed and verified.
- `IDamageable` interface is defined and used by `EnemyBase`.
- `EnemyBase` class provides common enemy functionalities.
- AI state machine system drives enemy behavior.
- At least 1-2 basic enemy types are implemented with distinct behaviors (patrol, chase, attack).
- Player can interact with enemies (e.g., deal damage, take damage).
- Milestone 3: Enemy Systems & Alpha (End of Week 8) is achieved.

# Test Strategy:
- Test enemy behaviors in isolated test scenes and integrated with player.
- Verify state transitions in AI (e.g., patrol -> chase on player detection, chase -> attack in range).
- Test combat interactions: player damaging enemy, enemy damaging player (if PlayerHealth is damageable).
- Debug AI behavior using logs and visualization tools (e.g., Gizmos for detection ranges).

# Notes/Questions:
- This phase is key to making the game world interactive and challenging.
- Placeholder art/animations for enemies can be used if final assets are not ready.