using PetalOfHope.Gameplay.Character;
using UnityEngine;
using PetalsOfHope.Core.StateMachine;
using PetalsOfHope.Data.Abilities.Types;

namespace PetalsOfHope.Gameplay.States
{
    public class DashState : BaseState
    {
        private readonly CharacterControllerBase _characterController;
        private readonly string _dashAnimationName;
        private readonly DashSO _dashData;
        private float _dashTimer;
        private Vector2 _dashDirection;

        public DashState(CharacterControllerBase characterController, StateMachine stateMachine, string dashAnimationName, DashSO dashData) 
            : base(stateMachine)
        {
            _characterController = characterController;
            _dashAnimationName = dashAnimationName;
            _dashData = dashData;
        }

        public override void Enter()
        {
            _characterController.CurrentStateName = _stateMachine.CurrentState.GetType().Name;
            _characterController.AnimationController.Play(_dashAnimationName);
            _dashTimer = _dashData.dashDuration;
            
            // Determine dash direction based on input or facing direction
            _dashDirection = _characterController.MoveInput.magnitude > 0.1f 
                ? _characterController.MoveInput.normalized 
                : Vector2.right * Mathf.Sign(_characterController.transform.localScale.x);
                
            // Apply initial dash force
            _characterController.Rigidbody.linearVelocity = _dashDirection * _dashData.dashSpeed;
            
            // Trigger any dash start events
            _characterController.OnDashStart?.Invoke();
            
            _characterController.ResetJumpInputFlags();
        }

        public override void Exit()
        {
            // Reset velocity when exiting dash to prevent sliding
            if (_characterController.Rigidbody != null)
            {
                _characterController.Rigidbody.linearVelocity = new Vector2(
                    _characterController.Rigidbody.linearVelocity.x * 0.5f, // Reduce horizontal velocity
                    _characterController.Rigidbody.linearVelocity.y);
            }

            _characterController.IsDashing = false;
            _characterController.OnDashEnd?.Invoke();
        }

        public override void Update()
        {
            _dashTimer -= Time.deltaTime;
            
            if (_dashTimer <= 0f)
            {
                // Return to appropriate state based on conditions
                if (!_characterController.IsGrounded)
                {
                    _stateMachine.ChangeState(_characterController.FallingState);
                }
                else if (Mathf.Abs(_characterController.MoveInput.x) > 0.1f)
                {
                    _stateMachine.ChangeState(_characterController.MovingState);
                }
                else
                {
                    _stateMachine.ChangeState(_characterController.IdleState);
                }
            }
        }

        public override void FixedUpdate()
        {
            // Maintain dash velocity during the dash
            if (_characterController.Rigidbody != null)
            {
                _characterController.Rigidbody.linearVelocity = _dashDirection * _dashData.dashSpeed;
            }
        }
    }
}
