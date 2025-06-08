using UnityEngine;
using PetalsOfHope.Core.StateMachine;

namespace PetalsOfHope.Gameplay.Player.States
{
    public class FallingState : BaseState
    {
        private readonly PlayerController _player;
        private readonly string _fallAnimationName;

        public FallingState(PlayerController player, StateMachine stateMachine, string fallAnimationName) 
            : base(stateMachine)
        {
            _player = player;
            _fallAnimationName = fallAnimationName;
        }

        public override void Enter()
        {
            _player.CurrentStateName = _stateMachine.CurrentState.GetType().Name;
            _player.AnimationController.Play(_fallAnimationName);
            _player.ResetJumpInputFlags();
        }

        public override void Update()
        {
            // Check for double jump input
            if (_player.JumpInputPressed && _player.RemainingJumps > 0)
            {
                _stateMachine.ChangeState(_player.JumpingState);
                return;
            }

            // Check for wall grab transition (only when falling or at apex)
            if (_player.CanWallGrab())
            {
                _stateMachine.ChangeState(_player.WallGrabState);
                return;
            }

            // Check for wall jump from falling (coyote wall jump)
            if (_player.CanWallJump())
            {
                _stateMachine.ChangeState(_player.WallJumpState);
                return;
            }

            // Handle normal ground landing
            if (_player.IsGrounded)
            {
                if (Mathf.Abs(_player.MoveInput.x) > Mathf.Epsilon)
                {
                    _stateMachine.ChangeState(_player.MovingState);
                }
                else
                {
                    _stateMachine.ChangeState(_player.IdleState);
                }
                return;
            }
            
            FlipSprite();
        }

        public override void FixedUpdate()
        {
            float airControlFactor = _player.Stats.airControlFactor;
            float targetVelocityX = _player.MoveInput.x * _player.Stats.movementSpeed * airControlFactor;
            _player.Rigidbody.linearVelocity = new Vector2(targetVelocityX, _player.Rigidbody.linearVelocity.y);
        }

        public override void Exit()
        {
            _player.ResetJumpInputFlags();
        }
        
        private void FlipSprite()
        {
            if (Mathf.Abs(_player.MoveInput.x) > 0.01f)
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
}
