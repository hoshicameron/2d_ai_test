using UnityEngine;
using System;

namespace PetalsOfHope.Data.Collectibles.Talismans
{
    /// <summary>
    /// Represents the data for a talisman collectible item.
    /// </summary>
    [CreateAssetMenu(fileName = "NewTalisman", menuName = "Petals of Hope/Collectibles/Talisman")]
    public class TalismanDataSO : ScriptableObject
    {
        #region Serialized Fields
        
        [Header("Basic Info")]
        [Tooltip("Unique identifier for this talisman")]
        [SerializeField] private string _id = Guid.NewGuid().ToString();
        
        [Tooltip("Display name of the talisman")]
        [SerializeField] private string _displayName = "New Talisman";
        
        [Tooltip("Description of the talisman's effect")]
        [TextArea(3, 5)]
        [SerializeField] private string _description = "Talisman description";
        
        [Header("Visuals")]
        [Tooltip("Icon to display in the UI")]
        [SerializeField] private Sprite _icon;
        
        [Tooltip("Prefab for the 3D model when the talisman is in the world")]
        [SerializeField] private GameObject _worldPrefab;
        
        [Header("Gameplay")]
        [Tooltip("Whether the talisman is unlocked by default")]
        [SerializeField] private bool _isUnlockedByDefault;
        
        [Tooltip("The effect or bonus provided by this talisman")]
        [SerializeField] private TalismanEffect _effect;
        
        [Tooltip("The value of the effect (e.g., percentage bonus, flat value)")]
        [SerializeField] private float _effectValue = 1f;
        
        [Header("Rarity")]
        [Tooltip("Rarity of the talisman")]
        [SerializeField] private ItemRarity _rarity = ItemRarity.Common;
        
        [Header("Acquisition")]
        [Tooltip("How this talisman is obtained")]
        [SerializeField] private AcquisitionMethod _acquisitionMethod = AcquisitionMethod.None;
        
        [Tooltip("Required level or condition to unlock this talisman")]
        [SerializeField] private string _unlockCondition = "";
        
        #endregion
        
        #region Public Properties
        
        /// <summary>
        /// Unique identifier for this talisman
        /// </summary>
        public string Id => _id;
        
        /// <summary>
        /// Display name of the talisman
        /// </summary>
        public string DisplayName => _displayName;
        
        /// <summary>
        /// Description of the talisman's effect
        /// </summary>
        public string Description => _description;
        
        /// <summary>
        /// Icon to display in the UI
        /// </summary>
        public Sprite Icon => _icon;
        
        /// <summary>
        /// Whether the talisman is unlocked by default
        /// </summary>
        public bool IsUnlockedByDefault => _isUnlockedByDefault;
        
        /// <summary>
        /// The effect provided by this talisman
        /// </summary>
        public TalismanEffect Effect => _effect;
        
        /// <summary>
        /// The value of the effect
        /// </summary>
        public float EffectValue => _effectValue;
        
        /// <summary>
        /// Rarity of the talisman
        /// </summary>
        public ItemRarity Rarity => _rarity;
        
        /// <summary>
        /// How this talisman is obtained
        /// </summary>
        public AcquisitionMethod AcquisitionMethod => _acquisitionMethod;
        
        /// <summary>
        /// Required level or condition to unlock this talisman
        /// </summary>
        public string UnlockCondition => _unlockCondition;
        
        #endregion
        
        #region Public Methods
        
        /// <summary>
        /// Applies this talisman's effect to the specified target
        /// </summary>
        public void ApplyEffect(Component target)
        {
            if (target == null) return;
            
            // In a real implementation, this would apply the effect to the target
            // based on the _effect and _effectValue
            switch (_effect)
            {
                case TalismanEffect.IncreaseHealth:
                    // Example: target.Health.IncreaseMaxHealth(_effectValue);
                    break;
                case TalismanEffect.IncreaseDamage:
                    // Example: target.DamageModifier += _effectValue;
                    break;
                // Add more cases for other effects
            }
        }
        
        /// <summary>
        /// Removes this talisman's effect from the specified target
        /// </summary>
        public void RemoveEffect(Component target)
        {
            if (target == null) return;
            
            // In a real implementation, this would remove the effect from the target
            switch (_effect)
            {
                case TalismanEffect.IncreaseHealth:
                    // Example: target.Health.ResetMaxHealth();
                    break;
                case TalismanEffect.IncreaseDamage:
                    // Example: target.DamageModifier -= _effectValue;
                    break;
                // Add more cases for other effects
            }
        }
        
        #endregion
        
        #region Editor Only
        
        #if UNITY_EDITOR
        
        private void OnValidate()
        {
            // Ensure the ID is always set and valid
            if (string.IsNullOrEmpty(_id))
            {
                _id = Guid.NewGuid().ToString();
                UnityEditor.EditorUtility.SetDirty(this);
            }
            
            // Ensure the effect value is positive
            _effectValue = Mathf.Max(0f, _effectValue);
            
            // Update the name in the editor for better organization
            string newName = $"Talisman_{_displayName.Replace(" ", "")}_{_effect}";
            if (name != newName)
            {
                name = newName;
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
        
        #endif
        
        #endregion
    }
    
    /// <summary>
    /// Represents the different types of effects a talisman can have
    /// </summary>
    public enum TalismanEffect
    {
        None = 0,
        IncreaseHealth = 1,
        IncreaseDamage = 2,
        IncreaseSpeed = 3,
        ReduceDamageTaken = 4,
        HealthRegen = 5,
        IncreaseJumpHeight = 6,
        IncreaseLuck = 7,
        IncreaseExperienceGain = 8,
        ReduceCooldowns = 9,
        IncreasePickupRadius = 10,
        // Add more effects as needed
    }
    
    /// <summary>
    /// Represents the rarity of an item
    /// </summary>
    public enum ItemRarity
    {
        Common = 0,
        Uncommon = 1,
        Rare = 2,
        Epic = 3,
        Legendary = 4,
        Mythical = 5
    }
    
    /// <summary>
    /// Represents how a talisman is obtained
    /// </summary>
    public enum AcquisitionMethod
    {
        None = 0,
        StoryProgression = 1,
        EnemyDrop = 2,
        ChestReward = 3,
        ShopPurchase = 4,
        QuestReward = 5,
        SecretDiscovery = 6,
        AchievementReward = 7,
        SpecialEvent = 8
    }
}
