# Task ID: 4.4.3
# Parent Task ID: 4.4
# Title: Implement WallGrabState (Wall Slide / Wall Hang)
# Status: completed
# Dependencies: 2.4.1, 4.4.1 # PlayerController, PlayerAbilities
# Priority: high
# Estimated Effort: L
# Assignee: Unassigned

# Description:
Implement `WallGrabState.cs` (or WallSlideState) for the player. This state allows the player to grab onto or slide down a wall if the ability is unlocked and conditions are met (e.g., player is airborne, moving towards a wall, and holding an input).

# Details:
1.  **`PlayerController` Modifications:**
    *   Add `public WallGrabState WallGrabState { get; private set; }`
    *   Instantiate in `Awake()`: `WallGrabState = new WallGrabState(this, StateMachine, "Player_WallSlide_Anim");`
    *   Add wall detection logic to `PlayerController.Update()` or `FixedUpdate()`:
        ```csharp
        // In PlayerController.cs
        [Header("Wall Check")]
        [SerializeField] private Transform _wallCheckPoint; // Positioned slightly in front of player
        [SerializeField] private float _wallCheckDistance = 0.5f;
        [SerializeField] private LayerMask _wallLayer; // Layer for walls
        public bool IsTouchingWall { get; private set; }
        public float WallDirectionX { get; private set; } // -1 for left wall, 1 for right wall

        // In PlayerController.Update() or FixedUpdate()
        // private void CheckWallCollision() // Call this from Update()
        // {
        //     float facingDirection = Mathf.Sign(transform.localScale.x); // Assumes scale.x indicates facing
        //     Vector2 raycastOrigin = _wallCheckPoint != null ? (Vector2)_wallCheckPoint.position : (Vector2)transform.position;
        //     RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.right * facingDirection, _wallCheckDistance, _wallLayer);
        //     
        //     IsTouchingWall = hit.collider != null;
        //     if (IsTouchingWall) WallDirectionX = facingDirection; else WallDirectionX = 0;
        //     
        //     // Gizmo for wall check
        //     // Debug.DrawRay(raycastOrigin, Vector2.right * facingDirection * _wallCheckDistance, IsTouchingWall ? Color.green : Color.red);
        // }
        ```
        (Gizmo drawing for `_wallCheckPoint` can be added to `OnDrawGizmosSelected`)
    *   Player's `InputReader` needs to be checked for a "grab" input if wall grab is not automatic (e.g., hold Shift or a direction towards the wall). For now, assume moving towards wall is enough.

2.  **Modify Airborne States (`JumpingState`, `FallingState`, `DoubleJumpState`):**
    *   In their `Update()` methods, check for wall grab conditions:
        ```csharp
        // In JumpingState.Update(), FallingState.Update(), DoubleJumpState.Update()
        // if (_player.IsTouchingWall && _player.Abilities.CanWallGrab() && _player.MoveInput.x * _player.WallDirectionX > 0) // Moving towards the wall
        // Or, if holding a grab button: ... && _player.GrabInputHeld ...
        // {
        //     stateMachine.ChangeState(_player.WallGrabState);
        //     return;
        // }
        ```
    *   The condition `_player.MoveInput.x * _player.WallDirectionX > 0` checks if horizontal input is towards the detected wall.

