using System;
using PetalsOfHope.Core.StateMachine;
using PetalsOfHope.Data.Abilities.Types;
using PetalsOfHope.Data.Player;
using PetalsOfHope.Gameplay.Player.States;
using PetalsOfHope.Interfaces;
using PetalsOfHope.Utilities;
using UnityEngine;
using CoreAnimation = PetalsOfHope.Core.Animation.AnimationController;

namespace _Project.Scripts.Gameplay.Character
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(StateMachine))]
    [RequireComponent(typeof(CoreAnimation))]
    [RequireComponent(typeof(ICharacterInputSource))] 
    public abstract class CharacterControllerBase : MonoBehaviour
    {
        #region Input Source
        
        // The source of the character's actions (player, AI, etc.)
        // It is protected so child classes can access it if needed, but not public
        public ICharacterInputSource InputSource { get; private set; }

        #endregion
        
        #region Serialized Variables

        [Header("Data Dependencies")]
        // Consider creating a generic CharacterStatsSO to be used by both players and enemies
        [SerializeField] private CharacterStatsSo _stats; 
        
        [Header("Ground Check")]
        [SerializeField] private Transform _groundCheckPoint;
        [SerializeField] private float _groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask _groundLayer;
        
        [Header("Abilities Data")]
        [SerializeField] private DashSO _dashData;
        [SerializeField] private WallGrabSO _wallGrabData;
        [SerializeField] private WallJumpSO _wallJumpData;
        [SerializeField] private DoubleJumpSO _doubleJumpData;
        [SerializeField] private ClimbSO _climbData;
        
        
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

        #endregion
        
        #region Events

        public Action OnDashStart;
        public Action OnDashEnd;
        public Action OnWallGrabEnd;
        public Action OnWallGrabStart;
        public Action OnWallJump;

        #endregion

        #region Properties
        
        // This is now generic, so it could be PlayerStatsSO or EnemyStatsSO if they share a base class
        public CharacterStatsSo Stats => _stats;
        public Vector2 MoveInput { get; private set; }
        public bool JumpInputPressed { get; private set; }
        public bool JumpInputReleased { get; private set; }
        public bool DashInputPressed { get; private set; }
        public bool IsGrounded { get; private set; }
        public Rigidbody2D Rigidbody { get; private set; }
        public CapsuleCollider2D Collider { get; private set; }
        public StateMachine StateMachine { get; private set; }
        public CoreAnimation AnimationController { get; private set; }
        public int RemainingJumps { get; set; }
        
        public bool IsDashing { get; set; }
        public DashSO DashData => _dashData;
        
        public bool IsTouchingWall { get; private set; }
        public bool IsWallSliding { get;  set; }
        public float LastWallTouchTime { get;  set; }
        public int WallSide { get;  set; } // -1 for left, 1 for right
        public bool IsWallGrabInput => (WallSide == 1 && MoveInput.x > 0.1f) || 
                                       (WallSide == -1 && MoveInput.x < -0.1f);

        public int MaxJumps => _doubleJumpData.MaxJumps;
        public float DoubleJumpForceMultiplier => _doubleJumpData.doubleJumpForceMultiplier;
        
        public bool IsTouchingLadder => _isTouchingLadder;
        public bool CanClimb => _isTouchingLadder && Mathf.Abs(_climbInput) > 0.1f;
        public ClimbSO ClimbData => _climbData;
        public Collider2D CurrentLadder => _currentLadder;
        public float ClimbInput => _climbInput;
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
            Collider = GetComponent<CapsuleCollider2D>();
            StateMachine = GetComponent<StateMachine>();
            AnimationController = GetComponent<CoreAnimation>();

            // Initialize states
            // NOTE: You will need to update the constructors of these State classes
            // to accept the ICharacterInputSource so they can call ConsumeJumpInput() etc.
            IdleState = new IdleState(this, StateMachine, idleAnimationName);
            MovingState = new MovingState(this, StateMachine, moveAnimationName);
            JumpingState = new JumpingState(this, StateMachine, jumpAnimationName);
            FallingState = new FallingState(this, StateMachine, fallAnimationName);
            DashState = new DashState(this, StateMachine, dashAnimationName, DashData);
            DeathState = new DeathState(this, StateMachine, deathAnimationName);
            WallGrabState = new WallGrabState(this, StateMachine, wallGrabAnimationName, _wallGrabData);
            WallJumpState = new WallJumpState(this, StateMachine, wallJumpAnimationName, _wallJumpData);
            ClimbState = new ClimbState(this, StateMachine, climbIdleAnimationName, climbDownAnimationName,
                climbUpAnimationName, _climbData);
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
        }

        protected virtual void OnEnable()
        {
            if (InputSource == null) return;
            
            InputSource.DashEvent += HandleDashInput;
            InputSource.MoveEvent += HandleMoveInput;
            InputSource.AttackEvent += HandleAttackInput;
            InputSource.JumpEvent += HandleJumpPressed;
            InputSource.JumpCancelledEvent += HandleJumpInputReleased;
        }
        
        protected virtual void OnDisable()
        {
            if (InputSource == null) return;
            
            InputSource.DashEvent -= HandleDashInput;
            InputSource.MoveEvent -= HandleMoveInput;
            InputSource.AttackEvent -= HandleAttackInput;
            InputSource.JumpEvent -= HandleJumpPressed;
            InputSource.JumpCancelledEvent -= HandleJumpInputReleased;
        }

        private void HandleJumpInputReleased() => JumpInputReleased = true;

        private void HandleJumpPressed() => JumpInputPressed = true;

        private void HandleAttackInput() => Debug.Log("Attack Input Pressed");

        private void HandleMoveInput(Vector2 moveInput) => MoveInput = moveInput;
        private void HandleDashInput() => StartDash();

        protected virtual void Update()
        {
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
            if (_dashCooldownTimer > 0f || !DashInputPressed) return ;
    
            _dashCooldownTimer = _dashData.cooldown;
            IsDashing = true;
            StateMachine.ChangeState(DashState);
        }
        
        private void HandleCharacterDeath()
        {
            StateMachine.ChangeState(DeathState);
        }
        
        private void CheckWallCollision()
        {
            if (_wallGrabData == null) return;

            IsTouchingWall = false;
            
            var checkPosition = (Vector2)transform.position + 
                                new Vector2(
                                    _wallGrabData.wallCheckOffset.x * transform.localScale.x, 
                                    _wallGrabData.wallCheckOffset.y
                                );

            var wallHit = Physics2D.OverlapBox(
                checkPosition, 
                _wallGrabData.wallCheckSize, 
                0f, 
                _wallGrabData.wallLayer
            );

            if (wallHit == null) return;
            IsTouchingWall = true;
            LastWallTouchTime = Time.time;
            WallSide = wallHit.transform.position.x > transform.position.x ? 1 : -1;
        }
        
       public bool CanWallGrab()
        {
            if (_wallGrabData == null) return false;
            
            return IsTouchingWall && 
                   !IsGrounded && 
                   IsWallGrabInput && 
                   Time.time < LastWallTouchTime + _wallGrabData.wallGrabTime;
        }
        
        public bool CanWallJump()
        {
            if (_wallJumpData == null) return false;
            
            // Note: We check JumpInputPressed directly from the property
            return (IsTouchingWall || Time.time < LastWallTouchTime + _wallJumpData.coyoteWallTime) && 
                   !IsGrounded && 
                   JumpInputPressed;
        }
        
        private void CheckLadder()
        {
            if (_climbData == null) return;

            var hit = Physics2D.OverlapBox(
                transform.position,
                _climbData.ladderCheckSize,
                0f,
                _climbData.climbableLayer
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
            if (_wallGrabData != null)
            {
                Gizmos.color = IsTouchingWall ? Color.green : Color.yellow;
                Vector2 checkPosition = (Vector2)transform.position + 
                                        new Vector2(
                                            _wallGrabData.wallCheckOffset.x * transform.localScale.x, 
                                            _wallGrabData.wallCheckOffset.y
                                        );
                Gizmos.DrawWireCube(checkPosition, _wallGrabData.wallCheckSize);
            }
            
            // Draw Ladder Check
            if (_climbData != null)
            {
                Gizmos.color = _isTouchingLadder ? Color.green : Color.yellow;
                Gizmos.DrawWireCube(transform.position, _climbData.ladderCheckSize);
            }
        }
    }
}