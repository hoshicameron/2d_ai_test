# Task ID: 2.4.2
# Parent Task ID: 2.4
# Title: Implement Player IdleState
# Status: pending
# Dependencies: 2.4.1 # PlayerController structure and BaseState
# Priority: critical
# Estimated Effort: S
# Assignee: Unassigned

# Description:
Implement `IdleState.cs` for the player. This state will be active when the player is grounded and not providing movement input. It handles transitions to `MovingState` or `JumpingState`.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/Gameplay/Player/States/IdleState.cs`
2.  **Namespace:** `PetalsOfHope.Gameplay.Player.States`
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/Gameplay/Player/States/IdleState.cs
    namespace PetalsOfHope.Gameplay.Player.States
    {
        using UnityEngine;
        using PetalsOfHope.Core.StateMachine;

        public class IdleState : BaseState
        {
            private readonly PlayerController _player;
            private readonly string _idleAnimationName; // Or int hash

            // Animation parameter hashes (cache for performance)
            // Example: private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");

            public IdleState(PlayerController player, StateMachine stateMachine, string idleAnimationName) : base(stateMachine)
            {
                _player = player;
                _idleAnimationName = idleAnimationName;
            }

            public override void Enter()
            {
                // Debug.Log("Entering Idle State");
                _player.AnimationController.Play(_idleAnimationName); // Or use hash
                // _player.AnimationController.SetBool(IsMovingHash, false); // Example of setting anim params

                // Ensure velocity is minimal or zero when entering idle from movement
                if (_player.Rigidbody.velocity.x != 0)
                {
                     _player.Rigidbody.velocity = new Vector2(0, _player.Rigidbody.velocity.y);
                }
                _player.ResetJumpInputFlags();
            }

            public override void Exit()
            {
                // Debug.Log("Exiting Idle State");
            }

            public override void Update()
            {
                // Transition to JumpState if jump input is pressed and player is grounded
                if (_player.JumpInputPressed && _player.IsGrounded)
                {
                    stateMachine.ChangeState(_player.JumpingState);
                    return; // Exit early after state change
                }

                // Transition to MovingState if there's horizontal movement input
                if (Mathf.Abs(_player.MoveInput.x) > Mathf.Epsilon)
                {
                    stateMachine.ChangeState(_player.MovingState);
                    return; // Exit early
                }

                // Transition to FallingState if player is no longer grounded (e.g., walks off a ledge)
                if (!_player.IsGrounded)
                {
                    stateMachine.ChangeState(_player.FallingState);
                    return; // Exit early
                }
            }

            public override void FixedUpdate()
            {
                // Idle state typically doesn't do much in FixedUpdate for platformers
                // unless specific physics interactions are needed while idle.
            }
        }
    }
    ```

# Acceptance Criteria:
- `IdleState.cs` is created, inherits from `BaseState`, and is in the correct namespace.
- Constructor takes `PlayerController`, `StateMachine`, and an animation name/hash.
- `Enter()`:
    - Plays the idle animation via `_player.AnimationController`.
    - Optionally sets relevant Animator parameters (e.g., `IsMoving` to false).
    - Resets jump input flags.
    - Stops horizontal movement if any.
- `Update()`:
    - Transitions to `JumpingState` if jump input is detected and player is grounded.
    - Transitions to `MovingState` if horizontal move input is detected.
    - Transitions to `FallingState` if `_player.IsGrounded` becomes false.
- `Exit()` and `FixedUpdate()` are present (can be empty if no specific logic needed).
- Script compiles without errors.

# Test Strategy:
- Manual/Integration Testing:
    - With `PlayerController` set up, ensure it defaults to `IdleState`.
    - Verify idle animation plays.
    - Provide move input: verify transition to `MovingState`.
    - From idle, press jump: verify transition to `JumpingState`.
    - Manually move the player (in Scene view) off a ledge while in `IdleState`: verify transition to `FallingState`.
    - Observe console logs (if added) for state entry/exit.

# Notes/Questions:
- The `_idleAnimationName` string will be used with `_player.AnimationController.Play()`. It's assumed an Animator Controller with a state named "idle" (or whatever string is passed) exists on the Player GameObject. Using `Animator.StringToHash()` for these names in the state's constructor or as static readonly fields is a performance optimization.
- The `Mathf.Epsilon` check for `MoveInput.x` is to handle floating point inaccuracies.
- Resetting horizontal velocity in `Enter()` ensures the player stops immediately when entering idle.