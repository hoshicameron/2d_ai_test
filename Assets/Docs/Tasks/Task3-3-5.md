# Task ID: 3.3.5
# Parent Task ID: 3.3
# Title: Test Enemy Behaviors and Interactions
# Status: pending
# Dependencies: 3.3.1, 3.3.2, 3.3.3, (potentially PlayerHealth implementing IDamageable)
# Priority: critical
# Estimated Effort: L
# Assignee: Unassigned

# Description:
Thoroughly test the implemented enemy types (Wolf, Spider, Archer Elf) in a test environment. Verify their AI behaviors (patrol, chase, attack), state transitions, and interactions with the player (damage dealing/taking).

# Details:
1.  **Setup Test Scene:**
    *   Create a scene `Assets/_Project/Scenes/Testing/EnemyTestScene.unity`.
    *   Include a Player character (from Phase 2, preferably with `PlayerHealth` now implementing `IDamageable` or having a temporary `IDamageable` wrapper for testing).
    *   Add varied terrain: platforms, walls, ceilings (if relevant for Spider).
    *   Place instances of `WolfPrefab`, `SpiderPrefab`, and `ArcherElfPrefab`.
2.  **Test Wolf Enemy:**
    *   Verify patrol path following.
    *   Verify player detection range and transition to chase.
    *   Verify chase behavior and speed.
    *   Verify attack initiation (charge/bite) and damage application to player.
    *   Verify player can damage and kill the wolf. Wolf death sequence.
3.  **Test Spider Enemy:**
    *   Verify initial state (e.g., ceiling hang).
    *   Verify player detection and drop mechanism.
    *   Verify ground behavior (patrol/chase/attack).
    *   Verify climbing if implemented.
    *   Verify combat interactions with player.
4.  **Test Archer Elf Enemy:**
    *   Verify patrol/positioning behavior.
    *   Verify player detection and LOS checks.
    *   Verify aiming and firing projectiles at player.
    *   Verify projectile accuracy, speed, and damage.
    *   Verify `fireRate` and attack cooldowns.
    *   Verify combat interactions with player.
5.  **General AI State Machine Testing:**
    *   For each enemy, observe state transitions using debug logs or an inspector that shows current AI state.
    *   Test edge cases: player appearing/disappearing suddenly, player unreachable.
6.  **Combat Feel and Balance (Initial Pass):**
    *   Assess if enemy attacks are avoidable.
    *   Assess if enemy health and damage values feel reasonable (initial pass, tuning will be ongoing).
7.  **Animator Integration:**
    *   Ensure all relevant animations (walk, run, attack, hurt, death) are playing correctly corresponding to AI states and actions.

# Acceptance Criteria:
- Wolf, Spider, and Archer Elf enemies demonstrate their core intended behaviors as per their design.
- AI state transitions occur correctly based on player actions and environmental context.
- Enemies correctly deal damage to a damageable player.
- Player can correctly deal damage to enemies, leading to their death.
- Projectiles (for Archer Elf) function as expected.
- Animations are generally synchronized with actions.
- No critical errors or crashes during enemy interactions.

# Test Strategy:
- As detailed in steps 1-7.
- Use Unity debugger to step through AI state logic if issues arise.
- Use Gizmos (e.g., for detection ranges, LOS) extensively for debugging AI.
- Record gameplay videos of interactions to review and identify issues.

# Notes/Questions:
- This is a crucial integration testing task for Milestone 3 (Alpha).
- For player to take damage, `PlayerHealth.cs` needs to implement `IDamageable` (Task 3.1.1) or have a temporary way to receive damage from enemies. If `PlayerHealth` does not yet implement `IDamageable`, this part of testing might be conceptual or require a temporary bridge. It's best if `PlayerHealth` implements `IDamageable`.
- Consider adding a temporary `IDamageable` implementation to `PlayerHealth` for this phase if it hasn't been refactored yet. (Refactor `PlayerHealth` to implement `IDamageable` would be a good small task to add before or during 3.3.5).