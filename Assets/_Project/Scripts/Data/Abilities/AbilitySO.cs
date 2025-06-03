using UnityEngine;

namespace PetalsOfHope.Data.Abilities
{
    public abstract class AbilitySO : ScriptableObject
    {
        [Header("Ability Info")]
        [Tooltip("Display name of the ability.")]
        public string abilityName = "New Ability";

        [Tooltip("Icon for the ability (e.g., for UI).")]
        public Sprite icon;

        [Min(0f)]
        [Tooltip("Cooldown time in seconds before the ability can be used again.")]
        public float cooldown = 1f;

        [TextArea(3, 5)]
        [Tooltip("Description of the ability.")]
        public string description = "";
    }
}
