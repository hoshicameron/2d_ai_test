using UnityEngine;
using PetalsOfHope.Core.Data.Stats;

namespace PetalsOfHope.Data.Player
{
    [CreateAssetMenu(menuName = "Petals of Hope/Data/Stats/Player Stats", fileName = "NewPlayerStatsSO")]
    public class PlayerStatsSO : EntityStatsSO
    {
        [Header("Player Specific Stats")]
        [Min(0f)]
        [Tooltip("Speed at which the player moves horizontally.")]
        public float movementSpeed = 5f;

        [Min(0f)]
        [Tooltip("Force applied to the player when jumping.")]
        public float jumpForce = 10f;
    }
}
