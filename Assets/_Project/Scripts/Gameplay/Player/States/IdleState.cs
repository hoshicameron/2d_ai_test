using UnityEngine;
using PetalsOfHope.Core.StateMachine;

namespace PetalsOfHope.Gameplay.Player.States
{
    public class IdleState : BaseState
    {
        private readonly PlayerController _player;
        private readonly string _idleAnimationName;

        public IdleState(PlayerController player, StateMachine stateMachine, string idleAnimationName) 
            : base(stateMachine)
        {
            _player = player;
            _idleAnimationName = idleAnimationName;
        }

        public override void Enter()
        {
            _player.CurrentStateName = _stateMachine.CurrentState.GetType().Name;
            _player.AnimationController.Play(_idleAnimationName);
            
            if (_player.Rigidbody.linearVelocity.x != 0)
            {
                _player.Rigidbody.linearVelocity = new Vector2(0, _player.Rigidbody.linearVelocity.y);
            }
            _player.ResetJumpInputFlags();
        }

        public override void Exit()
        {
            // Clean up any state-specific logic here when exiting IdleState
        }

        public override void Update()
        {
            if (_player.JumpInputPressed && _player.IsGrounded)
            {
                _stateMachine.ChangeState(_player.JumpingState);
                return;
            }

            if (Mathf.Abs(_player.MoveInput.x) > Mathf.Epsilon)
            {
                _stateMachine.ChangeState(_player.MovingState);
                return;
            }

            if (!_player.IsGrounded)
            {
                _stateMachine.ChangeState(_player.FallingState);
            }
        }

        public override void FixedUpdate()
        {
            
        }
    }
}
