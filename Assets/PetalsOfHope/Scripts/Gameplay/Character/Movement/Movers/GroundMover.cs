using UnityEngine;

namespace PetalOfHope.Gameplay.Character.Movement.Movers
{
    /// <summary>
    /// Implements standard 2D platformer movement, handling ground movement,
    /// air control, jumping, and dashing in one centralized place.
    /// </summary>
    [CreateAssetMenu(menuName = "Petals of Hope/Logic/Movement/Ground Mover")]
    public class GroundMover : Mover
    {
        public override void Move(Vector2 moveInput, float speed)
        {
            var targetVelocityX = moveInput.x * speed;
            rigidbody.linearVelocity = new Vector2(targetVelocityX, rigidbody.linearVelocity.y);
        }
    }
}