using PetalOfHope.Gameplay.Character;
using UnityEngine;
using PetalsOfHope.Core.StateMachine;

namespace PetalsOfHope.Gameplay.States
{
    public class FallingState : BaseState
    {
        private readonly CharacterControllerBase _characterController;
        private readonly string _fallAnimationName;

        public FallingState(CharacterControllerBase characterController, StateMachine stateMachine, string fallAnimationName) 
            : base(stateMachine)
        {
            _characterController = characterController;
            _fallAnimationName = fallAnimationName;
        }

        public override void Enter()
        {
            _characterController.CurrentStateName = _stateMachine.CurrentState.GetType().Name;
            _characterController.AnimationController.Play(_fallAnimationName);
            _characterController.ResetJumpInputFlags();
        }

        public override void Update()
        {
            
            if (_characterController.CanClimb && Mathf.Abs(_characterController.ClimbInput) > 0f)
            {
                _stateMachine.ChangeState(_characterController.ClimbState);
                return;
            }
            
            // Check for double jump input
            if (_characterController.JumpInputPressed && _characterController.RemainingJumps > 0)
            {
                _stateMachine.ChangeState(_characterController.JumpingState);
                return;
            }

            // Check for wall grab transition (only when falling or at apex)
            if (_characterController.CanWallGrab())
            {
                _stateMachine.ChangeState(_characterController.WallGrabState);
                return;
            }

            // Check for wall jump from falling (coyote wall jump)
            if (_characterController.CanWallJump())
            {
                _stateMachine.ChangeState(_characterController.WallJumpState);
                return;
            }

            // Handle normal ground landing
            if (_characterController.IsGrounded)
            {
                if (Mathf.Abs(_characterController.MoveInput.x) > Mathf.Epsilon)
                {
                    _stateMachine.ChangeState(_characterController.MovingState);
                }
                else
                {
                    _stateMachine.ChangeState(_characterController.IdleState);
                }
                return;
            }
            
            FlipSprite();
        }

        public override void FixedUpdate()
        {
            float airControlFactor = _characterController.Stats.airControlFactor;
            float targetVelocityX = _characterController.MoveInput.x * _characterController.Stats.movementSpeed * airControlFactor;
            _characterController.Rigidbody.linearVelocity = new Vector2(targetVelocityX, _characterController.Rigidbody.linearVelocity.y);
        }

        public override void Exit()
        {
            _characterController.ResetJumpInputFlags();
        }
        
        private void FlipSprite()
        {
            if (Mathf.Abs(_characterController.MoveInput.x) > 0.01f)
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
}
