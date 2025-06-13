# Task ID: 2.4.4
# Parent Task ID: 2.4
# Title: Implement Player JumpingState
# Status: completed
# Dependencies: 2.4.1 # PlayerController structure and BaseState
# Priority: critical
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement `JumpingState.cs` for the player. This state is active when the player initiates a jump. It applies upward force, controls jump animation, and handles transitions to `FallingState`. It also considers variable jump height based on jump button release.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/Gameplay/Player/States/JumpingState.cs`
2.  **Namespace:** `PetalsOfHope.Gameplay.Player.States`
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/Gameplay/Player/States/JumpingState.cs
    namespace PetalsOfHope.Gameplay.Player.States
    {
        using UnityEngine;
        using PetalsOfHope.Core.StateMachine;

        public class JumpingState : BaseState
        {
            private readonly PlayerController _player;
            private readonly string _jumpAnimationName;
            private bool _jumpCutoffApplied = false; // To ensure jump cutoff force reduction happens only once

            public JumpingState(PlayerController player, StateMachine stateMachine, string jumpAnimationName) : base(stateMachine)
            {
                _player = player;
                _jumpAnimationName = jumpAnimationName;
            }

            public override void Enter()
            {
                // Debug.Log("Entering Jumping State");
                _player.AnimationController.Play(_jumpAnimationName);
                // _player.AnimationController.SetBool("IsJumping", true); // Example
                // _player.AnimationController.SetBool("IsGrounded", false);

                // Apply jump force. We want to override existing y velocity for a consistent jump.
                _player.Rigidbody.velocity = new Vector2(_player.Rigidbody.velocity.x, 0f); // Zero out vertical velocity before jump
                _player.Rigidbody.AddForce(Vector2.up * _player.Stats.jumpForce, ForceMode2D.Impulse);
                
                _player.ResetJumpInputFlags(); // Consume the jump pressed flag
                _jumpCutoffApplied = false;
            }

            public override void Exit()
            {
                // Debug.Log("Exiting Jumping State");
                // _player.AnimationController.SetBool("IsJumping", false);
            }

            public override void Update()
            {
                // Transition to FallingState if vertical velocity becomes non-positive (i.e., player starts to fall)
                if (_player.Rigidbody.velocity.y <= 0f)
                {
                    stateMachine.ChangeState(_player.FallingState);
                    return;
                }

                // Handle variable jump height: if jump button released early, reduce upward velocity
                if (_player.JumpInputReleased && !_jumpCutoffApplied)
                {
                    // Reduce upward velocity for shorter jump. Factor can be tuned.
                    // Example: Halve the current upward velocity if it's positive.
                    if (_player.Rigidbody.velocity.y > 0)
                    {
                        _player.Rigidbody.velocity = new Vector2(_player.Rigidbody.velocity.x, _player.Rigidbody.velocity.y * 0.5f);
                    }
                    _jumpCutoffApplied = true; // Ensure this only happens once per jump
                    _player.ResetJumpInputFlags(); // Consume the jump released flag
                }

                // Handle sprite flipping based on MoveInput.x (if player can control air strafe direction)
                FlipSprite();
            }

            public override void FixedUpdate()
            {
                // Horizontal air control (strafe)
                // Similar to MovingState's horizontal movement but potentially with different speed/acceleration (air control factor)
                float airControlFactor = _player.Stats.airControlFactor ?? 1.0f; // Add airControlFactor to PlayerStatsSO if needed, default to 1
                float targetVelocityX = _player.MoveInput.x * _player.Stats.movementSpeed * airControlFactor;
                
                // Preserve current Y velocity, only change X
                _player.Rigidbody.velocity = new Vector2(targetVelocityX, _player.Rigidbody.velocity.y);
            }
            
            private void FlipSprite()
            {
                if (Mathf.Abs(_player.MoveInput.x) > 0.01f) // Only flip if there's significant horizontal input
                {
                    if (_player.MoveInput.x > 0.01f && _player.transform.localScale.x < 0f)
                    {
                        _player.transform.localScale = new Vector3(Mathf.Abs(_player.transform.localScale.x), _player.transform.localScale.y, _player.transform.localScale.z);
                    }
                    else if (_player.MoveInput.x < -0.01f && _player.transform.localScale.x > 0f)
                    {
                        _player.transform.localScale = new Vector3(-Mathf.Abs(_player.transform.localScale.x), _player.transform.localScale.y, _player.transform.localScale.z);
                    }
                }
            }
        }
    }
    ```

# Acceptance Criteria:
- `JumpingState.cs` is created, inherits `BaseState`, and is in the correct namespace.
- Constructor takes `PlayerController`, `StateMachine`, and an animation name.
- `Enter()`:
    - Plays the jump animation.
    - Applies an upward force to `_player.Rigidbody` using `_player.Stats.jumpForce`.
    - Resets jump input flags.
    - Initializes `_jumpCutoffApplied` to false.
- `Update()`:
    - Transitions to `FallingState` when `_player.Rigidbody.velocity.y <= 0`.
    - If `_player.JumpInputReleased` is true and `_jumpCutoffApplied` is false, reduces upward velocity (e.g., multiplies by 0.5f) and sets `_jumpCutoffApplied` to true.
    - Handles sprite flipping for air strafing.
- `FixedUpdate()`:
    - Applies horizontal air control based on `_player.MoveInput.x` and `_player.Stats.movementSpeed` (potentially modified by an air control factor from `PlayerStatsSO`).
- `Exit()` is present.
- Script compiles without errors.

# Test Strategy:
- Manual/Integration Testing:
    - From `IdleState` or `MovingState`, press jump: verify transition to `JumpingState`.
    - Verify jump animation plays and player moves upwards.
    - Verify jump height feels appropriate based on `jumpForce`.
    - Test variable jump height: short tap of jump key vs. holding it. Player should jump higher when held.
    - Verify transition to `FallingState` at the apex of the jump or when velocity.y becomes <= 0.
    - Test air control: try to move left/right while in air.
    - Test sprite flipping during air control.

# Notes/Questions:
- Variable jump height is implemented by checking `_player.JumpInputReleased`. The `PlayerController` should set this flag when the jump input is cancelled (key/button released).
- `airControlFactor` is a common parameter for platformers; it should be added to `PlayerStatsSO` if fine-grained control over air movement is desired. Added a nullable `airControlFactor` to `PlayerStatsSO` in thought, default to 1.0f.
- Zeroing out Y velocity before applying jump force (`_player.Rigidbody.velocity = new Vector2(_player.Rigidbody.velocity.x, 0f);`) ensures consistent jump height regardless of previous vertical speed (e.g., if jumping immediately after landing).
- Flipping sprite during jump/fall is common if the player can change horizontal direction mid-air.