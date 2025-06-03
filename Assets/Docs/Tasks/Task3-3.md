# Task ID: 3.3
# Parent Task ID: 3
# Title: Enemy Types & Prefabs Implementation
# Status: pending
# Dependencies: 3.1.2, 3.2, 1.3.5 # EnemyBase, AI System, EnemyStatsSO assets
# Priority: critical
# Estimated Effort: XL (aggregate of subtasks)
# Assignee: Unassigned

# Description:
Create concrete enemy types by deriving from `EnemyBase`, configuring their specific behaviors and `StateMachine`s. Create prefabs for these enemy types with all necessary components.

# Details:
This involves:
- Creating C# scripts for specific enemy types (e.g., `WolfEnemy.cs`, `SpiderEnemy.cs`, `ArcherElfEnemy.cs`).
- These scripts might override methods from `EnemyBase` or add unique behaviors.
- Creating prefabs for each enemy type.
- Configuring each prefab with `EnemyBase` (or derived script), `Rigidbody2D`, `Collider2D`, `AnimationController` (Unity's), `CoreAnimation` (our script), AI `StateMachine`, `PatrolPath`, and assigning `EnemyStatsSO` and AI State SOs.
- Testing enemy behaviors and interactions.
- Implementing a basic projectile system if needed (e.g., for Archer Elf).

Refer to subtasks 3.3.1, 3.3.2, 3.3.3, 3.3.4 (for Archer Elf projectile).

# Acceptance Criteria:
- All subtasks are completed.
- At least 1-2 distinct enemy types (e.g., Wolf, Spider) are implemented as prefabs.
- Enemies exhibit their intended behaviors (patrol, chase, attack) driven by their AI StateMachines and configured stats/states.
- Enemies can be damaged by the player and can damage the player (assuming PlayerHealth implements `IDamageable` or a temporary damage mechanism is in place).
- Archer Elf (if implemented in this first pass) can fire projectiles.

# Test Strategy:
- Place enemy prefabs in a test level with the player.
- Observe and verify individual enemy behaviors (patrol patterns, detection, chase, attack execution).
- Test combat: player attacks enemy, enemy attacks player. Verify health changes and death.
- Verify projectiles from Archer Elf (if implemented) are fired correctly and can interact with player/environment.

# Notes/Questions:
- The plan lists Wolf, Spider, and Archer Elf. This first pass might focus on Wolf and one other to establish the pattern.
- Art and animation assets for enemies are assumed to be placeholders if final assets are not ready. Dummy animations in Animator Controllers are sufficient for testing state transitions.