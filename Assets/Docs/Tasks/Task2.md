# Task ID: 2
# Parent Task ID: None
# Title: Phase 2: Core Gameplay Systems Implementation
# Status: pending
# Dependencies: 1 # Foundation Systems
# Priority: critical
# Estimated Effort: XL (aggregate of subtasks)
# Assignee: Unassigned

# Description:
This task is a parent epic for implementing the core gameplay systems of "Petals of Hope". This phase builds upon the foundation systems and introduces player interaction, animation, state management, and camera control.

# Details:
This phase includes:
- Input System: Handling player input via Unity's Input System package.
- Reusable Animation Controller: A decoupled component for managing character animations.
- State Machine System: A generic state machine for player and AI.
- Player Controller & States: Implementing player movement, actions, and various states.
- Player Health: Managing player health and death.
- Camera System: Implementing camera follow and potentially other behaviors.

Refer to subtasks for specific implementation details.

# Acceptance Criteria:
- All subtasks under Phase 2 (2.1.x, 2.2.x, etc.) are completed and verified.
- Core player mechanics (movement, jumping, basic actions) are functional.
- Player character is animated.
- Camera follows the player appropriately.
- Milestone 2: Basic Gameplay (End of Week 5) is achieved.

# Test Strategy:
- Extensive playtesting of player controls and core loop.
- Integration testing between Input, Player Controller, State Machine, Animation, and Camera systems.
- Verify player character responds correctly to inputs and transitions between states smoothly.

# Notes/Questions:
- This phase is crucial for establishing the "feel" of the game.