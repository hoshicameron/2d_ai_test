using UnityEngine;

namespace PetalsOfHope.Data.Abilities.Types
{
    [CreateAssetMenu(menuName = "Petals of Hope/Data/Abilities/Wall Jump")]
    public class WallJumpSO : AbilitySO
    {
        [Header("Wall Jump Settings")]
        [Min(1f), Tooltip("Upward force when wall jumping")]
        public float wallJumpForce = 15f;
        
        [Min(1f), Tooltip("Horizontal force when wall jumping away from wall")]
        public float wallJumpHorizontalForce = 10f;
        
        [Min(0f), Tooltip("How long to disable movement input after wall jump")]
        public float wallJumpInputDisableTime = 0.2f;
        
        [Min(0f), Tooltip("Time after leaving wall when you can still wall jump")]
        public float coyoteWallTime = 0.2f;

        public WallJumpSO()
        {
            abilityName = "WallJump";
            description = "Allows the player to jump off walls.";
        }
    }
}
