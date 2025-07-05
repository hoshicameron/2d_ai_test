using PetalOfHope.Gameplay.Character;
using UnityEngine;
using PetalsOfHope.Core.StateMachine;

namespace PetalsOfHope.Gameplay.States
{
    public class IdleState : BaseState
    {
        private readonly CharacterControllerBase _characterController;
        private readonly string _idleAnimationName;

        public IdleState(CharacterControllerBase characterController, StateMachine stateMachine, string idleAnimationName) 
            : base(stateMachine)
        {
            _characterController = characterController;
            _idleAnimationName = idleAnimationName;
        }

        public override void Enter()
        {
            _characterController.CurrentStateName = _stateMachine.CurrentState.GetType().Name;
            _characterController.AnimationController.Play(_idleAnimationName);
            
            if (_characterController.Rigidbody.linearVelocity.x != 0)
            {
                _characterController.Rigidbody.linearVelocity = new Vector2(0, _characterController.Rigidbody.linearVelocity.y);
            }
            _characterController.ResetJumpInputFlags();
        }

        public override void Exit()
        {
            // Clean up any state-specific logic here when exiting IdleState
        }

        public override void Update()
        {
            if (_characterController.CanClimb && Mathf.Abs(_characterController.ClimbInput) > 0f)
            {
                _stateMachine.ChangeState(_characterController.ClimbState);
                return;
            }
            
            if (_characterController.JumpInputPressed && _characterController.IsGrounded)
            {
                _stateMachine.ChangeState(_characterController.JumpingState);
                return;
            }

            if (Mathf.Abs(_characterController.MoveInput.x) > Mathf.Epsilon)
            {
                _stateMachine.ChangeState(_characterController.MovingState);
                return;
            }

            if (!_characterController.IsGrounded)
            {
                _stateMachine.ChangeState(_characterController.FallingState);
            }
        }

        public override void FixedUpdate()
        {
            
        }
    }
}
