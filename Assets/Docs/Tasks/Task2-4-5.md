# Task ID: 2.4.5
# Parent Task ID: 2.4
# Title: Implement Player FallingState
# Status: pending
# Dependencies: 2.4.1, 1.2.8 # PlayerController structure, BaseState, PlayerLandedEventSO
# Priority: critical
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement `FallingState.cs` for the player. This state is active when the player is in the air and moving downwards (or has walked off a ledge). It manages gravity's effect, controls fall animation, and handles transitions to `IdleState` or `MovingState` upon landing.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/Gameplay/Player/States/FallingState.cs`
2.  **Namespace:** `PetalsOfHope.Gameplay.Player.States`
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/Gameplay/Player/States/FallingState.cs
    namespace PetalsOfHope.Gameplay.Player.States
    {
        using UnityEngine;
        using PetalsOfHope.Core.StateMachine;

        public class FallingState : BaseState
        {
            private readonly PlayerController _player;
            private readonly string _fallAnimationName;

            public FallingState(PlayerController player, StateMachine stateMachine, string fallAnimationName) : base(stateMachine)
            {
                _player = player;
                _fallAnimationName = fallAnimationName;
            }

            public override void Enter()
            {
                // Debug.Log("Entering Falling State");
                _player.AnimationController.Play(_fallAnimationName);
                // _player.AnimationController.SetBool("IsGrounded", false);
                // _player.AnimationController.SetBool("IsJumping", false); // Ensure jump anim bool is off if used

                _player.ResetJumpInputFlags(); // Cannot jump while already falling (unless double jump is implemented as a separate state/logic)
            }

            public override void Exit()
            {
                // Debug.Log("Exiting Falling State");
            }

            public override void Update()
            {
                // Transition to Idle/Moving when grounded
                if (_player.IsGrounded)
                {
                    // PlayerLandedEventSO is raised by PlayerController.CheckIfGrounded()
                    // This state simply checks IsGrounded flag which is updated by PlayerController.
                    if (Mathf.Abs(_player.MoveInput.x) > Mathf.Epsilon)
                    {
                        stateMachine.ChangeState(_player.MovingState);
                    }
                    else
                    {
                        stateMachine.ChangeState(_player.IdleState);
                    }
                    return;
                }
                
                // If player has double jump ability and presses jump:
                // if (_player.CanDoubleJump() && _player.JumpInputPressed) {
                //    stateMachine.ChangeState(_player.DoubleJumpState); // DoubleJumpState would be a new state
                //    return;
                // }

                // Handle sprite flipping based on MoveInput.x (if player can control air strafe direction)
                FlipSprite();
            }

            public override void FixedUpdate()
            {
                // Horizontal air control (strafe)
                // Similar to JumpingState's horizontal movement.
                // Could also apply increased gravity multiplier here if desired for faster falling.
                float airControlFactor = _player.Stats.airControlFactor ?? 1.0f; // Get from PlayerStatsSO
                float targetVelocityX = _player.MoveInput.x * _player.Stats.movementSpeed * airControlFactor;
                
                _player.Rigidbody.velocity = new Vector2(targetVelocityX, _player.Rigidbody.velocity.y);

                // Optional: Apply extra gravity for faster fall / game feel
                // if (_player.Rigidbody.velocity.y < _player.Stats.maxFallSpeed) // Add maxFallSpeed to PlayerStatsSO
                // {
                //    _player.Rigidbody.AddForce(Vector2.down * _player.Stats.fallGravityMultiplier, ForceMode2D.Force);
                // }
            }

            private void FlipSprite()
            {
                 if (Mathf.Abs(_player.MoveInput.x) > 0.01f) 
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
- `FallingState.cs` is created, inherits `BaseState`, and is in the correct namespace.
- Constructor takes `PlayerController`, `StateMachine`, and an animation name.
- `Enter()`:
    - Plays the fall animation.
    - Resets jump input flags.
- `Update()`:
    - Transitions to `MovingState` if `_player.IsGrounded` is true and there's move input.
    - Transitions to `IdleState` if `_player.IsGrounded` is true and no move input.
    - Handles sprite flipping for air strafing.
- `FixedUpdate()`:
    - Applies horizontal air control.
    - (Optional) Applies additional gravity or caps fall speed if `PlayerStatsSO` defines such properties.
- `Exit()` is present.
- Script compiles without errors.

# Test Strategy:
- Manual/Integration Testing:
    - From `JumpingState`, observe transition to `FallingState` as player descends.
    - Walk player off a ledge: verify transition to `FallingState`.
    - Verify fall animation plays.
    - When player lands (hits ground layer):
        - Verify transition to `IdleState` if no input.
        - Verify transition to `MovingState` if holding move input during landing.
    - Test air control and sprite flipping while falling.
    - Test that `PlayerLandedEventSO` is raised by `PlayerController` when landing from this state.

# Notes/Questions:
- The transition from `FallingState` to ground states relies on `_player.IsGrounded` being updated by `PlayerController.CheckIfGrounded()`. The `PlayerLandedEventSO` raised by `PlayerController` can be used for sound/VFX but isn't strictly necessary for the state transition itself if `IsGrounded` is polled.
- Placeholder for double jump logic is commented out; this would involve a new `DoubleJumpState` and checks in `PlayerController` or `PlayerAbilities` component (Phase 3.4.4).
- `maxFallSpeed` and `fallGravityMultiplier` are common platformer mechanics for better game feel, can be added to `PlayerStatsSO`.