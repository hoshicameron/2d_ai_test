using _Project.Scripts.Gameplay.Character;
using UnityEngine;
using PetalsOfHope.Core.StateMachine;

namespace PetalsOfHope.Gameplay.Player.States
{
    public class DeathState : BaseState
    {
        private readonly CharacterControllerBase _characterController;
        private readonly string _deathAnimationName;

        public DeathState(CharacterControllerBase characterController, StateMachine stateMachine, string deathAnimationName) 
            : base(stateMachine)
        {
            _characterController = characterController;
            _deathAnimationName = deathAnimationName;
        }

        public override void Enter()
        {
            _characterController.CurrentStateName = _stateMachine.CurrentState.GetType().Name;
            
            // Play death animation
            _characterController.AnimationController.Play(_deathAnimationName);
            
            // Disable input
            _characterController.InputSource.DisableAllInput();
            
            // Stop all movement
            _characterController.Rigidbody.linearVelocity = Vector2.zero;
            _characterController.Rigidbody.bodyType = RigidbodyType2D.Kinematic;
            
            // Disable collider after a short delay to allow death animation to play
            var collider = _characterController.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = false;
            }
            
            // Disable other components that might interfere
            _characterController.enabled = false;
        }

        public override void Exit()
        {
            // This state is terminal, typically Exit won't be called
        }

        public override void Update()
        {
            // No updates needed in death state
        }

        public override void FixedUpdate()
        {
            // No fixed updates needed in death state
        }
    }
}
