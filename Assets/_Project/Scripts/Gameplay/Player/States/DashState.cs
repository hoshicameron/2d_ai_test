using UnityEngine;
using PetalsOfHope.Core.StateMachine;
using PetalsOfHope.Data.Abilities.Types;

namespace PetalsOfHope.Gameplay.Player.States
{
    public class DashState : BaseState
    {
        private readonly PlayerController _player;
        private readonly string _dashAnimationName;
        private readonly DashSO _dashData;
        private float _dashTimer;
        private Vector2 _dashDirection;

        public DashState(PlayerController player, StateMachine stateMachine, string dashAnimationName, DashSO dashData) 
            : base(stateMachine)
        {
            _player = player;
            _dashAnimationName = dashAnimationName;
            _dashData = dashData;
        }

        public override void Enter()
        {
            _player.CurrentStateName = _stateMachine.CurrentState.GetType().Name;
            _player.AnimationController.Play(_dashAnimationName);
            _dashTimer = _dashData.dashDuration;
            
            // Determine dash direction based on input or facing direction
            _dashDirection = _player.MoveInput.magnitude > 0.1f 
                ? _player.MoveInput.normalized 
                : Vector2.right * Mathf.Sign(_player.transform.localScale.x);
                
            // Apply initial dash force
            _player.Rigidbody.linearVelocity = _dashDirection * _dashData.dashSpeed;
            
            // Trigger any dash start events
            _player.OnDashStart?.Invoke();
        }

        public override void Exit()
        {
            // Reset velocity when exiting dash to prevent sliding
            if (_player.Rigidbody != null)
            {
                _player.Rigidbody.linearVelocity = new Vector2(
                    _player.Rigidbody.linearVelocity.x * 0.5f, // Reduce horizontal velocity
                    _player.Rigidbody.linearVelocity.y);
            }

            _player.IsDashing = false;
            _player.OnDashEnd?.Invoke();
        }

        public override void Update()
        {
            _dashTimer -= Time.deltaTime;
            
            if (_dashTimer <= 0f)
            {
                // Return to appropriate state based on conditions
                if (!_player.IsGrounded)
                {
                    _stateMachine.ChangeState(_player.FallingState);
                }
                else if (Mathf.Abs(_player.MoveInput.x) > 0.1f)
                {
                    _stateMachine.ChangeState(_player.MovingState);
                }
                else
                {
                    _stateMachine.ChangeState(_player.IdleState);
                }
            }
        }

        public override void FixedUpdate()
        {
            // Maintain dash velocity during the dash
            if (_player.Rigidbody != null)
            {
                _player.Rigidbody.linearVelocity = _dashDirection * _dashData.dashSpeed;
            }
        }
    }
}
