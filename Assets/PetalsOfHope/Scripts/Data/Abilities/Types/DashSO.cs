using UnityEngine;
using PetalsOfHope.Data.Abilities;

namespace PetalsOfHope.Data.Abilities.Types
{
    [CreateAssetMenu(menuName = "Petals of Hope/Data/Abilities/Dash")]
    public class DashSO : AbilitySO
    {
        [Header("Dash Specifics")]
        [Min(0.1f)]
        [Tooltip("Speed of the dash movement.")]
        public float dashSpeed = 20f;

        [Min(0.05f)]
        [Tooltip("Duration of the dash in seconds.")]
        public float dashDuration = 0.2f;
        
        [Min(0f)]
        [Tooltip("Cooldown time in seconds before the ability can be used again.")]
        public float cooldown = 1.5f;

        public DashSO()
        {
            abilityName = "Dash";
            cooldown = 1.5f;
            description = "A quick burst of speed in the facing direction.";
        }
    }
}
