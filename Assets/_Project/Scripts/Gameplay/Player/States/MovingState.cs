using UnityEngine;
using PetalsOfHope.Core.StateMachine;

namespace PetalsOfHope.Gameplay.Player.States
{
    public class MovingState : BaseState
    {
        private readonly PlayerController _player;
        private readonly string _moveAnimationName;

        public MovingState(PlayerController player, StateMachine stateMachine, string moveAnimationName) 
            : base(stateMachine)
        {
            _player = player;
            _moveAnimationName = moveAnimationName;
        }

        public override void Enter()
        {
            _player.CurrentStateName = _stateMachine.CurrentState.GetType().Name;
            _player.AnimationController.Play(_moveAnimationName);
            _player.ResetJumpInputFlags();
        }

        public override void Update()
        {
            if (_player.CanClimb && Mathf.Abs(_player.ClimbInput) > 0f)
            {
                _stateMachine.ChangeState(_player.ClimbState);
                return;
            }
            
            if (_player.JumpInputPressed && _player.IsGrounded)
            {
                _stateMachine.ChangeState(_player.JumpingState);
                return;
            }

            if (Mathf.Abs(_player.MoveInput.x) < Mathf.Epsilon)
            {
                _stateMachine.ChangeState(_player.IdleState);
                return;
            }

            if (!_player.IsGrounded)
            {
                _stateMachine.ChangeState(_player.FallingState);
                return;
            }

            FlipSprite();
        }

        public override void FixedUpdate()
        {
            float targetVelocityX = _player.MoveInput.x * _player.Stats.movementSpeed;
            _player.Rigidbody.linearVelocity = new Vector2(targetVelocityX, _player.Rigidbody.linearVelocity.y);
        }

        public override void Exit()
        {
            // Clean up any state-specific logic here when exiting MovingState
        }

        private void FlipSprite()
        {
            _player.transform.localScale = _player.MoveInput.x switch
            {
                > 0.01f when _player.transform.localScale.x < 0f => new Vector3(
                    Mathf.Abs(_player.transform.localScale.x), _player.transform.localScale.y,
                    _player.transform.localScale.z),
                < -0.01f when _player.transform.localScale.x > 0f => new Vector3(
                    -Mathf.Abs(_player.transform.localScale.x), _player.transform.localScale.y,
                    _player.transform.localScale.z),
                _ => _player.transform.localScale
            };
        }
    }
}
