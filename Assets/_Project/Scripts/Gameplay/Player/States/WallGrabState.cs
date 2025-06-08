using UnityEngine;
using PetalsOfHope.Core.StateMachine;
using PetalsOfHope.Data.Abilities.Types;

namespace PetalsOfHope.Gameplay.Player.States
{
    public class WallGrabState : BaseState
    {
        private readonly PlayerController _player;
        private readonly string _animationName;
        private readonly WallGrabSO _wallGrabData;
        private float _wallGrabStartTime;
        private bool _isWallSliding;
        private int _wallSide;

        public WallGrabState(PlayerController player, StateMachine stateMachine, string animationName, WallGrabSO wallGrabData) 
            : base(stateMachine)
        {
            _player = player;
            _animationName = animationName;
            _wallGrabData = wallGrabData;
        }

        public override void Enter()
        {
            _wallGrabStartTime = Time.time;
            _isWallSliding = false;
            _wallSide = _player.WallSide;
            _player.AnimationController.Play(_animationName);
            _player.OnWallGrabStart?.Invoke();
        }

        public override void Exit()
        {
            _player.IsWallSliding = false;
            _player.OnWallGrabEnd?.Invoke();
            _player.ResetJumpInputFlags();
        }

        public override void Update()
        {
            // Check if we should start wall sliding
            if (!_isWallSliding && Time.time > _wallGrabStartTime + _wallGrabData.wallGrabTime)
            {
                _isWallSliding = true;
                _player.IsWallSliding = true;
            }

            // Check for wall jump input
            if (_player.JumpInputPressed)
            {
                _stateMachine.ChangeState(_player.WallJumpState);
                return;
            }

            // Check if we should release from wall
            if (!_player.IsTouchingWall || !_player.IsWallGrabInput || _player.IsGrounded)
            {
                _stateMachine.ChangeState(_player.FallingState);
                return;
            }
        }

        public override void FixedUpdate()
        {
            if (_player.Rigidbody == null) return;

            // Apply wall slide velocity if sliding
            if (_isWallSliding)
            {
                var velocity = _player.Rigidbody.linearVelocity;
                velocity.y = Mathf.Max(velocity.y, -_wallGrabData.wallSlideSpeed);
                _player.Rigidbody.linearVelocity = velocity;
            }
            else
            {
                // Stick to wall while grabbing
                var velocity = _player.Rigidbody.linearVelocity;
                velocity.y = 0f;
                _player.Rigidbody.linearVelocity = velocity;
            }
        }
    }
}
