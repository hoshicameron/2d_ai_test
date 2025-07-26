# Task ID: 4.4.5
# Parent Task ID: 4.4
# Title: Implement DashState
# Status: completed
# Dependencies: 2.4.1, 4.4.1, 1.3.2 # PlayerController, PlayerAbilities, DashSO
# Priority: high
# Estimated Effort: L
# Assignee: Unassigned

# Description:
Implement `DashState.cs` for the player. This state allows the player to perform a quick burst of movement in their current facing direction (or input direction), if the dash ability is unlocked and off cooldown.

# Details:
1.  **`PlayerController` Modifications:**
    *   Add `public DashState DashState { get; private set; }`
    *   Instantiate in `Awake()`: `DashState = new DashState(this, StateMachine, "Player_Dash_Anim");`
    *   Add input handling for Dash in `PlayerController.OnEnable/OnDisable` (listening to `InputReader.DashEvent`) and a handler method:
        ```csharp
        // In PlayerController.cs
        // private GameEventSO _dashInputEvent; // From InputReader, assigned in inspector or fetched
        // OnEnable(): _inputReader.DashEvent.RegisterListener(HandleDashInput);
        // OnDisable(): _inputReader.DashEvent.UnregisterListener(HandleDashInput);
        
        // public bool DashInputPressed { get; private set; } // Add this
        // private void HandleDashInput() { DashInputPressed = true; }
        // In Update(): if (DashInputPressed) { /* logic to try entering dash state */ DashInputPressed = false; }
        ```
        Alternatively, states themselves can check for `DashInputPressed`. The plan for `InputReader` (2.1.2) has `DashEvent` (GameEventSO), so `PlayerController` handles this.

2.  **Modify relevant states (e.g., `IdleState`, `MovingState`, `JumpingState`, `FallingState`) to transition to `DashState`:**
    *   In their `Update()` methods, check for dash input:
        ```csharp
        // In applicable states' Update() methods
        // if (_player.DashInputPressed && _player.Abilities.CanDash())
        // {
        //     stateMachine.ChangeState(_player.DashState);
        //     _player.ConsumeDashInput(); // Method in PlayerController to reset DashInputPressed
        //     return;
        // }
        ```
    *  `PlayerController` would need `public bool DashInputPressed { get; private set; }` and `public void ConsumeDashInput() { DashInputPressed = false; }`. `HandleDashInput` in `PlayerController` sets `DashInputPressed = true;`.

