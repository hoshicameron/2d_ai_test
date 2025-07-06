using PetalOfHope.Gameplay.Character;
using UnityEngine;

namespace PetalsOfHope.Gameplay.Enemies.Core
{
    [RequireComponent(typeof(EnemyHealth))]
    public class EnemyController : CharacterControllerBase
    {
        private EnemyHealth _enemyHealth;

        protected override void Awake()
        {
            base.Awake();
            _enemyHealth = GetComponent<EnemyHealth>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _enemyHealth.OnDeath += HandleCharacterDeath;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _enemyHealth.OnDeath -= HandleCharacterDeath;
        }
    }
}