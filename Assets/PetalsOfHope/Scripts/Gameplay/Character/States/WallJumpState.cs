using PetalOfHope.Gameplay.Character;
using UnityEngine;
using PetalsOfHope.Core.StateMachine;
using PetalsOfHope.Data.Abilities;

namespace PetalsOfHope.Gameplay.States
{
    public class WallJumpState : BaseState
    {
        private readonly CharacterControllerBase _characterController;
        private readonly string _animationName;
        private readonly WallJumpData _wallJumpData;
        private float _wallJumpStartTime;
        private bool _hasWallJumped;
        private int _wallJumpDirection;
        private bool _canMove;

        public WallJumpState(CharacterControllerBase characterController, StateMachine stateMachine, string animationName, WallJumpData wallJumpData) 
            : base(stateMachine)
        {
            _characterController = characterController;
            _animationName = animationName;
            _wallJumpData = wallJumpData;
        }

        public override void Enter()
        {
            _wallJumpStartTime = Time.time;
            _hasWallJumped = false;
            _canMove = false;
            _wallJumpDirection = -_characterController.WallSide; // Jump away from wall
            
            _characterController.AnimationController.Play(_animationName);
            _characterController.OnWallJump?.Invoke();
        }

        public override void Exit()
        {
            _characterController.ResetJumpInputFlags();
        }

        public override void Update()
        {
            // Allow movement after a short delay
            if (!_canMove && Time.time > _wallJumpStartTime + _wallJumpData.wallJumpInputDisableTime)
            {
                _canMove = true;
            }

            // Check if we should transition to falling
            if (_characterController.FallingState != null && 
                (_characterController.Rigidbody != null && _characterController.Rigidbody.linearVelocity.y <= 0))
            {
                _stateMachine.ChangeState(_characterController.FallingState);
                return;
            }

            // If we somehow touch the ground
            if (_characterController.IsGrounded)
            {
                if (_characterController.MovingState != null && Mathf.Abs(_characterController.MoveInput.x) > 0.1f)
                {
                    _stateMachine.ChangeState(_characterController.MovingState);
                    return;
                }
                _stateMachine.ChangeState(_characterController.IdleState);
            }
        }

        public override void FixedUpdate()
        {
            if (_characterController.Rigidbody == null) return;

            // Apply wall jump force on first FixedUpdate
            if (!_hasWallJumped)
            {
                _hasWallJumped = true;
                var jumpForce = new Vector2(
                    _wallJumpDirection * _wallJumpData.wallJumpHorizontalForce,
                    _wallJumpData.wallJumpForce
                );
                _characterController.Rigidbody.linearVelocity = jumpForce;
            }
            // Apply movement input after the initial jump
            else if (_canMove)
            {
                var velocity = _characterController.Rigidbody.linearVelocity;
                velocity.x = _characterController.MoveInput.x * _characterController.AbilitySheetData.moveData.movementSpeed;
                _characterController.Rigidbody.linearVelocity = velocity;
            }
        }
    }
}
