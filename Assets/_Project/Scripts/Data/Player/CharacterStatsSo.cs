using UnityEngine;
using PetalsOfHope.Core.Data.Stats;

namespace PetalsOfHope.Data.Player
{
    [CreateAssetMenu(menuName = "Petals of Hope/Data/Stats/Player Stats", fileName = "NewPlayerStatsSO")]
    public class CharacterStatsSo : EntityStatsSO
    {
        [Header("Player Specific Stats")]
        [Min(0f)]
        [Tooltip("Speed at which the player moves horizontally.")]
        public float movementSpeed = 5f;

        [Min(0f)]
        [Tooltip("Force applied to the player when jumping.")]
        public float jumpForce = 10f;

        [Range(0f, 1f)]
        [Tooltip("How much control the player has in the air (0 = no control, 1 = full control like on ground).")]
        public float airControlFactor = 0.8f;

        public float gravityScale = 1f;
    }
}
