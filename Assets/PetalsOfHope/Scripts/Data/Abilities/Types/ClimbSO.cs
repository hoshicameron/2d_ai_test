using UnityEngine;

namespace PetalsOfHope.Data.Abilities.Types
{
    [CreateAssetMenu(fileName = "NewClimbData", menuName = "Petals of Hope/Data/Abilities/Climb")]
    public class ClimbSO : AbilitySO
    {
        [Header("Climb Settings")] [Min(0.1f), Tooltip("Speed at which the player climbs up/down the ladder")]
        public float climbSpeed = 3f;

        [Min(0.1f), Tooltip("How quickly the player reaches maximum climb speed")]
        public float climbAcceleration = 15f;

        [Min(0.1f), Tooltip("How quickly the player slows down when releasing climb input")]
        public float climbDeceleration = 20f;

        [Tooltip("Layer mask that defines what is considered climbable (e.g., ladders)")]
        public LayerMask climbableLayer;

        [Tooltip("Size of the box used to detect ladders around the player")]
        public Vector2 ladderCheckSize = new(0.5f, 0.1f);

        [Min(0f), Tooltip("Offset from the top of the ladder where climbing transitions to ground movement")]
        public float ladderTopOffset = 0.2f;

        [Range(0f, 1f), Tooltip("How quickly the player snaps to the center of the ladder when starting to climb")]
        public float horizontalSnapDistance = 0.1f;

        [Min(0f), Tooltip("Minimum vertical input required to start climbing")]
        public float minVerticalInputThreshold = 0.1f;

    }
}