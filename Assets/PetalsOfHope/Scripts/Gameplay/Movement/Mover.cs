

using PetalsOfHope.Interfaces;
using UnityEngine;

namespace PetalsOfHope.Gameplay.Movement
{
    /// <summary>
    /// An abstract base class for creating Mover strategies as ScriptableObjects.
    /// </summary>
    public abstract class Mover : ScriptableObject, IMover
    {
        protected GameObject agent;
        protected Rigidbody2D rigidbody;
        protected Transform transform;

        public virtual void Init(GameObject agent, Rigidbody2D rb)
        {
            this.agent = agent;
            this.rigidbody = rb;
            this.transform = agent.transform;
        }

        public abstract void Move(Vector2 moveInput, float speed);
        public abstract void Stop();
        
        // Concrete movers must now implement Jump and Dash.
        public abstract void Jump(float jumpForce);
        public abstract void Dash(Vector2 dashDirection, float dashForce);
        
        public abstract void Tick(float deltaTime);
    }
}