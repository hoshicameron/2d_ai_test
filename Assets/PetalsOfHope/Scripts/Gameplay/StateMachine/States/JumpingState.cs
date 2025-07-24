using PetalOfHope.Gameplay.Character;
using UnityEngine;
using PetalsOfHope.Gameplay.StateMachine;

namespace PetalsOfHope.Gameplay.States
{
    public class JumpingState : BaseState
    {
        private readonly CharacterControllerBase _characterController;
        private readonly string _jumpAnimationName;
        private bool _jumpCutoffApplied = false;

        public JumpingState(CharacterControllerBase characterController, StateMachine.StateMachine stateMachine, string jumpAnimationName) 
            : base(stateMachine)
        {
            _characterController = characterController;
            _jumpAnimationName = jumpAnimationName;
        }

        public override void Enter()
        {
            _characterController.CurrentStateName = _stateMachine.CurrentState.GetType().Name;
            _characterController.AnimationController.Play(_jumpAnimationName);
            
            // Calculate jump force (normal jump or double jump)
            var isDoubleJump = _characterController.RemainingJumps < _characterController.AbilitySheetData.jumpData.maxJumps;
            var  jumpForce = isDoubleJump ? _characterController.AbilitySheetData.jumpData.jumpForce * _characterController.AbilitySheetData.jumpData.doubleJumpForceMultiplier 
                                            : _characterController.AbilitySheetData.jumpData.jumpForce;
            
            // Apply jump force
            _characterController.Rigidbody.linearVelocity = new Vector2(_characterController.Rigidbody.linearVelocity.x, 0f);
            _characterController.Rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            
            // Consume a jump
            _characterController.RemainingJumps--;
            
            _characterController.ResetJumpInputFlags();
            _jumpCutoffApplied = false;
        }

        public override void Exit()
        {
            // Clean up any state-specific logic here when exiting JumpingState
            _jumpCutoffApplied = false;
            _characterController.ResetJumpInputFlags();
        }

        public override void Update()
        {
            if (_characterController.FallingState != null && _characterController.Rigidbody.linearVelocity.y <= 0f)
            {
                _stateMachine.ChangeState(_characterController.FallingState);
                return;
            }

            if (_characterController.JumpInputReleased && !_jumpCutoffApplied)
            {
                if (_characterController.Rigidbody.linearVelocity.y > 0)
                {
                    _characterController.Rigidbody.linearVelocity = new Vector2(_characterController.Rigidbody.linearVelocity.x, _characterController.Rigidbody.linearVelocity.y * 0.5f);
                }
                _jumpCutoffApplied = true;
                _characterController.ResetJumpInputFlags();
            }

            FlipSprite();
        }

        public override void FixedUpdate()
        {
            float airControlFactor = _characterController.AbilitySheetData.jumpData.airControlFactor;
            float targetVelocityX = _characterController.MoveInput.x * _characterController.AbilitySheetData.moveData.movementSpeed * airControlFactor;
            _characterController.Rigidbody.linearVelocity = new Vector2(targetVelocityX, _characterController.Rigidbody.linearVelocity.y);
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
