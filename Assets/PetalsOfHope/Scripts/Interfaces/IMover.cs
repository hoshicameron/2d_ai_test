using UnityEngine;

namespace PetalsOfHope.Interfaces
{
    /// <summary>
    /// The contract for any movement strategy. Defines the core functionalities
    /// for initializing, moving, stopping, and updating a character's physics,
    /// as well as handling special movement abilities.
    /// </summary>
    public interface IMover
    {
        /// <summary>
        /// Initializes the mover with references to the character it will control.
        /// </summary>
        void Init(GameObject agent, Rigidbody2D rb);

        /// <summary>
        /// Sets the character's continuous movement intent.
        /// </summary>
        void Move(Vector2 moveInput, float speed);

        /// <summary>
        /// Commands the character to stop all continuous movement.
        /// </summary>
        void Stop();

        /// <summary>
        /// Executes a jump action.
        /// </summary>
        /// <param name="jumpForce">The vertical force to apply for the jump.</param>
        void Jump(float jumpForce);

        /// <summary>
        /// Executes a dash action.
        /// </summary>
        /// <param name="dashDirection">The direction of the dash.</param>
        /// <param name="dashForce">The force or speed of the dash.</param>
        void Dash(Vector2 dashDirection, float dashForce);

        /// <summary>
        /// The main physics update loop for the mover, called every FixedUpdate.
        /// </summary>
        void Tick(float deltaTime);
    }
}