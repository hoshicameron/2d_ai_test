using UnityEngine;

namespace PetalsOfHope.Interfaces
{
    public interface IMover
    {
        void Init(ICharacterController controller);
        void Move(Vector2 moveInput, float speed);
    }
}