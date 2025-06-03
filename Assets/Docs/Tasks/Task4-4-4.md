# Task ID: 4.4.4
# Parent Task ID: 4.4
# Title: Implement WallJumpState
# Status: pending
# Dependencies: 4.4.3 # WallGrabState (or other states that can initiate wall jump)
# Priority: high
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement `WallJumpState.cs` for the player. This state allows the player to jump off a wall they are grabbing or sliding on, typically propelling them upwards and away from the wall.

# Details:
1.  **`PlayerController` Modifications:**
    *   Add `public WallJumpState WallJumpState { get; private set; }`
    *   Instantiate in `Awake()`: `WallJumpState = new WallJumpState(this, StateMachine, "Player_WallJump_Anim");`
2.  **Modify `WallGrabState.cs` (and potentially other states like `FallingState` if wall jump can occur without explicit grab):**
    *   Ensure `WallGrabState.Update()` transitions to `_player.WallJumpState` on jump input if `_player.Abilities.CanWallJump()` is true. (Already included in 4.4.3)
3.  **Implementation of `WallJumpState.cs`:**
    ```csharp
    // In Assets/_Project/Scripts/Gameplay/Player/States/WallJumpState.cs
    namespace PetalsOfHope.Gameplay.Player.States
    {
        using UnityEngine;
        using PetalsOfHope.Core.StateMachine;
        using PetalsOfHope.Data.Abilities.Types; // For WallJumpSO if it holds specific data like force vector

        public class WallJumpState : BaseState
        {
            private readonly PlayerController _player;
            private readonly string _wallJumpAnimationName;
            
            private Vector2 _wallJumpDirection; // Calculated on Enter
            private float _wallJumpForce;      // From AbilitySO or PlayerStatsSO
            private float _wallJumpDuration;   // Duration for which player input might be locked or influenced

            public WallJumpState(PlayerController player, StateMachine stateMachine, string animationName) : base(stateMachine)
            {
                _player = player;
                _wallJumpAnimationName = animationName;

                // Get wall jump parameters from AbilitySO or PlayerStatsSO
                var wallJumpAbilityData = _player.Abilities.GetWallJumpAbilityData(); // Assumes method in PlayerAbilities
                if (wallJumpAbilityData != null && wallJumpAbilityData is WallJumpSO specificData) {
                    // _wallJumpDirection = specificData.wallJumpDirection; // If defined in SO
                    _wallJumpForce = specificData.wallJumpForce;
                    _wallJumpDuration = specificData.wallJumpCtrlLockDuration;
                } else {
                    // Fallback values or get from PlayerStatsSO
                    _wallJumpForce = _player.Stats.wallJumpForce ?? 12f; // Add wallJumpForce to PlayerStatsSO
                    _wallJumpDuration = _player.Stats.wallJumpControlLockDuration ?? 0.2f; // Add to PlayerStatsSO
                }
            }

            public override void Enter()
            {
                // Debug.Log("Entering Wall Jump State");
                _player.AnimationController.Play(_wallJumpAnimationName);
                _player.ResetJumpInputFlags();
                _player.SetHasDoubleJumpedThisAirborneSequence(false); // Wall jump also often resets air jump

                // Determine jump direction: away from the wall player was touching
                // _player.WallDirectionX is -1 if wall is on left, 1 if wall is on right.
                // So, jump horizontally in the opposite direction.
                float jumpDirX = -_player.WallDirectionX; 
                // Vertical component is usually mostly up, but can be angled.
                // Using a fixed upward angle for simplicity, e.g., (1, 1.5) normalized.
                _wallJumpDirection = new Vector2(jumpDirX, 1.5f).normalized; 

                _player.Rigidbody.velocity = Vector2.zero; // Reset velocity before applying force
                _player.Rigidbody.AddForce(_wallJumpDirection * _wallJumpForce, ForceMode2D.Impulse);
                
                // Flip player to face away from the wall
                if (jumpDirX * _player.transform.localScale.x < 0) {
                     _player.transform.localScale = new Vector3(-_player.transform.localScale.x, _player.transform.localScale.y, _player.transform.localScale.z);
                }
            }

            public override void Exit() { }

            public override void Update()
            {
                _wallJumpDuration -= Time.deltaTime;

                if (_wallJumpDuration <= 0f) // After initial burst, transition to normal airborne state
                {
                    // Check if player is still moving upwards or has peaked
                    if (_player.Rigidbody.velocity.y > 0)
                    {
                        // Could transition to a brief "WallJumpApexState" or directly to Jumping/Falling
                        // For simplicity, if still moving up, consider it like a normal jump's upward phase
                        // stateMachine.ChangeState(_player.JumpingState); // Or a custom post-wall-jump airborne state
                        // However, JumpingState applies its own force. Better to go to FallingState
                        // or a generic AirborneState that doesn't apply upward force.
                        // For now, assume it goes to FallingState once control is regained or peak is reached.
                    }
                    // Fall through to next check or directly to FallingState if Y velocity non-positive
                }
                
                // Always transition to FallingState if Y velocity indicates downward movement or peak.
                if (_player.Rigidbody.velocity.y <= 0.1f) // Use a small threshold to catch peak
                {
                     stateMachine.ChangeState(_player.FallingState);
                     return;
                }

                // Allow limited air control after initial burst (if _wallJumpDuration > 0)
                // Or full air control if _wallJumpDuration <= 0
                // No sprite flipping here, as player is committed to initial jump direction during burst.
            }

            public override void FixedUpdate()
            {
                // During the _wallJumpDuration, horizontal input might be ignored or dampened
                // After _wallJumpDuration, normal air control applies.
                if (_wallJumpDuration <= 0f)
                {
                    float airControlFactor = _player.Stats.airControlFactor ?? 1.0f;
                    float targetVelocityX = _player.MoveInput.x * _player.Stats.movementSpeed * airControlFactor;
                    _player.Rigidbody.velocity = new Vector2(targetVelocityX, _player.Rigidbody.velocity.y);
                }
                // Else, velocity is primarily determined by the initial impulse.
            }
        }
    }
    ```
    **Note**: `WallJumpSO` might be created (similar to `DoubleJumpSO`) if wall jump needs specific configurable parameters like `wallJumpForce`, `wallJumpDirection` (as a Vector2), `wallJumpCtrlLockDuration`. These would be accessed via `_player.Abilities.GetWallJumpAbilityData()`. For now, added fallback to `PlayerStatsSO`.

