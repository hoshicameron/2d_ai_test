using UnityEngine;
using PetalsOfHope.Data.Enemies;

namespace PetalsOfHope.Data.Enemies.Bosses
{
    [CreateAssetMenu(menuName = "Petals of Hope/Data/Stats/Boss Stats", fileName = "NewBossStatsSO")]
    public class BossStatsSO : EnemyStatsSO
    {
        [Header("Boss Specific Stats")]
        [Min(0)]
        [Tooltip("Special attack damage for boss abilities.")]
        public int specialAttackDamage = 25;
    }
}
