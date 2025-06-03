# Task ID: 4.4.6
# Parent Task ID: 4.4
# Title: Integrate and Test All Advanced Player Abilities
# Status: pending
# Dependencies: 4.4.1, 4.4.2, 4.4.3, 4.4.4, 4.4.5 # PlayerAbilities & all advanced states
# Priority: critical
# Estimated Effort: L
# Assignee: Unassigned

# Description:
Integrate all implemented advanced player abilities (Double Jump, Wall Grab, Wall Jump, Dash) with the `PlayerController` and `PlayerAbilities` systems. Conduct thorough testing of these abilities in combination and ensure they function correctly based on unlock status (mocked for now).

# Details:
1.  **Verify `PlayerController.Awake()`:**
    *   Ensure all new state instances (`DoubleJumpState`, `WallGrabState`, `WallJumpState`, `DashState`) are correctly created and assigned.
    *   Ensure `PlayerAbilities` component is get/assigned.
2.  **Verify `PlayerController.Update/FixedUpdate/InputHandlers`:**
    *   Ensure wall check logic (`CheckWallCollision()`) is called.
    *   Ensure dash input (`HandleDashInput()`) is correctly setting `DashInputPressed`.
    *   Ensure `_hasDoubleJumpedThisAirborneSequence` is reset on grounding.
3.  **Verify State Transitions:**
    *   Review `Update()` methods of `IdleState`, `MovingState`, `JumpingState`, `FallingState`, `DoubleJumpState`, `WallGrabState` to ensure all transitions to new ability states are correctly implemented (checking input flags and `PlayerAbilities.Can...()` methods).
4.  **`PlayerAbilities` Configuration:**
    *   Ensure the Player prefab has the `PlayerAbilities` component.
    *   Assign the relevant `AbilitySO` assets (`DoubleJumpSO`, `DashSO`, `WallJumpSO` if created) to its fields.
5.  **Test Scene Setup:**
    *   Use or create a test scene with varied geometry:
        *   Gaps requiring double jump or dash.
        *   Walls for wall grab/jump.
        *   Open areas for dashing.
6.  **Thorough Playtesting (with mocked `GameProgressionManager.IsAbilityUnlocked()`):**
    *   Initially, have `PlayerAbilities` assume all abilities are unlocked (or mock `GameProgressionManager` to say so).
    *   **Double Jump:** Test from ground, while falling, after regular jump. Test limits (only one double jump).
    *   **Wall Grab/Slide:** Test approaching walls from different angles, verify grab/slide. Test detaching.
    *   **Wall Jump:** Test jumping from wall grab. Test direction and force. Test if double jump resets.
    *   **Dash:** Test from ground and air. Test direction, speed, duration. Test cooldown.
    *   **Combinations:**
        *   Jump -> Wall Grab -> Wall Jump -> Double Jump -> Dash.
        *   Dash into wall -> Wall Grab.
        *   Other creative sequences.
    *   **Unlock Status Testing:**
        *   Modify `PlayerAbilities` or mock `GameProgressionManager` to make specific abilities "locked".
        *   Verify player cannot perform locked abilities.
        *   "Unlock" an ability and verify it becomes usable.
    *   **Input Robustness:** Test with rapid inputs, holding buttons, etc.
    *   **Physics Interactions:** Observe how abilities interact with `Rigidbody2D` physics (gravity, momentum).
    *   Check for console errors and warnings.

# Acceptance Criteria:
- All advanced player states are correctly integrated into `PlayerController`'s state machine.
- `PlayerAbilities` component correctly manages ability availability (cooldowns, mocked unlock status).
- Player can perform Double Jump, Wall Grab/Slide, Wall Jump, and Dash as per their individual state implementations.
- Abilities can be chained or used in sequence where logical.
- Player cannot use abilities that are "locked" (via mocked progression check).
- The overall player control scheme with advanced abilities feels responsive and intuitive (initial feel pass).

# Test Strategy:
- As detailed in "Thorough Playtesting" (Step 6).
- Utilize debugger to trace state transitions and `PlayerAbilities` checks.
- Use Animator window to observe animation state changes related to abilities.
- Invite others for a quick playtest if possible for feedback on controls and feel.

# Notes/Questions:
- This task is a major integration point and is critical for core gameplay feel.
- Placeholders for animations, VFX, and SFX for abilities are acceptable at this stage. Focus is on mechanical functionality.
- Tuning of ability parameters (forces, speeds, durations, cooldowns from SOs) will be an ongoing process.