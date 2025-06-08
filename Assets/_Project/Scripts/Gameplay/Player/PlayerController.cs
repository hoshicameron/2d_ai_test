using System;
using PetalsOfHope.Core.Events;
using PetalsOfHope.Core.Input;
using PetalsOfHope.Core.StateMachine;
using PetalsOfHope.Data.Abilities.Types;
using PetalsOfHope.Data.Player;
using PetalsOfHope.Editor;
using PetalsOfHope.Gameplay.Player.States;
using PetalsOfHope.Utilities;
using UnityEngine;
using CoreAnimation = PetalsOfHope.Core.Animation.AnimationController;

namespace PetalsOfHope.Gameplay.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(StateMachine))]
    [RequireComponent(typeof(CoreAnimation))]
    public class PlayerController : MonoBehaviour
    {
        #region Serialized Variables

        [Header("Health Events")]
        [Tooltip("Listen to the player dies event.")]
        [SerializeField] private GameEventSO _playerDiedEventSO;
        
        [Header("Data Dependencies")]
        [SerializeField] private PlayerStatsSO _stats;
        [SerializeField] private InputReader _inputReader;
        [SerializeField] private GameEventSO _playerLandedEventSO;

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
        private bool _isClimbing;
        private float _climbInput;

        #endregion
        
        #region Events

        public Action OnDashStart;
        public Action OnDashEnd;
        public Action OnWallGrabEnd;
        public Action OnWallGrabStart;
        public Action OnWallJump;

        #endregion

        #region Properties

        
        public PlayerStatsSO Stats => _stats;
        public InputReader InputReader => _inputReader;
        public Vector2 MoveInput { get; private set; }
        public bool JumpInputPressed { get; private set; }
        public bool JumpInputReleased { get; private set; }
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
        public bool IsClimbing => _isClimbing;
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
        

        private void Awake()
        {
            // Get Core Components
            Rigidbody = GetComponent<Rigidbody2D>();
            Collider = GetComponent<CapsuleCollider2D>();
            StateMachine = GetComponent<StateMachine>();
            AnimationController = GetComponent<CoreAnimation>();

            // Initialize states
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
        
            _inputReader?.EnableGameplayInput();
        }

        private void OnEnable()
        {
            if (_inputReader != null)
            {
                _inputReader.MoveEvent.RegisterListener(HandleMoveInput);
                _inputReader.JumpEvent.RegisterListener(HandleJumpPressedInput);
                _inputReader.JumpCancelledEvent.RegisterListener(HandleJumpReleasedInput);
                _inputReader.DashEvent.RegisterListener(HandleDashInput);
            }
            
            _playerDiedEventSO.RegisterListener(HandlePlayerDeath);
        }

        private void OnDisable()
        {
            if (_inputReader != null)
            {
                _inputReader.MoveEvent.UnregisterListener(HandleMoveInput);
                _inputReader.JumpEvent.UnregisterListener(HandleJumpPressedInput);
                _inputReader.JumpCancelledEvent.UnregisterListener(HandleJumpReleasedInput);
                _inputReader.DashEvent.UnregisterListener(HandleDashInput);
            }
            
            _playerDiedEventSO.UnregisterListener(HandlePlayerDeath);
        }

        private void Update()
        {
            CheckIfGrounded();
            CheckWallCollision();
            CheckLadder();
            
            if (_dashCooldownTimer > 0f)
            {
                _dashCooldownTimer -= Time.deltaTime;
            }
            
            UpdateClimbInput(MoveInput);

            // Update the current state name for debugging
            if (StateMachine != null && StateMachine.CurrentState != null)
            {
                CurrentStateName = StateMachine.CurrentState.GetType().Name;
            }
        }

        private void CheckIfGrounded()
        {
            bool wasGrounded = IsGrounded;
            IsGrounded = Physics2D.OverlapCircle(_groundCheckPoint.position, _groundCheckRadius, _groundLayer);

            if (!wasGrounded && IsGrounded)
            {
                // Reset jumps when grounded
                RemainingJumps = MaxJumps;
                
                // Reset jump input flags when landing
                ResetJumpInputFlags();
                
                _playerLandedEventSO?.Raise();
            }
        }

        private void HandleMoveInput(Vector2 moveValue) => MoveInput = moveValue;
        private void HandleJumpPressedInput() => JumpInputPressed = true;
        private void HandleJumpReleasedInput() => JumpInputReleased = true;
        private void HandleDashInput() => TryStartDash();

        public void ResetJumpInputFlags()
        {
            JumpInputPressed = false;
            JumpInputReleased = false;
        }

        public bool TryStartDash()
        {
            if (_dashCooldownTimer > 0f) return false;
    
            _dashCooldownTimer = _dashData.cooldown;
            IsDashing = true;
            StateMachine.ChangeState(DashState);
            return true;
        }
        
        private void HandlePlayerDeath()
        {
            StateMachine.ChangeState(DeathState);
        }
        
        private void CheckWallCollision()
        {
            if (_wallGrabData == null) return;

            IsTouchingWall = false;
            
            // Calculate the check position based on the player's facing direction and offset
            var checkPosition = (Vector2)transform.position + 
                                new Vector2(
                                    _wallGrabData.wallCheckOffset.x * transform.localScale.x, 
                                    _wallGrabData.wallCheckOffset.y
                                );

            // Check for wall collision using OverlapBox
            var wallHit = Physics2D.OverlapBox(
                checkPosition, 
                _wallGrabData.wallCheckSize, 
                0f, 
                _wallGrabData.wallLayer
            );

            // Update wall collision status
            if (wallHit == null) return;
            IsTouchingWall = true;
            LastWallTouchTime = Time.time;
            // Determine wall side based on hit normal or position
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
        
        private void OnDrawGizmos()
        {
            if (_wallGrabData == null) return;
            
            // Draw wall check box
            Gizmos.color = IsTouchingWall ? Color.green : Color.yellow;
            Vector2 checkPosition = (Vector2)transform.position + 
                                    new Vector2(
                                        _wallGrabData.wallCheckOffset.x * transform.localScale.x, 
                                        _wallGrabData.wallCheckOffset.y
                                    );
            Gizmos.DrawWireCube(checkPosition, _wallGrabData.wallCheckSize);
            
            // Draw direction indicator
            if (IsTouchingWall)
            {
                Gizmos.color = Color.red;
                Vector3 dir = WallSide > 0 ? Vector3.right : Vector3.left;
                Gizmos.DrawRay(transform.position, dir);
            }
            
            if (_groundCheckPoint != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(_groundCheckPoint.position, _groundCheckRadius);
            }
            
            if (_climbData != null)
            {
                Gizmos.color = _isTouchingLadder ? Color.green : Color.yellow;
                Gizmos.DrawWireCube(transform.position, _climbData.ladderCheckSize);
            }
        }
    }
}
