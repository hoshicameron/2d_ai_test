using UnityEngine;
using PetalsOfHope.Core.StateMachine;
using PetalsOfHope.Data.Abilities.Types;

namespace PetalsOfHope.Gameplay.Player.States
{
    public class WallJumpState : BaseState
    {
        private readonly PlayerController _player;
        private readonly string _animationName;
        private readonly WallJumpSO _wallJumpData;
        private float _wallJumpStartTime;
        private bool _hasWallJumped;
        private int _wallJumpDirection;
        private bool _canMove;

        public WallJumpState(PlayerController player, StateMachine stateMachine, string animationName, WallJumpSO wallJumpData) 
            : base(stateMachine)
        {
            _player = player;
            _animationName = animationName;
            _wallJumpData = wallJumpData;
        }

        public override void Enter()
        {
            _wallJumpStartTime = Time.time;
            _hasWallJumped = false;
            _canMove = false;
            _wallJumpDirection = -_player.WallSide; // Jump away from wall
            
            _player.AnimationController.Play(_animationName);
            _player.OnWallJump?.Invoke();
        }

        public override void Exit()
        {
            _player.ResetJumpInputFlags();
        }

        public override void Update()
        {
            // Allow movement after a short delay
            if (!_canMove && Time.time > _wallJumpStartTime + _wallJumpData.wallJumpInputDisableTime)
            {
                _canMove = true;
            }

            // Check if we should transition to falling
            if (_player.Rigidbody != null && _player.Rigidbody.linearVelocity.y <= 0)
            {
                _stateMachine.ChangeState(_player.FallingState);
                return;
            }

            // If we somehow touch the ground
            if (_player.IsGrounded)
            {
                _stateMachine.ChangeState(Mathf.Abs(_player.MoveInput.x) > 0.1f ? 
                    _player.MovingState : _player.IdleState);
            }
        }

        public override void FixedUpdate()
        {
            if (_player.Rigidbody == null) return;

            // Apply wall jump force on first FixedUpdate
            if (!_hasWallJumped)
            {
                _hasWallJumped = true;
                var jumpForce = new Vector2(
                    _wallJumpDirection * _wallJumpData.wallJumpHorizontalForce,
                    _wallJumpData.wallJumpForce
                );
                _player.Rigidbody.linearVelocity = jumpForce;
            }
            // Apply movement input after the initial jump
            else if (_canMove)
            {
                var velocity = _player.Rigidbody.linearVelocity;
                velocity.x = _player.MoveInput.x * _player.Stats.movementSpeed;
                _player.Rigidbody.linearVelocity = velocity;
            }
        }
    }
}
