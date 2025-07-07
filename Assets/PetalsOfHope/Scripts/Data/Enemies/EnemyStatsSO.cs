using UnityEngine;
using PetalsOfHope.Core.Data.Stats;

namespace PetalsOfHope.Data.Enemies
{
    [CreateAssetMenu(menuName = "Petals of Hope/Data/Stats/Enemy Stats", fileName = "NewEnemyStatsSO")]
    public class EnemyStatsSO : EntityStatsSO
    {
        [Header("Enemy Specific Stats")]
        [Min(0f)]
        [Tooltip("Speed at which the enemy patrols.")]
        public float patrolSpeed = 2f;

        [Min(0f)]
        [Tooltip("Speed at which the enemy chases the player.")]
        public float chaseSpeed = 4f;

        [Min(0f)]
        [Tooltip("Range at which the enemy can detect the player.")]
        public float detectionRange = 8f;

        [Min(0)]
        [Tooltip("Amount of damage the enemy deals on contact or attack.")]
        public int damage = 10;

        [Min(0f)]
        [Tooltip("Duration of the death animation in seconds before the enemy is destroyed.")]
        public float deathAnimationDuration = 1f;
    }
}
