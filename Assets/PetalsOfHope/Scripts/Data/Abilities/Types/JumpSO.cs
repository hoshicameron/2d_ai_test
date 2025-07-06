using UnityEngine;

namespace PetalsOfHope.Data.Abilities.Types
{
    [CreateAssetMenu(menuName = "Petals of Hope/Data/Abilities/Jump")]
    public class JumpSO : AbilitySO
    {
        
        [Min(0f)]
        [Tooltip("Force applied to the player when jumping.")]
        public float jumpForce = 10f;

        [Range(0f, 1f)]
        [Tooltip("How much control the player has in the air (0 = no control, 1 = full control like on ground).")]
        public float airControlFactor = 0.8f;
        
        [Header("Double Jump Specifics")]
        [Min(0f)]
        [Tooltip("Additional jump force for the double jump, relative to normal jump or absolute.")]
        public float doubleJumpForceMultiplier = 1.0f;

        public int MaxJumps = 2;

        public JumpSO()
        {
            abilityName = "Double Jump";
            description = "Allows the player to perform an additional jump in mid-air.";
        }
    }
}
