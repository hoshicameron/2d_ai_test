using PetalsOfHope.Contracts;
using UnityEngine;

namespace PetalOfHope.Gameplay.Character.Movement
{
    /// <summary>
    /// An abstract base class for creating Mover strategies as ScriptableObjects.
    /// </summary>
    public abstract class Mover : ScriptableObject, IMover
    {
        protected ICharacterController controller;
        protected Rigidbody2D rigidbody;
        protected Transform transform;

        public virtual void Init(ICharacterController controller)
        {
            this.controller = controller;
            this.rigidbody = controller.Rigidbody;
            this.transform = controller.Transform;
        }

        public abstract void Move(Vector2 moveInput, float speed);
    }
}