# Acceptance Criteria:
- `WallJumpState.cs` is implemented.
- `PlayerController` instantiates `WallJumpState`.
- `WallGrabState` (and potentially other states) can transition to `WallJumpState` if player presses jump and `PlayerAbilities.CanWallJump()` is true.
- `Enter()`:
    - Plays wall jump animation.
    - Calculates jump direction (upwards and away from the wall).
    - Applies force using parameters from `WallJumpSO` or `PlayerStatsSO`.
    - Flips player to face away from the wall.
    - Resets double jump availability.
- `Update()`/`FixedUpdate()`:
    - Manages a brief period (`wallJumpDuration`) where player input might be restricted.
    - After this duration or when Y velocity is no longer positive, transitions to `FallingState` (or `JumpingState` if still moving up significantly without new force).
- Player can perform a wall jump if the ability is "unlocked".

# Test Strategy:
- Manual Testing:
    - Mock `PlayerAbilities.CanWallJump()` to return true.
    - In test level, have player enter `WallGrabState`.
    - Press jump button while on wall.
    - Verify player detaches, jumps up and away from wall, plays animation.
    - Verify player faces new jump direction.
    - Verify transition to `FallingState` after jump apex.
    - Test if double jump is available after a wall jump.

# Notes/Questions:
- `PlayerStatsSO` needs fields like `wallJumpForce` (float) and `wallJumpControlLockDuration` (float).
- The exact trajectory and feel of the wall jump (force, direction, control lock duration) are key tuning parameters.
- A `WallJumpSO` can be created if more distinct properties are needed than what `PlayerStatsSO` provides. This task assumes parameters are primarily from `PlayerStatsSO` for now, with an optional `WallJumpSO` for more specific data.