using UnityEngine;
using PetalsOfHope.Core.StateMachine;

namespace PetalsOfHope.Gameplay.Player.States
{
    public class DeathState : BaseState
    {
        private readonly PlayerController _player;
        private readonly string _deathAnimationName;

        public DeathState(PlayerController player, StateMachine stateMachine, string deathAnimationName) 
            : base(stateMachine)
        {
            _player = player;
            _deathAnimationName = deathAnimationName;
        }

        public override void Enter()
        {
            _player.CurrentStateName = _stateMachine.CurrentState.GetType().Name;
            
            // Play death animation
            _player.AnimationController.Play(_deathAnimationName);
            
            // Disable input
            _player.InputReader.DisableAllInput();
            
            // Stop all movement
            _player.Rigidbody.linearVelocity = Vector2.zero;
            _player.Rigidbody.bodyType = RigidbodyType2D.Kinematic;
            
            // Disable collider after a short delay to allow death animation to play
            var collider = _player.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = false;
            }
            
            // Disable other components that might interfere
            _player.enabled = false;
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
