using PetalOfHope.Gameplay.Character;
using PetalsOfHope.Core.StateMachine;
using PetalsOfHope.Data.Abilities.Types;
using UnityEngine;

namespace PetalsOfHope.Gameplay.States
{
    public class ClimbState : BaseState
    {
        private readonly CharacterControllerBase _characterController;
        private readonly string _climbIdleAnimationName;
        private readonly string _climbDownAnimationName;
        private readonly string _climbUpAnimationName;
        private readonly ClimbSO _climbData;
        private float _verticalVelocity;
        private bool _isAtTop;
        private bool _isAtBottom;
        
        

        public ClimbState(CharacterControllerBase characterController, StateMachine stateMachine, 
            string climbIdleAnimationName, string climbDownAnimationName, string climbUpAnimationName,
            ClimbSO climbData)
            : base(stateMachine)
        {
            _characterController = characterController;
            _climbIdleAnimationName = climbIdleAnimationName;
            _climbDownAnimationName = climbDownAnimationName;
            _climbUpAnimationName = climbUpAnimationName;
            _climbData = climbData;
        }

        public override void Enter()
        {
            _characterController.CurrentStateName = _stateMachine.CurrentState.GetType().Name;
            _characterController.AnimationController.Play(_climbIdleAnimationName);
            _characterController.Rigidbody.gravityScale = 0f;
            _characterController.Rigidbody.linearVelocity = Vector2.zero;
            SnapToLadder();
        }

        public override void Update()
        {
            // Check for jump input to dismount
            if (_characterController.JumpInputPressed)
            {
                _stateMachine.ChangeState(_characterController.JumpingState);
                return;
            }

            // Check if still on ladder
            if (!_characterController.IsTouchingLadder || _characterController.CurrentLadder == null)
            {
                _stateMachine.ChangeState(_characterController.FallingState);
                return;
            }

            // Check for ground when moving down at bottom of ladder
            if (_characterController.IsGrounded && _characterController.ClimbInput < -0.1f)
            {
                _stateMachine.ChangeState(_characterController.IdleState);
                return;
            }

            UpdateClimbing();
        }

        public override void FixedUpdate()
        {
            // Apply vertical movement
            var targetVelocity = new Vector2(
                _characterController.Rigidbody.linearVelocity.x,
                _verticalVelocity * _climbData.climbSpeed
            );

            _characterController.Rigidbody.linearVelocity = targetVelocity;
        }

        public override void Exit()
        {
            _characterController.Rigidbody.gravityScale = _characterController.Stats.gravityScale;
            _verticalVelocity = 0f;
        }

        private void SnapToLadder()
        {
            if (_characterController.CurrentLadder == null) return;

            var position = _characterController.transform.position;
            var ladderPosition = _characterController.CurrentLadder.transform.position.x;
            position.x = Mathf.Lerp(position.x, ladderPosition, _climbData.horizontalSnapDistance);
            _characterController.transform.position = position;
        }

        private void UpdateClimbing()
        {
            // Get raw input for more responsive controls
            float targetVelocity = _characterController.ClimbInput;

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
                _characterController.AnimationController.Play(Mathf.Sign(_verticalVelocity) > 0 ?
                    _climbUpAnimationName : _climbDownAnimationName);
            }
            else
            {
                _characterController.AnimationController.Play(_climbIdleAnimationName);
            }
        }
    }

}