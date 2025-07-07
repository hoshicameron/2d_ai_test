# Task ID: 2.4.6
# Parent Task ID: 2.4
# Title: Integrate Player States with PlayerController and Test Basic Loop
# Status: pending
# Dependencies: 2.4.1, 2.4.2, 2.4.3, 2.4.4, 2.4.5 # PlayerController and all implemented states
# Priority: critical
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Ensure all implemented player states (`IdleState`, `MovingState`, `JumpingState`, `FallingState`) are correctly instantiated and linked within `PlayerController.cs`. Perform thorough testing of the basic player movement and state transition loop.

# Details:
1.  **Verify `PlayerController.Awake()`:**
    *   Ensure state instances are correctly created:
        ```csharp
        // In PlayerController.Awake()
        IdleState = new IdleState(this, StateMachine, "Player_Idle_Anim"); // Use actual animation names/hashes
        MovingState = new MovingState(this, StateMachine, "Player_Run_Anim");
        JumpingState = new JumpingState(this, StateMachine, "Player_Jump_Anim");
        FallingState = new FallingState(this, StateMachine, "Player_Fall_Anim");
        ```
    *   Replace `"Player_Idle_Anim"`, etc., with the actual names of animation states defined in the Player's Animator Controller, or use `Animator.StringToHash()` for these.
2.  **Verify `PlayerController.Start()`:**
    *   Ensure `StateMachine.Initialize(IdleState);` is called.
3.  **Create Player Prefab / Setup Scene Object:**
    *   Create a Player prefab or configure a player GameObject in a test scene.
    *   **Components:**
        *   `SpriteRenderer` (or child object with sprite).
        *   `Rigidbody2D`: Set `Body Type` to `Dynamic`, adjust `Mass`, `Linear Drag`, `Gravity Scale` (e.g., 3-5 is common for snappy platformers if not using custom gravity logic per state). Set `Collision Detection` to `Continuous` if fast movement occurs. Freeze Z rotation.
        *   `CapsuleCollider2D` (or `BoxCollider2D`): Adjust size/offset to fit sprite.
        *   `Animator`: Create a basic Animator Controller asset (e.g., `PlayerAnimatorController`) with states: `Player_Idle_Anim`, `Player_Run_Anim`, `Player_Jump_Anim`, `Player_Fall_Anim`. Create parameters (e.g., `IsMoving` (bool), `IsJumping` (bool), `IsGrounded` (bool), `YSpeed` (float)) and set up transitions between these animation states.
        *   `PetalsOfHope.Core.Animation.AnimationController` (our script).
        *   `PetalsOfHope.Core.StateMachine.StateMachine` (our script).
        *   `PetalsOfHope.Gameplay.Player.PlayerController` (our script).
    *   **`PlayerController` Configuration:**
        *   Assign `PlayerStatsSO`.
        *   Assign `InputReader` SO.
        *   Assign `GroundCheckPoint` (an empty child GameObject positioned at player's feet).
        *   Set `GroundCheckRadius`.
        *   Set `GroundLayer` (create a "Ground" layer and assign it to platform GameObjects).
        *   Assign `PlayerLandedEventSO`.
4.  **Test Scene:**
    *   Create a test scene with platforms (on "Ground" layer) for walking, jumping, and falling.
5.  **Thorough Playtesting:**
    *   **Idle:** Player starts idle, idle animation plays.
    *   **Move:** Input moves player, run animation plays, speed matches stats, sprite flips.
    *   **Stop Moving:** Player returns to idle, idle animation.
    *   **Jump from Idle:** Player jumps, jump animation, lands, returns to idle.
    *   **Jump from Moving:** Player jumps while moving, jump animation, preserves horizontal momentum, lands, returns to moving (if input held) or idle.
    *   **Variable Jump:** Tap jump vs. hold jump results in different jump heights.
    *   **Fall off Ledge:** Player walks off ledge, fall animation, lands, returns to appropriate ground state.
    *   **Air Control:** Player can slightly adjust horizontal position mid-air.
    *   **Ground Check Reliability:** Test on slopes, moving platforms (if any yet), edges of platforms.
    *   **State Transitions:** Use debugger and logs to ensure states are changing as expected and no state gets "stuck".
    *   Check for console errors or warnings.

# Acceptance Criteria:
- Player states are correctly instantiated and initialized in `PlayerController`.
- Player prefab/GameObject is correctly configured with all necessary components and settings.
- Player character can perform all basic actions: idle, move, jump (variable height), fall, land.
- Animations corresponding to these actions are played correctly.
- State transitions are smooth and logical.
- Player movement and actions feel responsive.
- No significant bugs or errors in the core player loop.

# Test Strategy:
- As detailed in "Thorough Playtesting" (Step 5).
- Use Unity's Profiler to check for any performance issues if movement feels laggy (unlikely at this stage but good to keep in mind).
- Use the Animator window to observe animation state transitions live.
- Use the Scene view to observe Rigidbody velocities and Gizmos.

# Notes/Questions:
- This task is an integration and testing task, pulling together all previous PlayerController and State subtasks.
- The actual animation names/hashes used in state constructors must match those in the Player's Animator Controller.
- Rigidbody2D `Gravity Scale` is a key tuning parameter for platformer feel.
- Fine-tuning of `PlayerStatsSO` values (speed, jumpforce, etc.) will be an ongoing process.
- Add dummy animation clips to the Animator Controller if real animations are not yet available, just to see state changes.