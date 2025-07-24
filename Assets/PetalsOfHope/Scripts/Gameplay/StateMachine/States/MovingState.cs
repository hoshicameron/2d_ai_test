using PetalOfHope.Gameplay.Character;
using UnityEngine;
using PetalsOfHope.Gameplay.StateMachine;

namespace PetalsOfHope.Gameplay.States
{
    public class MovingState : BaseState
    {
        private readonly CharacterControllerBase _characterController;
        private readonly string _moveAnimationName;

        public MovingState(CharacterControllerBase characterController, StateMachine.StateMachine stateMachine, string moveAnimationName) 
            : base(stateMachine)
        {
            _characterController = characterController;
            _moveAnimationName = moveAnimationName;
        }

        public override void Enter()
        {
            _characterController.CurrentStateName = _stateMachine.CurrentState.GetType().Name;
            _characterController.AnimationController.Play(_moveAnimationName);
            _characterController.ResetJumpInputFlags();
        }

        public override void Update()
        {
            if (_characterController.ClimbState != null 
                && _characterController.CanClimb 
                && Mathf.Abs(_characterController.ClimbInput) > 0f)
            {
                _stateMachine.ChangeState(_characterController.ClimbState);
                return;
            }
            
            if (_characterController.JumpingState != null 
                && _characterController.JumpInputPressed 
                && _characterController.IsGrounded)
            {
                _stateMachine.ChangeState(_characterController.JumpingState);
                return;
            }

            if (Mathf.Abs(_characterController.MoveInput.x) < Mathf.Epsilon)
            {
                _stateMachine.ChangeState(_characterController.IdleState);
                return;
            }

            if (_characterController.FallingState != null && !_characterController.IsGrounded)
            {
                _stateMachine.ChangeState(_characterController.FallingState);
                return;
            }

            FlipSprite();
        }

        public override void FixedUpdate()
        {
            _characterController.Mover.Move(_characterController.MoveInput,
                _characterController.AbilitySheetData.moveData.movementSpeed);
        }

        public override void Exit()
        {
            // Clean up any state-specific logic here when exiting MovingState
        }

        private void FlipSprite()
        {
            _characterController.transform.localScale = _characterController.MoveInput.x switch
            {
                > 0.01f when _characterController.transform.localScale.x < 0f => new Vector3(
                    Mathf.Abs(_characterController.transform.localScale.x), _characterController.transform.localScale.y,
                    _characterController.transform.localScale.z),
                < -0.01f when _characterController.transform.localScale.x > 0f => new Vector3(
                    -Mathf.Abs(_characterController.transform.localScale.x), _characterController.transform.localScale.y,
                    _characterController.transform.localScale.z),
                _ => _characterController.transform.localScale
            };
        }
    }
}
