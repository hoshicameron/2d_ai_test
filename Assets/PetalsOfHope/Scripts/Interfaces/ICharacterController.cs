using UnityEngine;

namespace PetalsOfHope.Interfaces
{
    /// <summary>
    /// An interface that provides a read-only view of a character's state and properties.
    /// This allows core systems like Movers to access necessary data without depending
    /// on the concrete CharacterControllerBase class, breaking circular dependencies.
    /// </summary>
    public interface ICharacterController
    {
        // Component Properties
        GameObject GameObject { get; }
        Transform Transform { get; }
        Rigidbody2D Rigidbody { get; }

        // State Properties
        bool IsGrounded { get; }
        int RemainingJumps { get; }
        int MaxJumps { get; }
        Vector2 MoveInput { get; }

        // Data Properties
        float MovementSpeed { get; }
        float JumpForce { get; }
        float AirControlFactor { get; }
        float DashForce { get; }
        float DoubleJumpForceMultiplier { get; }
    }
}