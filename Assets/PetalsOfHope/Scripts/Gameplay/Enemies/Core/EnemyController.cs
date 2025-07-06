using PetalOfHope.Gameplay.Character;
using PetalsOfHope.Data.Player;
using UnityEngine;

namespace PetalsOfHope.Gameplay.Enemies.Core
{
    public class EnemyController : CharacterControllerBase
    {
        [Header("Health")]
        [SerializeField] private EnemyHealth enemyHealth;

        protected override void OnEnable()
        {
            base.OnEnable();
            enemyHealth.OnDeath += HandleCharacterDeath;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            enemyHealth.OnDeath -= HandleCharacterDeath;
        }
    }
}