3.  **Implementation of `WallGrabState.cs`:**
    ```csharp
    // In Assets/_Project/Scripts/Gameplay/Player/States/WallGrabState.cs
    namespace PetalsOfHope.Gameplay.Player.States
    {
        using UnityEngine;
        using PetalsOfHope.Core.StateMachine;

        public class WallGrabState : BaseState
        {
            private readonly PlayerController _player;
            private readonly string _wallGrabAnimationName;
            private float _wallSlideSpeed; // Get from PlayerStatsSO or define here

            public WallGrabState(PlayerController player, StateMachine stateMachine, string animationName) : base(stateMachine)
            {
                _player = player;
                _wallGrabAnimationName = animationName;
                _wallSlideSpeed = _player.Stats.wallSlideSpeed ?? 2f; // Add wallSlideSpeed to PlayerStatsSO
            }

            public override void Enter()
            {
                // Debug.Log("Entering Wall Grab State");
                _player.AnimationController.Play(_wallGrabAnimationName);
                _player.ResetJumpInputFlags();
                _player.SetHasDoubleJumpedThisAirborneSequence(false); // Wall interaction usually resets air jump
                
                // Ensure player faces the wall
                if (_player.WallDirectionX * _player.transform.localScale.x < 0) {
                     _player.transform.localScale = new Vector3(-_player.transform.localScale.x, _player.transform.localScale.y, _player.transform.localScale.z);
                }
            }

            public override void Exit() { }

            public override void Update()
            {
                // Transition to WallJumpState
                if (_player.JumpInputPressed && _player.Abilities.CanWallJump())
                {
                    stateMachine.ChangeState(_player.WallJumpState);
                    return;
                }

                // Detach from wall: if no longer holding towards wall OR no longer touching wall
                bool holdingTowardsWall = (_player.MoveInput.x * _player.WallDirectionX) > 0.01f; // Or specific grab input
                if (!holdingTowardsWall || !_player.IsTouchingWall)
                {
                    stateMachine.ChangeState(_player.FallingState);
                    return;
                }

                // Detach if grounded
                if (_player.IsGrounded)
                {
                    stateMachine.ChangeState(_player.IdleState); // Or MovingState based on input
                    return;
                }
            }

            public override void FixedUpdate()
            {
                // Apply slow slide down the wall
                // Player might also be able to slowly climb up if holding up input.
                float verticalInput = _player.MoveInput.y; // Check vertical input for climb/faster slide
                float currentSlideSpeed = _wallSlideSpeed;

                if (verticalInput < -0.1f) // Holding down, slide faster
                {
                    currentSlideSpeed *= 2f; // Example: double slide speed
                }
                else if (verticalInput > 0.1f && _player.Abilities.CanWallClimb()) // Holding up, climb (if CanWallClimb exists)
                {
                    // currentSlideSpeed = -_player.Stats.wallClimbSpeed; // Negative to move up
                }
                
                _player.Rigidbody.velocity = new Vector2(0, -currentSlideSpeed);
            }
        }
    }
    ```

# Acceptance Criteria:
- `PlayerController` has wall detection logic (`IsTouchingWall`, `WallDirectionX`).
- Airborne states (`JumpingState`, `FallingState`, `DoubleJumpState`) can transition to `WallGrabState` if player is moving towards a wall, `PlayerAbilities.CanWallGrab()` is true.
- `WallGrabState.cs` is implemented.
- `Enter()`: Plays wall grab/slide animation, resets double jump. Ensures player faces the wall.
- `Update()`:
    - Transitions to `WallJumpState` on jump input if `CanWallJump()` is true.
    - Transitions to `FallingState` if player stops holding towards wall (or releases grab button) or is no longer touching wall.
    - Transitions to `IdleState` (or `MovingState`) if player becomes grounded.
- `FixedUpdate()` applies a slow downward slide velocity. Optionally allows faster slide or slow climb with vertical input.
- Player can grab/slide on walls if ability is "unlocked".

# Test Strategy:
- Manual Testing:
    - Mock `PlayerAbilities.CanWallGrab()` to return true.
    - In a test level with walls (on `_wallLayer`), jump towards a wall while holding direction into it.
    - Verify player enters `WallGrabState`, plays animation, and slides down slowly.
    - Test detaching from wall by releasing directional input or moving away.
    - Test jumping from wall (will trigger `WallJumpState`).
    - Test landing on ground while wall sliding.
    - Verify `PlayerStatsSO` needs `wallSlideSpeed` (and `wallClimbSpeed` if climbing is added).

# Notes/Questions:
- `PlayerStatsSO` needs a new field: `public float wallSlideSpeed = 2f;`.
- Wall climbing is an optional extension; for now, just sliding.
- The condition for entering `WallGrabState` (just holding towards wall vs. a dedicated grab button) needs to be decided. The example uses holding towards wall.
- Wall detection in `PlayerController` needs a `_wallCheckPoint` (Transform child of Player, offset to front) and `_wallCheckDistance`. A Gizmo for `_wallCheckPoint` should be added to `PlayerController.OnDrawGizmosSelected`.
- Resetting `_hasDoubleJumpedThisAirborneSequence` in `WallGrabState.Enter()` is a common design choice, allowing a fresh double jump after a wall interaction.