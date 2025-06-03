# Task ID: 2.4.1
# Parent Task ID: 2.4
# Title: Implement PlayerController MonoBehaviour Shell and Component Setup
# Status: pending
# Dependencies: 2.1.2, 2.2, 2.3.2, 1.3.1, 1.2.8 # InputReader SO, AnimationController script, StateMachine script, PlayerStatsSO definition, EventSO assets
# Priority: critical
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement the basic structure of `PlayerController.cs`, including required component references (`Rigidbody2D`, `Collider2D`, `StateMachine`, `AnimationController`), fields for `PlayerStatsSO` and `InputReader`, and basic setup in `Awake()` and `Start()`.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/Gameplay/Player/PlayerController.cs`
2.  **Namespace:** `PetalsOfHope.Gameplay.Player`
3.  **Implementation Shell:**
    ```csharp
    // In Assets/_Project/Scripts/Gameplay/Player/PlayerController.cs
    namespace PetalsOfHope.Gameplay.Player
    {
        using UnityEngine;
        using PetalsOfHope.Core.Input;
        using PetalsOfHope.Core.StateMachine;
        using CoreAnimation = PetalsOfHope.Core.Animation.AnimationController; // Alias to avoid conflict if Unity.Animation is used
        using PetalsOfHope.Data.Player;
        using PetalsOfHope.Core.Events; // For EventSOs

        [RequireComponent(typeof(Rigidbody2D))]
        [RequireComponent(typeof(CapsuleCollider2D))] // Or BoxCollider2D, adjust as needed
        [RequireComponent(typeof(StateMachine))]
        [RequireComponent(typeof(CoreAnimation))]
        public class PlayerController : MonoBehaviour
        {
            [Header("Data Dependencies")]
            [Tooltip("ScriptableObject containing player's statistics.")]
            [SerializeField] private PlayerStatsSO _stats;
            public PlayerStatsSO Stats => _stats;

            [Tooltip("ScriptableObject responsible for reading player input.")]
            [SerializeField] private InputReader _inputReader;
            public InputReader InputReader => _inputReader; // States might need this for complex input checks not covered by events

            [Header("Movement Parameters (Read by States)")]
            public Vector2 MoveInput { get; private set; }
            public bool JumpInputPressed { get; private set; }
            public bool JumpInputReleased { get; private set; } // For variable jump height

            [Header("Ground Check")]
            [SerializeField] private Transform _groundCheckPoint;
            [SerializeField] private float _groundCheckRadius = 0.2f;
            [SerializeField] private LayerMask _groundLayer;
            public bool IsGrounded { get; private set; }

            [Header("Player Action Events (Optional - Raised by PlayerController or States)")]
            [SerializeField] private GameEventSO _playerLandedEventSO;
            // [SerializeField] private GameEventSO _playerTookDamageEventSO; // This is more for PlayerHealth

            // Component References
            public Rigidbody2D Rigidbody { get; private set; }
            public CapsuleCollider2D Collider { get; private set; } // Or BoxCollider2D
            public StateMachine StateMachine { get; private set; }
            public CoreAnimation AnimationController { get; private set; }
            // public SpriteRenderer SpriteRenderer { get; private set; } // If needed for flipping sprite

            // State Instances (will be instantiated in Awake)
            public IdleState IdleState { get; private set; }
            public MovingState MovingState { get; private set; }
            public JumpingState JumpingState { get; private set; }
            public FallingState FallingState { get; private set; }
            // ... other states ...

            private void Awake()
            {
                // Get Core Components
                Rigidbody = GetComponent<Rigidbody2D>();
                Collider = GetComponent<CapsuleCollider2D>(); // Adjust if using BoxCollider2D
                StateMachine = GetComponent<StateMachine>();
                AnimationController = GetComponent<CoreAnimation>();
                // SpriteRenderer = GetComponentInChildren<SpriteRenderer>(); // Or GetComponent<SpriteRenderer>()

                if (_stats == null) Debug.LogError("PlayerStatsSO not assigned to PlayerController.", this);
                if (_inputReader == null) Debug.LogError("InputReader not assigned to PlayerController.", this);
                if (_groundCheckPoint == null) Debug.LogError("GroundCheckPoint transform not assigned to PlayerController.", this);

                // Instantiate States (pass 'this' PlayerController instance for context)
                IdleState = new IdleState(this, StateMachine, "idle"); // "idle" is an example animation state name
                MovingState = new MovingState(this, StateMachine, "move");
                JumpingState = new JumpingState(this, StateMachine, "jump");
                FallingState = new FallingState(this, StateMachine, "fall");
            }

            private void Start()
            {
                if (StateMachine != null && IdleState != null)
                {
                    StateMachine.Initialize(IdleState);
                }
                else
                {
                    Debug.LogError("StateMachine or IdleState is null. Cannot initialize PlayerController's StateMachine.", this);
                }
                // Enable gameplay input via InputReader
                _inputReader?.EnableGameplayInput();
            }

            private void OnEnable()
            {
                // Register input event listeners
                if (_inputReader != null)
                {
                    _inputReader.MoveEvent.RegisterListener(HandleMoveInput);
                    _inputReader.JumpEvent.RegisterListener(HandleJumpPressedInput);
                    _inputReader.JumpCancelledEvent.RegisterListener(HandleJumpReleasedInput);
                    // ... register other gameplay actions like Dash, Interact ...
                }
            }

            private void OnDisable()
            {
                // Unregister input event listeners
                if (_inputReader != null)
                {
                    _inputReader.MoveEvent.UnregisterListener(HandleMoveInput);
                    _inputReader.JumpEvent.UnregisterListener(HandleJumpPressedInput);
                    _inputReader.JumpCancelledEvent.UnregisterListener(HandleJumpReleasedInput);
                    // ... unregister others ...
                }
            }

            private void Update()
            {
                CheckIfGrounded();
                // StateMachine.Update() is called automatically by its own MonoBehaviour Update.
                // Handle sprite flipping based on movement direction if not handled by states
            }

            private void FixedUpdate()
            {
                // StateMachine.FixedUpdate() is called automatically.
            }

            // Input Handlers
            private void HandleMoveInput(Vector2 moveValue)
            {
                MoveInput = moveValue;
            }

            private void HandleJumpPressedInput()
            {
                JumpInputPressed = true;
                // States will check this flag and reset it after consuming
            }
            
            private void HandleJumpReleasedInput()
            {
                JumpInputReleased = true;
                // States (like JumpingState) will check this flag for variable jump height
            }

            private void CheckIfGrounded()
            {
                bool wasGrounded = IsGrounded;
                IsGrounded = Physics2D.OverlapCircle(_groundCheckPoint.position, _groundCheckRadius, _groundLayer);

                if (!wasGrounded && IsGrounded)
                {
                    _playerLandedEventSO?.Raise();
                    // This event can be used by FallingState to transition to Idle/Moving
                    // or by other systems (sound, particles).
                }
            }

            public void ResetJumpInputFlags()
            {
                JumpInputPressed = false;
                JumpInputReleased = false;
            }

            // Gizmo for ground check visualization
            private void OnDrawGizmosSelected()
            {
                if (_groundCheckPoint != null)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawWireSphere(_groundCheckPoint.position, _groundCheckRadius);
                }
            }
        }
    }
    ```

# Acceptance Criteria:
- `PlayerController.cs` is created with `RequireComponent` attributes for `Rigidbody2D`, `Collider2D` (Capsule or Box), `StateMachine`, and `CoreAnimation` (the alias for our AnimationController).
- Fields for `_stats` (`PlayerStatsSO`), `_inputReader` (`InputReader`), `_groundCheckPoint`, `_groundCheckRadius`, `_groundLayer` are present and serialized.
- Public properties expose key components and data (e.g., `Rigidbody`, `Stats`, `AnimationController`, `MoveInput`, `IsGrounded`).
- `Awake()` correctly gets component references and instantiates placeholder state objects (passing `this` and the `StateMachine` instance). Null checks for critical dependencies are included.
- `Start()` initializes the `StateMachine` with `IdleState` and enables gameplay input.
- `OnEnable()`/`OnDisable()` register/unregister input event listeners from `InputReader`.
- Input handler methods (`HandleMoveInput`, `HandleJumpPressedInput`, `HandleJumpReleasedInput`) update internal input state variables.
- `CheckIfGrounded()` method uses `Physics2D.OverlapCircle` to update `IsGrounded` and raises `_playerLandedEventSO` on landing.
- `OnDrawGizmosSelected()` visualizes the ground check sphere.
- `ResetJumpInputFlags()` method is available for states to call.
- Script compiles without errors.

# Test Strategy:
- Manual Verification:
    - Create a Player GameObject. Attach `Rigidbody2D`, `CapsuleCollider2D` (or `BoxCollider2D`), `Animator`, `StateMachine`, and `CoreAnimation` (our AnimationController).
    - Attach `PlayerController.cs`.
    - Assign a `PlayerStatsSO` asset, an `InputReader` SO asset, a Transform for `_groundCheckPoint`, and set `_groundLayer`.
    - Assign `PlayerLandedEventSO`.
    - In Play Mode, check for any errors in `Awake()` or `Start()`.
    - Verify `IsGrounded` updates correctly when the player is on/off a surface matching `_groundLayer`.
    - Press input keys and verify `MoveInput`, `JumpInputPressed` change accordingly (use debugger or log values).
    - Observe gizmo for ground check in Scene view.

# Notes/Questions:
- A `CapsuleCollider2D` is often preferred for player characters for smoother movement over small bumps, but `BoxCollider2D` can also work. The choice affects `GetComponent<...Collider2D>()`. The plan does not specify; `CapsuleCollider2D` is chosen as a common default.
- State instantiation in `Awake()`: `new IdleState(this, StateMachine, "idleAnimationName")`. The last parameter is a placeholder for the animation state name/hash this state should trigger. This will be refined when states are implemented.
- Input flags like `JumpInputPressed` are typically consumed by states and then reset (e.g., in the state's `Update` or `Enter` method). A `ResetJumpInputFlags()` method is added for convenience.
- `SpriteRenderer` might be needed for flipping the character sprite; this can be added if horizontal input also controls facing direction.