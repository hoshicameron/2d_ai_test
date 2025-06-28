using UnityEngine;
using PetalsOfHope.Data.Abilities;

namespace PetalsOfHope.Data.Abilities.Types
{
    [CreateAssetMenu(menuName = "Petals of Hope/Data/Abilities/Double Jump", fileName = "NewDoubleJumpAbilitySO")]
    public class DoubleJumpSO : AbilitySO
    {
        [Header("Double Jump Specifics")]
        [Min(0f)]
        [Tooltip("Additional jump force for the double jump, relative to normal jump or absolute.")]
        public float doubleJumpForceMultiplier = 1.0f;

        public int MaxJumps = 2;

        public DoubleJumpSO()
        {
            abilityName = "Double Jump";
            cooldown = 0f;
            description = "Allows the player to perform an additional jump in mid-air.";
        }
    }
}
