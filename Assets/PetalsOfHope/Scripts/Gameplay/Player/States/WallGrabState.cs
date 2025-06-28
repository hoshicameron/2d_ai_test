using _Project.Scripts.Gameplay.Character;
using UnityEngine;
using PetalsOfHope.Core.StateMachine;
using PetalsOfHope.Data.Abilities.Types;

namespace PetalsOfHope.Gameplay.Player.States
{
    public class WallGrabState : BaseState
    {
        private readonly CharacterControllerBase _characterController;
        private readonly string _animationName;
        private readonly WallGrabSO _wallGrabData;
        private float _wallGrabStartTime;
        private bool _isWallSliding;
        private int _wallSide;

        public WallGrabState(CharacterControllerBase characterController, StateMachine stateMachine, string animationName, WallGrabSO wallGrabData) 
            : base(stateMachine)
        {
            _characterController = characterController;
            _animationName = animationName;
            _wallGrabData = wallGrabData;
        }

        public override void Enter()
        {
            _wallGrabStartTime = Time.time;
            _isWallSliding = false;
            _wallSide = _characterController.WallSide;
            _characterController.AnimationController.Play(_animationName);
            _characterController.OnWallGrabStart?.Invoke();
        }

        public override void Exit()
        {
            _characterController.IsWallSliding = false;
            _characterController.OnWallGrabEnd?.Invoke();
            _characterController.ResetJumpInputFlags();
        }

        public override void Update()
        {
            // Check if we should start wall sliding
            if (!_isWallSliding && Time.time > _wallGrabStartTime + _wallGrabData.wallGrabTime)
            {
                _isWallSliding = true;
                _characterController.IsWallSliding = true;
            }

            // Check for wall jump input
            if (_characterController.JumpInputPressed)
            {
                _stateMachine.ChangeState(_characterController.WallJumpState);
                return;
            }

            // Check if we should release from wall
            if (!_characterController.IsTouchingWall || !_characterController.IsWallGrabInput || _characterController.IsGrounded)
            {
                _stateMachine.ChangeState(_characterController.FallingState);
                return;
            }
        }

        public override void FixedUpdate()
        {
            if (_characterController.Rigidbody == null) return;

            // Apply wall slide velocity if sliding
            if (_isWallSliding)
            {
                var velocity = _characterController.Rigidbody.linearVelocity;
                velocity.y = Mathf.Max(velocity.y, -_wallGrabData.wallSlideSpeed);
                _characterController.Rigidbody.linearVelocity = velocity;
            }
            else
            {
                // Stick to wall while grabbing
                var velocity = _characterController.Rigidbody.linearVelocity;
                velocity.y = 0f;
                _characterController.Rigidbody.linearVelocity = velocity;
            }
        }
    }
}
