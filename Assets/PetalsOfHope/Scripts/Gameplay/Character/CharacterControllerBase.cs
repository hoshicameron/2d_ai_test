using System;
using PetalsOfHope.Core.StateMachine;
using PetalsOfHope.Data.Abilities.Types;
using PetalsOfHope.Data.Player;
using PetalsOfHope.Gameplay.States;
using PetalsOfHope.Interfaces;
using PetalsOfHope.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using CoreAnimation = PetalsOfHope.Core.Animation.AnimationController;

namespace PetalOfHope.Gameplay.Character
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(StateMachine))]
    [RequireComponent(typeof(CoreAnimation))]
    [RequireComponent(typeof(ICharacterInputSource))] 
    public abstract class CharacterControllerBase : MonoBehaviour, ICharacterController
    {
        
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
        
        [Space]
        [Header("Abilities Data")]
        [Header("Assign data to activate ability")]
        [SerializeField] private DashSO dashData;
        [SerializeField] private WallGrabSO wallGrabData;
        [SerializeField] private WallJumpSO wallJumpData;
        [SerializeField] private JumpSO jumpData;
        [SerializeField] private ClimbSO climbData;
        [SerializeField] private MoveSO moveData;
        [SerializeField] private FallSO fallData;
        
        
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
        
        public MoveSO MoveData => moveData;
        public FallSO FallData => fallData;
        public JumpSO JumpData => jumpData;
        public Vector2 MoveInput { get; protected set; }
        public float MovementSpeed => moveData != null ? moveData.movementSpeed : 0f;
        public float JumpForce => jumpData != null ? jumpData.jumpForce : 0f;
        public float AirControlFactor => jumpData != null ? jumpData.airControlFactor : 1f;
        public float DashForce => dashData != null ? dashData.dashSpeed : 0f;
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
        public DashSO DashData => dashData;
        
        public bool IsTouchingWall { get; private set; }
        public bool IsWallSliding { get;  set; }
        public float LastWallTouchTime { get;  set; }
        public int WallSide { get;  set; } // -1 for left, 1 for right
        public bool IsWallGrabInput => (WallSide == 1 && MoveInput.x > 0.1f) || 
                                       (WallSide == -1 && MoveInput.x < -0.1f);

        public int MaxJumps => jumpData.MaxJumps;
        public float DoubleJumpForceMultiplier => jumpData.doubleJumpForceMultiplier;
        
        public bool IsTouchingLadder => _isTouchingLadder;
        public bool CanClimb => _isTouchingLadder && Mathf.Abs(_climbInput) > 0.1f;
        public ClimbSO ClimbData => climbData;
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
            Collider = GetComponent<Collider2D>();
            StateMachine = GetComponent<StateMachine>();
            AnimationController = GetComponent<CoreAnimation>();

            // Initialize states
            //Persistant States
            DeathState = new DeathState(this, StateMachine, deathAnimationName);
            IdleState = new IdleState(this, StateMachine, idleAnimationName);
            
            //Optional States
            if(moveData!= null)
                MovingState = new MovingState(this, StateMachine, moveAnimationName);
            if(jumpData!= null)
                JumpingState = new JumpingState(this, StateMachine, jumpAnimationName);
            if(dashData!= null)
                DashState = new DashState(this, StateMachine, dashAnimationName, DashData);
            if(wallGrabData!= null)
                WallGrabState = new WallGrabState(this, StateMachine, wallGrabAnimationName, wallGrabData);
            if(wallJumpData!= null)
                WallJumpState = new WallJumpState(this, StateMachine, wallJumpAnimationName, wallJumpData);
            if(climbData!= null)
                ClimbState = new ClimbState(this, StateMachine, climbIdleAnimationName, climbDownAnimationName,
                climbUpAnimationName, climbData);

            if (fallData != null && fallData.gravityScale != 0)
            {
                FallingState = new FallingState(this, StateMachine, fallAnimationName);
                Rigidbody.gravityScale = fallData.gravityScale;
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
            Debug.Log($"[CharacterController] Scale updated to: {transform.localScale}");
        }

        protected virtual void FixedUpdate()
        {
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
            if (_dashCooldownTimer > 0f || !DashInputPressed) return ;
    
            _dashCooldownTimer = dashData.cooldown;
            IsDashing = true;
            StateMachine.ChangeState(DashState);
        }
        
        
        private void CheckWallCollision()
        {
            if (wallGrabData == null) return;

            IsTouchingWall = false;
            
            var checkPosition = (Vector2)transform.position + 
                                new Vector2(
                                    wallGrabData.wallCheckOffset.x * transform.localScale.x, 
                                    wallGrabData.wallCheckOffset.y
                                );

            var wallHit = Physics2D.OverlapBox(
                checkPosition, 
                wallGrabData.wallCheckSize, 
                0f, 
                wallGrabData.wallLayer
            );

            if (wallHit == null) return;
            IsTouchingWall = true;
            LastWallTouchTime = Time.time;
            WallSide = wallHit.transform.position.x > transform.position.x ? 1 : -1;
        }
        
       public bool CanWallGrab()
        {
            if (wallGrabData == null) return false;
            
            return IsTouchingWall && 
                   !IsGrounded && 
                   IsWallGrabInput && 
                   Time.time < LastWallTouchTime + wallGrabData.wallGrabTime;
        }
        
        public bool CanWallJump()
        {
            if (wallJumpData == null) return false;
            
            // Note: We check JumpInputPressed directly from the property
            return (IsTouchingWall || Time.time < LastWallTouchTime + wallJumpData.coyoteWallTime) && 
                   !IsGrounded && 
                   JumpInputPressed;
        }
        
        private void CheckLadder()
        {
            if (climbData == null) return;

            var hit = Physics2D.OverlapBox(
                transform.position,
                climbData.ladderCheckSize,
                0f,
                climbData.climbableLayer
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
            if (wallGrabData != null)
            {
                Gizmos.color = IsTouchingWall ? Color.green : Color.yellow;
                Vector2 checkPosition = (Vector2)transform.position + 
                                        new Vector2(
                                            wallGrabData.wallCheckOffset.x * transform.localScale.x, 
                                            wallGrabData.wallCheckOffset.y
                                        );
                Gizmos.DrawWireCube(checkPosition, wallGrabData.wallCheckSize);
            }
            
            // Draw Ladder Check
            if (climbData != null)
            {
                Gizmos.color = _isTouchingLadder ? Color.green : Color.yellow;
                Gizmos.DrawWireCube(transform.position, climbData.ladderCheckSize);
            }
        }
        
        protected void HandleCharacterDeath()
        {
            StateMachine.ChangeState(DeathState);
        }
    }
}