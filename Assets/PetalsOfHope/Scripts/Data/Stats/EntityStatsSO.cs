using UnityEngine;

namespace PetalsOfHope.Data.Stats
{
    public abstract class EntityStatsSO : ScriptableObject
    {
        [Header("Base Stats")]
        [Min(1)]
        [Tooltip("Maximum health of the entity.")]
        public int maxHealth = 100;

        protected virtual void OnValidate()
        {
            if (maxHealth <= 0)
            {
                maxHealth = 1;
                Debug.LogWarning($"{name}: maxHealth must be greater than 0. Reset to 1.", this);
            }
        }
    }
}
