using PetalOfHope.Gameplay.Character;
using PetalsOfHope.Gameplay.StateMachine;
using UnityEngine;

namespace PetalsOfHope.Gameplay.States
{
    public class RespawnState : BaseState
    {
        private readonly CharacterControllerBase _characterController;

        public RespawnState(CharacterControllerBase characterController, StateMachine.StateMachine stateMachine) 
            : base(stateMachine)
        {
            _characterController = characterController;
        }

        public override void Enter()
        {
            // Re-enable components
            _characterController.enabled = true;
            var collider = _characterController.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = true;
            }

            // Reset Rigidbody
            _characterController.Rigidbody.bodyType = RigidbodyType2D.Dynamic;
            _characterController.Rigidbody.linearVelocity = Vector2.zero;

            // Re-enable input
            _characterController.InputSource.EnableGameplayInput();

            // Immediately transition to a neutral state
            _stateMachine.ChangeState(_characterController.IdleState);
        }

        public override void Exit() { }
        public override void Update() { }
        public override void FixedUpdate() { }
    }
}