3.  **Implementation of `DashState.cs`:**
    ```csharp
    // In Assets/_Project/Scripts/Gameplay/Player/States/DashState.cs
    namespace PetalsOfHope.Gameplay.Player.States
    {
        using UnityEngine;
        using PetalsOfHope.Core.StateMachine;
        using PetalsOfHope.Data.Abilities.Types; // For DashSO

        public class DashState : BaseState
        {
            private readonly PlayerController _player;
            private readonly string _dashAnimationName;
            
            private float _dashSpeed;
            private float _dashDuration;
            private float _dashTimer;
            private Vector2 _dashDirection;

            public DashState(PlayerController player, StateMachine stateMachine, string animationName) : base(stateMachine)
            {
                _player = player;
                _dashAnimationName = animationName;
            }

            public override void Enter()
            {
                // Debug.Log("Entering Dash State");
                _player.AnimationController.Play(_dashAnimationName);
                _player.Abilities.NotifyDashUsed(); // Starts cooldown in PlayerAbilities

                var dashAbilityData = _player.Abilities.GetDashAbilityData() as DashSO;
                if (dashAbilityData == null) {
                    Debug.LogError("DashSO not found or not of correct type in PlayerAbilities!");
                    // Fallback or exit state
                    _dashSpeed = 20f; 
                    _dashDuration = 0.2f;
                } else {
                    _dashSpeed = dashAbilityData.dashSpeed;
                    _dashDuration = dashAbilityData.dashDuration;
                }
                
                _dashTimer = _dashDuration;

                // Determine dash direction:
                // Option 1: Current facing direction
                _dashDirection = new Vector2(Mathf.Sign(_player.transform.localScale.x), 0);
                // Option 2: Based on current move input (if any, else facing direction)
                if (Mathf.Abs(_player.MoveInput.x) > Mathf.Epsilon || Mathf.Abs(_player.MoveInput.y) > Mathf.Epsilon) {
                    _dashDirection = _player.MoveInput.normalized; // Allows 8-directional dash based on input
                    // If strictly horizontal dash desired even with Y input, then:
                    // _dashDirection = new Vector2(Mathf.Sign(_player.MoveInput.x != 0 ? _player.MoveInput.x : _player.transform.localScale.x), 0).normalized;
                } else if (_dashDirection.x == 0) { // If no input and facing direction somehow neutral (e.g. scale.x=0), default right
                     _dashDirection = Vector2.right;
                }


                if (_dashDirection == Vector2.zero) _dashDirection = Vector2.right * Mathf.Sign(_player.transform.localScale.x); // Default if input is neutral

                // Make player temporarily immune to gravity or other forces if desired
                _player.Rigidbody.gravityScale = 0f; 
                _player.Rigidbody.velocity = _dashDirection * _dashSpeed; // Apply dash velocity
                
                // Optional: Make player intangible or ignore collisions for dash duration
                // _player.Collider.isTrigger = true; // Or change layers
            }

            public override void Exit()
            {
                _player.Rigidbody.gravityScale = _player.Stats.originalGravityScale ?? 3f; // Restore original gravity. Add originalGravityScale to PlayerStatsSO
                // If velocity was not set to zero on exit, player might retain some dash momentum.
                // Depending on game feel, might want to reset X velocity or let it be.
                // _player.Rigidbody.velocity = new Vector2(0, _player.Rigidbody.velocity.y); // Option: Stop horizontal movement
                
                // Restore collision if changed
                // _player.Collider.isTrigger = false;
            }

            public override void Update()
            {
                _dashTimer -= Time.deltaTime;
                if (_dashTimer <= 0f)
                {
                    // Dash finished, transition to appropriate state
                    if (_player.IsGrounded)
                    {
                        stateMachine.ChangeState(Mathf.Abs(_player.MoveInput.x) > Mathf.Epsilon ? _player.MovingState : _player.IdleState);
                    }
                    else
                    {
                        stateMachine.ChangeState(_player.FallingState); // Or JumpingState if Y velocity is still positive
                    }
                }
            }

            public override void FixedUpdate()
            {
                // Maintain dash velocity during the dash, overriding other forces
                // This ensures dash isn't slowed by friction or slight bumps if desired.
                if (_dashTimer > 0f) { // Only if still dashing
                    _player.Rigidbody.velocity = _dashDirection * _dashSpeed;
                }
            }
        }
    }
    ```

# Acceptance Criteria:
- `DashState.cs` is implemented.
- `PlayerController` instantiates `DashState` and handles dash input to trigger transitions.
- Relevant states (`Idle`, `Moving`, `Jumping`, `Falling`) can transition to `DashState` if `PlayerAbilities.CanDash()` is true.
- `Enter()`:
    - Plays dash animation.
    - Calls `PlayerAbilities.NotifyDashUsed()` to start cooldown.
    - Determines dash direction (facing or input-based).
    - Applies dash velocity using `DashSO.dashSpeed`.
    - Temporarily sets `Rigidbody.gravityScale` to 0.
- `Update()`:
    - Manages `_dashTimer` based on `DashSO.dashDuration`.
    - Transitions to an appropriate airborne or grounded state when dash timer expires.
- `FixedUpdate()` maintains dash velocity during the dash.
- `Exit()` restores original gravity scale.
- Player can perform a dash if the ability is "unlocked" and off cooldown.

# Test Strategy:
- Manual Testing:
    - Mock `PlayerAbilities.CanDash()` to return true initially.
    - Assign a `DashSO` asset to `PlayerAbilities`.
    - In test level, press dash input from various states (Idle, Moving, Jumping, Falling).
    - Verify player performs a quick dash, plays animation.
    - Verify dash direction (facing vs. input-based).
    - Verify player becomes controllable again after dash duration.
    - Test dash cooldown: attempt to dash again immediately; it should fail until cooldown expires.
    - Verify gravity is restored after dash.

# Notes/Questions:
- `PlayerStatsSO` needs `originalGravityScale` (or this can be cached by `PlayerController` in `Awake`). Let's assume `PlayerStatsSO` has it for now.
- Dash direction (based on facing or current input) is a design choice. The example provides a common input-based approach.
- Invincibility or passing through certain objects during dash could be added by manipulating colliders or layers in `Enter()`/`Exit()`.
- The dash input event comes from `InputReader.DashEvent` (Task 2.1.2), which `PlayerController` listens to and sets a flag like `DashInputPressed`. States then check this flag.