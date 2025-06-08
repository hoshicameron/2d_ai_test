using PetalsOfHope.Core.StateMachine;
using PetalsOfHope.Data.Abilities.Types;
using UnityEngine;

namespace PetalsOfHope.Gameplay.Player.States
{
    public class ClimbState : BaseState
    {
        private readonly PlayerController _player;
        private readonly string _climbIdleAnimationName;
        private readonly string _climbDownAnimationName;
        private readonly string _climbUpAnimationName;
        private readonly ClimbSO _climbData;
        private float _verticalVelocity;
        private bool _isAtTop;
        private bool _isAtBottom;
        
        

        public ClimbState(PlayerController player, StateMachine stateMachine, 
            string climbIdleAnimationName, string climbDownAnimationName, string climbUpAnimationName,
            ClimbSO climbData)
            : base(stateMachine)
        {
            _player = player;
            _climbIdleAnimationName = climbIdleAnimationName;
            _climbDownAnimationName = climbDownAnimationName;
            _climbUpAnimationName = climbUpAnimationName;
            _climbData = climbData;
        }

        public override void Enter()
        {
            _player.CurrentStateName = _stateMachine.CurrentState.GetType().Name;
            _player.AnimationController.Play(_climbIdleAnimationName);
            _player.Rigidbody.gravityScale = 0f;
            _player.Rigidbody.linearVelocity = Vector2.zero;
            SnapToLadder();
        }

        public override void Update()
        {
            // Check for jump input to dismount
            if (_player.JumpInputPressed)
            {
                _stateMachine.ChangeState(_player.JumpingState);
                return;
            }

            // Check if still on ladder
            if (!_player.IsTouchingLadder || _player.CurrentLadder == null)
            {
                _stateMachine.ChangeState(_player.FallingState);
                return;
            }

            // Check for ground when moving down at bottom of ladder
            if (_player.IsGrounded && _player.ClimbInput < -0.1f)
            {
                _stateMachine.ChangeState(_player.IdleState);
                return;
            }

            UpdateClimbing();
        }

        public override void FixedUpdate()
        {
            // Apply vertical movement
            var targetVelocity = new Vector2(
                _player.Rigidbody.linearVelocity.x,
                _verticalVelocity * _climbData.climbSpeed
            );

            _player.Rigidbody.linearVelocity = targetVelocity;
        }

        public override void Exit()
        {
            _player.Rigidbody.gravityScale = _player.Stats.gravityScale;
            _verticalVelocity = 0f;
        }

        private void SnapToLadder()
        {
            if (_player.CurrentLadder == null) return;

            var position = _player.transform.position;
            var ladderPosition = _player.CurrentLadder.transform.position.x;
            position.x = Mathf.Lerp(position.x, ladderPosition, _climbData.horizontalSnapDistance);
            _player.transform.position = position;
        }

        private void UpdateClimbing()
        {
            // Get raw input for more responsive controls
            float targetVelocity = _player.ClimbInput;

            // Apply acceleration/deceleration
            if (Mathf.Abs(targetVelocity) > 0.1f)
            {
                _verticalVelocity = Mathf.MoveTowards(
                    _verticalVelocity,
                    targetVelocity,
                    _climbData.climbAcceleration * Time.deltaTime
                );
            }
            else
            {
                _verticalVelocity = Mathf.MoveTowards(
                    _verticalVelocity,
                    0f,
                    _climbData.climbDeceleration * Time.deltaTime
                );
            }

            // Update animation based on movement
            if (Mathf.Abs(_verticalVelocity) > 0.1f)
            {
                _player.AnimationController.Play(Mathf.Sign(_verticalVelocity) > 0 ?
                    _climbUpAnimationName : _climbDownAnimationName);
            }
            else
            {
                _player.AnimationController.Play(_climbIdleAnimationName);
            }
        }
    }

}