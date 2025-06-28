using _Project.Scripts.Gameplay.Character;
using UnityEngine;

namespace PetalsOfHope.Gameplay.Enemies.Core
{
    public class EnemyController : CharacterControllerBase
    {
        [Header("Health")]
        [SerializeField] private EnemyHealth enemyHealth;

        protected override void Awake()
        {
            base.Awake();
            enemyHealth.Initialize(_stats.maxHealth);
        }

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