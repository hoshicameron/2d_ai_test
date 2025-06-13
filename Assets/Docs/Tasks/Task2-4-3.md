# Task ID: 2.4.3
# Parent Task ID: 2.4
# Title: Implement Player MovingState
# Status: completed
# Dependencies: 2.4.1 # PlayerController structure and BaseState
# Priority: critical
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement `MovingState.cs` for the player. This state is active when the player is grounded and providing movement input. It handles horizontal movement, transitions to other states (Idle, Jumping, Falling), and controls walk/run animations.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/Gameplay/Player/States/MovingState.cs`
2.  **Namespace:** `PetalsOfHope.Gameplay.Player.States`
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/Gameplay/Player/States/MovingState.cs
    namespace PetalsOfHope.Gameplay.Player.States
    {
        using UnityEngine;
        using PetalsOfHope.Core.StateMachine;

        public class MovingState : BaseState
        {
            private readonly PlayerController _player;
            private readonly string _moveAnimationName; // e.g., "run" or "walk"

            // Animation parameter hashes (cache for performance)
            // private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
            // private static readonly int HorizontalSpeedHash = Animator.StringToHash("HorizontalSpeed");

            public MovingState(PlayerController player, StateMachine stateMachine, string moveAnimationName) : base(stateMachine)
            {
                _player = player;
                _moveAnimationName = moveAnimationName;
            }

            public override void Enter()
            {
                // Debug.Log("Entering Moving State");
                _player.AnimationController.Play(_moveAnimationName);
                // _player.AnimationController.SetBool(IsMovingHash, true);
                _player.ResetJumpInputFlags();
            }

            public override void Exit()
            {
                // Debug.Log("Exiting Moving State");
                // Optionally stop movement if exiting to a state that shouldn't have residual velocity
                // _player.Rigidbody.velocity = new Vector2(0, _player.Rigidbody.velocity.y);
                // _player.AnimationController.SetBool(IsMovingHash, false);
            }

            public override void Update()
            {
                // Transition to JumpState if jump input is pressed and player is grounded
                if (_player.JumpInputPressed && _player.IsGrounded)
                {
                    stateMachine.ChangeState(_player.JumpingState);
                    return;
                }

                // Transition to IdleState if no horizontal movement input
                if (Mathf.Abs(_player.MoveInput.x) < Mathf.Epsilon)
                {
                    stateMachine.ChangeState(_player.IdleState);
                    return;
                }

                // Transition to FallingState if player is no longer grounded
                if (!_player.IsGrounded)
                {
                    stateMachine.ChangeState(_player.FallingState);
                    return;
                }

                // Update animation parameters if needed (e.g., speed for blend tree)
                // _player.AnimationController.SetFloat(HorizontalSpeedHash, Mathf.Abs(_player.MoveInput.x));

                // Handle sprite flipping based on MoveInput.x
                FlipSprite();
            }

            public override void FixedUpdate()
            {
                // Apply horizontal movement
                float targetVelocityX = _player.MoveInput.x * _player.Stats.movementSpeed;
                _player.Rigidbody.velocity = new Vector2(targetVelocityX, _player.Rigidbody.velocity.y);
            }

            private void FlipSprite()
            {
                // Assuming player has a child SpriteRenderer or similar visual representation
                // This flips the entire PlayerController's transform scale.
                // Adjust if sprite is on a child and only child needs flipping.
                if (_player.MoveInput.x > 0.01f && _player.transform.localScale.x < 0f) // Moving right, facing left
                {
                    _player.transform.localScale = new Vector3(Mathf.Abs(_player.transform.localScale.x), _player.transform.localScale.y, _player.transform.localScale.z);
                }
                else if (_player.MoveInput.x < -0.01f && _player.transform.localScale.x > 0f) // Moving left, facing right
                {
                    _player.transform.localScale = new Vector3(-Mathf.Abs(_player.transform.localScale.x), _player.transform.localScale.y, _player.transform.localScale.z);
                }
            }
        }
    }
    ```

# Acceptance Criteria:
- `MovingState.cs` is created, inherits `BaseState`, and is in the correct namespace.
- Constructor takes `PlayerController`, `StateMachine`, and an animation name.
- `Enter()`:
    - Plays the move animation (walk/run).
    - Resets jump input flags.
- `Update()`:
    - Transitions to `JumpingState` on jump input if grounded.
    - Transitions to `IdleState` if no horizontal input.
    - Transitions to `FallingState` if not grounded.
    - Calls `FlipSprite()` method.
- `FixedUpdate()`:
    - Applies horizontal velocity to `_player.Rigidbody` based on `_player.MoveInput.x` and `_player.Stats.movementSpeed`.
- `Exit()` is present.
- `FlipSprite()` method correctly adjusts player's `transform.localScale.x` to face movement direction.
- Script compiles without errors.

# Test Strategy:
- Manual/Integration Testing:
    - In `PlayerController` test setup, provide horizontal input. Verify transition from `IdleState` to `MovingState`.
    - Verify move animation plays and player character moves horizontally.
    - Verify player speed matches `PlayerStatsSO.movementSpeed` (approx).
    - Test sprite flipping: move left, player faces left; move right, player faces right.
    - Stop input: verify transition back to `IdleState`.
    - While moving, press jump: verify transition to `JumpingState`.
    - While moving, walk off a ledge: verify transition to `FallingState`.

# Notes/Questions:
- `FlipSprite()` logic assumes the player's visual representation is affected by `_player.transform.localScale`. If the sprite is on a child object, that child's scale should be manipulated instead, or the sprite itself flipped.
- The animation parameters (`IsMovingHash`, `HorizontalSpeedHash`) are commented out as examples; actual parameters depend on the Animator Controller setup.
- Exiting `MovingState` might not always require stopping velocity if transitioning to e.g. `JumpingState` where X velocity should be preserved. This can be refined.