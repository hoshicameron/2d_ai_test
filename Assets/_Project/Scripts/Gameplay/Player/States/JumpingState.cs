using UnityEngine;
using PetalsOfHope.Core.StateMachine;

namespace PetalsOfHope.Gameplay.Player.States
{
    public class JumpingState : BaseState
    {
        private readonly PlayerController _player;
        private readonly string _jumpAnimationName;
        private bool _jumpCutoffApplied = false;

        public JumpingState(PlayerController player, StateMachine stateMachine, string jumpAnimationName) 
            : base(stateMachine)
        {
            _player = player;
            _jumpAnimationName = jumpAnimationName;
        }

        public override void Enter()
        {
            _player.AnimationController.Play(_jumpAnimationName);
            _player.Rigidbody.linearVelocity = new Vector2(_player.Rigidbody.linearVelocity.x, 0f);
            _player.Rigidbody.AddForce(Vector2.up * _player.Stats.jumpForce, ForceMode2D.Impulse);
            _player.ResetJumpInputFlags();
            _jumpCutoffApplied = false;
        }

        public override void Exit()
        {
            // Clean up any state-specific logic here when exiting JumpingState
            _jumpCutoffApplied = false;
            _player.ResetJumpInputFlags();
        }

        public override void Update()
        {
            if (_player.Rigidbody.linearVelocity.y <= 0f)
            {
                _stateMachine.ChangeState(_player.FallingState);
                return;
            }

            if (_player.JumpInputReleased && !_jumpCutoffApplied)
            {
                if (_player.Rigidbody.linearVelocity.y > 0)
                {
                    _player.Rigidbody.linearVelocity = new Vector2(_player.Rigidbody.linearVelocity.x, _player.Rigidbody.linearVelocity.y * 0.5f);
                }
                _jumpCutoffApplied = true;
                _player.ResetJumpInputFlags();
            }

            FlipSprite();
        }

        public override void FixedUpdate()
        {
            float airControlFactor = _player.Stats.airControlFactor;
            float targetVelocityX = _player.MoveInput.x * _player.Stats.movementSpeed * airControlFactor;
            _player.Rigidbody.linearVelocity = new Vector2(targetVelocityX, _player.Rigidbody.linearVelocity.y);
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
