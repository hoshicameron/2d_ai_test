using System;
using PetalOfHope.Gameplay.Character.Movement;
using PetalsOfHope.Data.Abilities;
using PetalsOfHope.Gameplay.StateMachine;
using PetalsOfHope.Gameplay.States;
using PetalsOfHope.Interfaces;
using PetalsOfHope.Utilities;
using UnityEngine;
using CoreAnimation = PetalsOfHope.Gameplay.Animation.AnimationController;

namespace PetalOfHope.Gameplay.Character
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(StateMachine))]
    [RequireComponent(typeof(CoreAnimation))]
    [RequireComponent(typeof(ICharacterInputSource))] 
    public abstract class CharacterControllerBase : MonoBehaviour, ICharacterController
    {
        #region Mover Strategy
        [Header("Movement Strategy")]
        [Tooltip("The ScriptableObject that defines how this character moves and handles physics.")]
        [SerializeField] private Mover _mover;
        public IMover Mover => _mover;
        #endregion
        
        #region Input Source
        
        // The source of the character's actions (player, AI, etc.)
        // It is protected so child classes can access it if needed, but not public
        public ICharacterInputSource InputSource { get; private set; }

        #endregion
        
        #region Serialized Variables

        [Header("Ground Check")]
        [SerializeField] private Transform _groundCheckPoint;
        [SerializeField] private float _groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask _groundLayer;

        [Space] [Header("Abilities Data")] 
        [SerializeField] private AbilitySheetSO _abilitySheetData;

        [Header("Event Listeners")]
        [SerializeField] private PetalsOfHope.Core.Events.Channels.AbilityCheckChannelSO abilityCheckChannel;
        [SerializeField] private PetalsOfHope.Core.Events.GameEventSO sceneLoadCompletedEvent;
        [SerializeField] private PetalsOfHope.Core.Events.GameEventSO progressionChangedEvent;
        
        [Header("Animation")]
        [SerializeField] private string idleAnimationName = "Idle";
        [SerializeField] private string jumpAnimationName = "Jump";
        [SerializeField] private string fallAnimationName = "Fall";
        [SerializeField] private string moveAnimationName = "Move";
        [SerializeField] private string dashAnimationName = "Dash";
        [SerializeField] private string wallGrabAnimationName = "WallGrab";
        [SerializeField] private string wallJumpAnimationName = "WallJump";
        [SerializeField] private string deathAnimationName = "Death";
        [SerializeField] private string climbUpAnimationName = "ClimbUp";
        [SerializeField] private string climbDownAnimationName = "ClimbDown";
        [SerializeField] private string climbIdleAnimationName = "ClimbIdle";

        [field: SerializeField, ReadOnly] public string CurrentStateName { get; set; } = "None";

        #endregion

        #region Private Variables

        private float _dashCooldownTimer;
        private bool _isTouchingLadder;
        private Collider2D _currentLadder;
        private float _climbInput;
        protected bool wasGrounded;

        // Ability Flags
        private bool _isDashUnlocked;
        private bool _isDoubleJumpUnlocked;
        private bool _isWallGrabUnlocked;
        private bool _isWallJumpUnlocked;

        #endregion
        
        #region Events

        public Action OnDashStart;
        public Action OnDashEnd;
        public Action OnWallGrabEnd;
        public Action OnWallGrabStart;
        public Action OnWallJump;

        #endregion

        #region Properties
        
        public Vector2 MoveInput { get; protected set; }
        public bool JumpInputPressed { get; protected set; }
        public bool JumpInputReleased { get; protected set; }
        public bool DashInputPressed { get; private set; }
        public bool IsGrounded { get; protected set; }
        public GameObject GameObject => gameObject;
        public Transform Transform => transform;
        public Rigidbody2D Rigidbody { get; private set; }
        public Collider2D Collider { get; private set; }
        public StateMachine StateMachine { get; protected set; }
        public CoreAnimation AnimationController { get; protected set; }
        public int RemainingJumps { get; set; }
        
        public bool IsDashing { get; set; }
        
        public bool IsTouchingWall { get; private set; }
        public bool IsWallSliding { get;  set; }
        public float LastWallTouchTime { get;  set; }
        public int WallSide { get;  set; } // -1 for left, 1 for right
        public bool IsWallGrabInput => (WallSide == 1 && MoveInput.x > 0.1f) || 
                                       (WallSide == -1 && MoveInput.x < -0.1f);

        public bool IsTouchingLadder => _isTouchingLadder;
        public bool CanClimb => _isTouchingLadder && Mathf.Abs(_climbInput) > 0.1f;
        public Collider2D CurrentLadder => _currentLadder;
        public float ClimbInput => _climbInput;

        public AbilitySheetSO AbilitySheetData => _abilitySheetData;
        
        #endregion

        #region State Instances

        public IdleState IdleState { get; private set; }
        public MovingState MovingState { get; private set; }
        public JumpingState JumpingState { get; private set; }
        public FallingState FallingState { get; private set; }
        public DashState DashState { get; private set; }
        public DeathState DeathState { get; private set; }
        public WallGrabState WallGrabState { get; private set; }
        public WallJumpState WallJumpState { get; private set; }
        public ClimbState ClimbState { get; private set; }
        public RespawnState RespawnState { get; private set; }
        

        #endregion
        
        // Awake is now protected and virtual, allowing child classes to extend it if needed.
        protected virtual void Awake()
        {
            // Get the component that dictates the character's behavior.
            InputSource = GetComponent<ICharacterInputSource>();
            if (InputSource == null)
            {
                Debug.LogError("CharacterControllerBase requires a component that implements ICharacterInputSource!", this);
            }
            
            // Get Core Components
            Rigidbody = GetComponent<Rigidbody2D>();
            Collider = GetComponent<Collider2D>();
            StateMachine = GetComponent<StateMachine>();
            AnimationController = GetComponent<CoreAnimation>();

            InitializeStates();

            if (_mover != null)
            {
                _mover = Instantiate(_mover);
                _mover.Init(this);
            }
            else
            {
                Debug.Log("Mover not assigned. please assign a mover in editor.");
            }
        }

        private void InitializeStates()
        {
            // Initialize states
            //Persistant States
            DeathState = new DeathState(this, StateMachine, deathAnimationName);
            RespawnState = new RespawnState(this, StateMachine);
            IdleState = new IdleState(this, StateMachine, idleAnimationName);
            
            //Optional States
            if(_abilitySheetData.moveData.isEnabled)
                MovingState = new MovingState(this, StateMachine, moveAnimationName);
            if(_abilitySheetData.jumpData.isEnabled)
                JumpingState = new JumpingState(this, StateMachine, jumpAnimationName);
            if(_abilitySheetData.dashData.isEnabled)
                DashState = new DashState(this, StateMachine, dashAnimationName, _abilitySheetData.dashData);
            if(_abilitySheetData.wallGrabData.isEnabled)
                WallGrabState = new WallGrabState(this, StateMachine, wallGrabAnimationName, _abilitySheetData.wallGrabData);
            if(_abilitySheetData.wallJumpData.isEnabled)
                WallJumpState = new WallJumpState(this, StateMachine, wallJumpAnimationName, _abilitySheetData.wallJumpData);
            if(_abilitySheetData.climbData.isEnabled)
                ClimbState = new ClimbState(this, StateMachine, climbIdleAnimationName, climbDownAnimationName,
                    climbUpAnimationName, _abilitySheetData.climbData);

            if (_abilitySheetData.fallData.isEnabled)
            {
                FallingState = new FallingState(this, StateMachine, fallAnimationName);
                Rigidbody.gravityScale = _abilitySheetData.fallData.gravityScale;
            }
            else
            {
                Rigidbody.gravityScale = 0;
            }
        }

        protected virtual void Start()
        {
            if (StateMachine != null && IdleState != null)
            {
                StateMachine.Initialize(IdleState);
            }
            else
            {
                Debug.LogError("StateMachine or IdleState is null. Cannot initialize Character's StateMachine.", this);
            }

            InputSource?.EnableGameplayInput();
            UpdateUnlockedAbilities();
        }

        protected virtual void OnEnable()
        {
            if (InputSource == null) return;
            
            InputSource.DashEvent += HandleDashInput;
            InputSource.MoveEvent += HandleMoveInput;
            InputSource.AttackEvent += HandleAttackInput;
            InputSource.JumpEvent += HandleJumpPressed;
            InputSource.JumpCancelledEvent += HandleJumpInputReleased;

            if (sceneLoadCompletedEvent != null)
            {
                sceneLoadCompletedEvent.RegisterListener(UpdateUnlockedAbilities);
            }
            if (progressionChangedEvent != null)
            {
                progressionChangedEvent.RegisterListener(UpdateUnlockedAbilities);
            }
        }
        
        protected virtual void OnDisable()
        {
            if (InputSource == null) return;
            
            InputSource.DashEvent -= HandleDashInput;
            InputSource.MoveEvent -= HandleMoveInput;
            InputSource.AttackEvent -= HandleAttackInput;
            InputSource.JumpEvent -= HandleJumpPressed;
            InputSource.JumpCancelledEvent -= HandleJumpInputReleased;

            if (sceneLoadCompletedEvent != null)
            {
                sceneLoadCompletedEvent.UnregisterListener(UpdateUnlockedAbilities);
            }
            if (progressionChangedEvent != null)
            {
                progressionChangedEvent.UnregisterListener(UpdateUnlockedAbilities);
            }
        }

        private void HandleJumpInputReleased() => JumpInputReleased = true;

        private void HandleJumpPressed() => JumpInputPressed = true;

        private void HandleAttackInput() => Debug.Log("Attack Input Pressed");

        private void HandleMoveInput(Vector2 moveInput) => MoveInput = moveInput;
        private void HandleDashInput() => StartDash();

        protected virtual void Update()
        {
            StateMachine.UpdateState();
            // Universal physics and environment checks
            CheckIfGrounded();
            HandleCharacterLanded();
            CheckWallCollision();
            CheckLadder();
            
            if (_dashCooldownTimer > 0f)
            {
                _dashCooldownTimer -= Time.deltaTime;
            }
            
            UpdateClimbInput(MoveInput);

            // Update the current state name for debugging
            if (StateMachine.CurrentState != null)
            {
                CurrentStateName = StateMachine.CurrentState.GetType().Name;
            }
        }

        protected virtual void FixedUpdate()
        {
            // The StateMachine decides the logical action (e.g., calling Mover.Move).
            StateMachine.FixedUpdateState();
        }
        
        public void ResetJumpInputFlags()
        {
            JumpInputPressed = false;
            JumpInputReleased = false;
        }
        

        private void CheckIfGrounded()
        {
            wasGrounded = IsGrounded;
            IsGrounded = Physics2D.OverlapCircle(_groundCheckPoint.position, _groundCheckRadius, _groundLayer);
        }

        protected virtual void HandleCharacterLanded()
        {
            
        }
        
        private void StartDash()
        {
            if (_isDashUnlocked && _dashCooldownTimer <= 0f && DashInputPressed)
            {
                _dashCooldownTimer = _abilitySheetData.dashData.cooldown;
                IsDashing = true;
                StateMachine.ChangeState(DashState);
            }
        }
        
        
        private void CheckWallCollision()
        {
            if (_abilitySheetData.wallGrabData == null) return;

            IsTouchingWall = false;
            
            var checkPosition = (Vector2)transform.position + 
                                new Vector2(
                                    _abilitySheetData.wallGrabData.wallCheckOffset.x * transform.localScale.x, 
                                    _abilitySheetData.wallGrabData.wallCheckOffset.y
                                );

            var wallHit = Physics2D.OverlapBox(
                checkPosition, 
                _abilitySheetData.wallGrabData.wallCheckSize, 
                0f, 
                _abilitySheetData.wallGrabData.wallLayer
            );

            if (wallHit == null) return;
            IsTouchingWall = true;
            LastWallTouchTime = Time.time;
            WallSide = wallHit.transform.position.x > transform.position.x ? 1 : -1;
        }
        
       public bool CanWallGrab()
        {
            if (!_isWallGrabUnlocked || _abilitySheetData.wallGrabData == null) return false;
            
            return IsTouchingWall && 
                   !IsGrounded && 
                   IsWallGrabInput && 
                   Time.time < LastWallTouchTime + _abilitySheetData.wallGrabData.wallGrabTime;
        }
        
        public bool CanWallJump()
        {
            if (!_isWallJumpUnlocked || _abilitySheetData.wallJumpData == null) return false;
            
            // Note: We check JumpInputPressed directly from the property
            return (IsTouchingWall || Time.time < LastWallTouchTime + _abilitySheetData.wallJumpData.coyoteWallTime) && 
                   !IsGrounded && 
                   JumpInputPressed;
        }
        
        private void CheckLadder()
        {
            if (_abilitySheetData.climbData == null) return;

            var hit = Physics2D.OverlapBox(
                transform.position,
                _abilitySheetData.climbData.ladderCheckSize,
                0f,
                _abilitySheetData.climbData.climbableLayer
            );

            _isTouchingLadder = hit != null;
            _currentLadder = hit;
        }

        private void UpdateClimbInput(Vector2 moveInput)
        {
            _climbInput = moveInput.y;
        }
        
        protected virtual void OnDrawGizmos()
        {
            // Draw Ground Check
            if (_groundCheckPoint != null)
            {
                Gizmos.color = IsGrounded ? Color.green : Color.red;
                Gizmos.DrawWireSphere(_groundCheckPoint.position, _groundCheckRadius);
            }

            // Draw Wall Check
            if (_abilitySheetData.wallGrabData != null)
            {
                Gizmos.color = IsTouchingWall ? Color.green : Color.yellow;
                Vector2 checkPosition = (Vector2)transform.position + 
                                        new Vector2(
                                            _abilitySheetData.wallGrabData.wallCheckOffset.x * transform.localScale.x, 
                                            _abilitySheetData.wallGrabData.wallCheckOffset.y
                                        );
                Gizmos.DrawWireCube(checkPosition, _abilitySheetData.wallGrabData.wallCheckSize);
            }
            
            // Draw Ladder Check
            if (_abilitySheetData.climbData != null)
            {
                Gizmos.color = _isTouchingLadder ? Color.green : Color.yellow;
                Gizmos.DrawWireCube(transform.position, _abilitySheetData.climbData.ladderCheckSize);
            }
        }
        
        protected void HandleCharacterDeath()
        {
            StateMachine.ChangeState(DeathState);
        }

        private void UpdateUnlockedAbilities()
        {
            if (abilityCheckChannel == null) return;

            _isDashUnlocked = abilityCheckChannel.IsUnlocked?.Invoke("TALISMAN_DASH") ?? false;
            _isDoubleJumpUnlocked = abilityCheckChannel.IsUnlocked?.Invoke("TALISMAN_DOUBLE_JUMP") ?? false;
            _isWallGrabUnlocked = abilityCheckChannel.IsUnlocked?.Invoke("TALISMAN_WALL_GRAB") ?? false;
            _isWallJumpUnlocked = abilityCheckChannel.IsUnlocked?.Invoke("TALISMAN_WALL_JUMP") ?? false;

            // We need to update the max jumps based on the double jump ability
            if (_abilitySheetData != null && _abilitySheetData.jumpData != null)
            {
                RemainingJumps = _isDoubleJumpUnlocked ? _abilitySheetData.jumpData.maxJumps : 1;
            }
        }
    }
}
