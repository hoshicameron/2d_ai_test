using UnityEngine;

namespace PetalsOfHope.Data.Abilities.Types
{
    [CreateAssetMenu(menuName = "Petals of Hope/Data/Abilities/Move")]
    public class MoveSO : AbilitySO
    {
        [Min(0f)]
        [Tooltip("Speed at which the character moves horizontally.")]
        public float movementSpeed = 5f;
    }
}