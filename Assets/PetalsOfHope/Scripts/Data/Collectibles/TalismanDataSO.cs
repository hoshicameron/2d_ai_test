using UnityEngine;
using PetalsOfHope.Contracts;

namespace PetalsOfHope.Data.Collectibles
{
    [CreateAssetMenu(menuName = "Petals of Hope/Data/Collectibles/Talisman Data", fileName = "NewTalismanDataSO")]
    public class TalismanDataSO : ScriptableObject, ICollectible
    {
        [Header("Talisman Identification")]
        [Tooltip("Unique identifier for this talisman. Must be unique across all talismans.")]
        public string talismanID;

        [Header("Display Information")]
        [Tooltip("Name displayed to the player.")]
        public string displayName = "New Talisman";
        
        [TextArea(3, 5)]
        [Tooltip("Description of the talisman's lore or effect.")]
        public string description = "A mysterious talisman.";

        [Tooltip("Icon representing the talisman in UI.")]
        public Sprite icon;

        // Interface implementations
        public string ID => talismanID;
        public string DisplayName => displayName;
        public string Description => description;

        public override bool Equals(object other)
        {
            if (other is TalismanDataSO otherTalisman)
            {
                return !string.IsNullOrEmpty(talismanID) && talismanID == otherTalisman.talismanID;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return !string.IsNullOrEmpty(talismanID) ? talismanID.GetHashCode() : base.GetHashCode();
        }
        
        /*
        HOW TO USE:
        1. Create an instance of this ScriptableObject for each unique talisman in your game.
        2. Assign a unique "Talisman ID" (e.g., "TALISMAN_DOUBLE_JUMP"). This ID is used for saving and checking progression.
        3. Fill in the display name, description, and icon for UI purposes.
        4. Add this asset to the TalismanDatabaseSO to make it accessible to the save/load system.
        */
    }
}
