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
        
        [Header("Dash")]
        [SerializeField] private DashSO _dashData;
        private float _dashCooldownTimer;
        public Action OnDashStart;
        public Action OnDashEnd;
        public bool IsDashing { get; set; }
        public DashSO DashData => _dashData;
        
        [Header("Animation")]
        [SerializeField] private string idleAnimationName = "idle";
        [SerializeField] private string jumpAnimationName = "jump";
        [SerializeField] private string fallAnimationName = "fall";
        [SerializeField] private string moveAnimationName = "move";
        [SerializeField] private string dashAnimationName = "dash";
        [SerializeField] private string deathAnimationName = "death";

        [field: SerializeField, ReadOnly]
        public string CurrentStateName { get; set; } = "None";
        
        // Properties
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

        // State Instances
        public IdleState IdleState { get; private set; }
        public MovingState MovingState { get; private set; }
        public JumpingState JumpingState { get; private set; }
        public FallingState FallingState { get; private set; }
        public DashState DashState { get; private set; }
        public DeathState DeathState { get; private set; }

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
            
            if (_dashCooldownTimer > 0f)
            {
                _dashCooldownTimer -= Time.deltaTime;
            }

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

        private void OnDrawGizmosSelected()
        {
            if (_groundCheckPoint != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(_groundCheckPoint.position, _groundCheckRadius);
            }
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
    }
}
