# Task ID: 4.4.2
# Parent Task ID: 4.4
# Title: Implement DoubleJumpState
# Status: pending
# Dependencies: 2.4.1, 4.4.1 # PlayerController, PlayerAbilities
# Priority: high
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement `DoubleJumpState.cs` for the player. This state allows the player to perform a second jump while in the air, if the double jump ability is unlocked and available.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/Gameplay/Player/States/DoubleJumpState.cs`
2.  **Namespace:** `PetalsOfHope.Gameplay.Player.States`
3.  **PlayerController Modification:**
    *   Add `public DoubleJumpState DoubleJumpState { get; private set; }` to `PlayerController.cs`.
    *   Instantiate it in `PlayerController.Awake()`:
        `DoubleJumpState = new DoubleJumpState(this, StateMachine, "Player_DoubleJump_Anim");` (use actual animation name).
    *   Ensure `PlayerAbilities` component is accessible from `PlayerController` (e.g., `public PlayerAbilities Abilities { get; private set; }` and `Abilities = GetComponent<PlayerAbilities>();` in `Awake`).
4.  **Modify `JumpingState.cs` and `FallingState.cs`:**
    *   In their `Update()` methods, before other logic, check for double jump input:
        ```csharp
        // In JumpingState.Update() and FallingState.Update()
        if (_player.JumpInputPressed && _player.Abilities.CanDoubleJump() && !_player.HasDoubleJumpedThisAirborneSequence) // Add HasDoubleJumpedThisAirborneSequence to PlayerController
        {
            _player.SetHasDoubleJumpedThisAirborneSequence(true); // Mark double jump as used
            stateMachine.ChangeState(_player.DoubleJumpState);
            return;
        }
        ```
    *   `PlayerController` will need a new boolean flag `_hasDoubleJumpedThisAirborneSequence` and a public property `HasDoubleJumpedThisAirborneSequence`. This flag should be reset to `false` whenever the player becomes grounded (e.g., in `CheckIfGrounded()` when `!wasGrounded && IsGrounded`). And a setter method `SetHasDoubleJumpedThisAirborneSequence(bool value)`.

5.  **Implementation of `DoubleJumpState.cs`:**
    ```csharp
    // In Assets/_Project/Scripts/Gameplay/Player/States/DoubleJumpState.cs
    namespace PetalsOfHope.Gameplay.Player.States
    {
        using UnityEngine;
        using PetalsOfHope.Core.StateMachine;
        using PetalsOfHope.Data.Abilities.Types; // For DoubleJumpSO if specific data is needed

        public class DoubleJumpState : BaseState
        {
            private readonly PlayerController _player;
            private readonly string _doubleJumpAnimationName;
            private bool _jumpCutoffApplied = false;

            public DoubleJumpState(PlayerController player, StateMachine stateMachine, string animationName) : base(stateMachine)
            {
                _player = player;
                _doubleJumpAnimationName = animationName;
            }

            public override void Enter()
            {
                // Debug.Log("Entering Double Jump State");
                _player.AnimationController.Play(_doubleJumpAnimationName);

                float doubleJumpForce = _player.Stats.jumpForce; // Default to regular jump force
                var doubleJumpAbilityData = _player.Abilities.GetDoubleJumpAbilityData() as DoubleJumpSO;
                if (doubleJumpAbilityData != null)
                {
                    doubleJumpForce *= doubleJumpAbilityData.doubleJumpForceMultiplier; // Apply multiplier
                }
                
                _player.Rigidbody.velocity = new Vector2(_player.Rigidbody.velocity.x, 0f); // Zero out Y for consistent force
                _player.Rigidbody.AddForce(Vector2.up * doubleJumpForce, ForceMode2D.Impulse);
                
                _player.ResetJumpInputFlags(); // Consume jump input
                _jumpCutoffApplied = false; 
                // Note: PlayerController should have already marked that double jump was used for this air sequence.
            }

            public override void Exit() { }

            public override void Update()
            {
                if (_player.Rigidbody.velocity.y <= 0f)
                {
                    stateMachine.ChangeState(_player.FallingState);
                    return;
                }

                if (_player.JumpInputReleased && !_jumpCutoffApplied)
                {
                    if (_player.Rigidbody.velocity.y > 0)
                    {
                        _player.Rigidbody.velocity = new Vector2(_player.Rigidbody.velocity.x, _player.Rigidbody.velocity.y * 0.5f);
                    }
                    _jumpCutoffApplied = true;
                    _player.ResetJumpInputFlags();
                }
                FlipSprite();
            }

            public override void FixedUpdate()
            {
                float airControlFactor = _player.Stats.airControlFactor ?? 1.0f;
                float targetVelocityX = _player.MoveInput.x * _player.Stats.movementSpeed * airControlFactor;
                _player.Rigidbody.velocity = new Vector2(targetVelocityX, _player.Rigidbody.velocity.y);
            }
            
            private void FlipSprite()
            {
                // Same flip logic as in JumpingState/FallingState
                if (Mathf.Abs(_player.MoveInput.x) > 0.01f)
                {
                    float currentScaleX = _player.transform.localScale.x;
                    if (_player.MoveInput.x * currentScaleX < 0) // If input direction and facing direction are opposite
                        _player.transform.localScale = new Vector3(-currentScaleX, _player.transform.localScale.y, _player.transform.localScale.z);
                }
            }
        }
    }
    ```

# Acceptance Criteria:
- `DoubleJumpState.cs` is implemented.
- `PlayerController` instantiates `DoubleJumpState` and manages a flag `_hasDoubleJumpedThisAirborneSequence` (reset on grounding).
- `JumpingState` and `FallingState` can transition to `DoubleJumpState` if player presses jump, `PlayerAbilities.CanDoubleJump()` is true, and double jump hasn't been used yet in the current air sequence.
- `DoubleJumpState.Enter()` applies an upward force (potentially modified by `DoubleJumpSO.doubleJumpForceMultiplier`).
- `DoubleJumpState` behaves similarly to `JumpingState` regarding air control, variable jump height (if applicable to double jump), and transition to `FallingState`.
- Player can perform a double jump if the ability is "unlocked" (mocked via `PlayerAbilities` for now).

# Test Strategy:
- Manual Testing:
    - In a test level, mock `PlayerAbilities.CanDoubleJump()` to return true.
    - Perform a regular jump. While in air (either in `JumpingState` or `FallingState`), press jump again.
    - Verify player transitions to `DoubleJumpState`, performs another jump, and associated animation plays.
    - Verify player cannot perform a third jump.
    - Verify after landing, the double jump is available again for the next air sequence.
    - Test variable height for double jump if implemented.

# Notes/Questions:
- `PlayerController` needs a new boolean field like `private bool _hasDoubleJumpedThisAirborneSequence;` and methods to manage it: `public bool HasDoubleJumpedThisAirborneSequence => _hasDoubleJumpedThisAirborneSequence; public void SetHasDoubleJumpedThisAirborneSequence(bool value) { _hasDoubleJumpedThisAirborneSequence = value; }`. This flag should be reset to `false` in `PlayerController.CheckIfGrounded()` when the player lands.
- The `DoubleJumpSO` (from Task 1.3.2) should be assigned to `PlayerAbilities._doubleJumpAbilityData` to get parameters like `doubleJumpForceMultiplier`.
- The flip sprite logic can be refactored into a utility method in `PlayerController` if it's identical across multiple states.