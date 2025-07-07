using PetalsOfHope.Interfaces;
using UnityEngine;

namespace PetalOfHope.Gameplay.Character.Movement.Movers
{
    [CreateAssetMenu(menuName = "Petals of Hope/Logic/Movement/Fly Mover")]
    public class FlyingMover : Mover
    {
        public override void Init(ICharacterController controller)
        {
            base.Init(controller);
            if (rigidbody != null)
            {
                rigidbody.gravityScale = 0;
            }
        }

        public override void Move(Vector2 moveInput, float speed)
        {
            rigidbody.linearVelocity = moveInput * speed;
        }
    }
}