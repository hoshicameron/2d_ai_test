using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace PetalsOfHope.Data.Collectibles
{
    [CreateAssetMenu(menuName = "Petals of Hope/Data/Collectibles/Talisman Database", fileName = "TalismanDatabaseSO")]
    public class TalismanDatabaseSO : ScriptableObject
    {
        [Tooltip("List of all talisman data assets in the game. Populate this in the editor.")]
        public List<TalismanDataSO> allTalismans;

        private Dictionary<string, TalismanDataSO> _talismanMap;

        private void OnEnable()
        {
            InitializeMap();
        }

        private void InitializeMap()
        {
            if (allTalismans == null)
            {
                _talismanMap = new Dictionary<string, TalismanDataSO>();
                return;
            }

            _talismanMap = allTalismans
                .Where(t => t != null && !string.IsNullOrEmpty(t.talismanID))
                .GroupBy(t => t.talismanID)
                .ToDictionary(g => g.Key, g => {
                    if (g.Count() > 1) {
                        Debug.LogWarning($"Duplicate talismanID '{g.Key}' found in TalismanDatabaseSO. Using the first one.", this);
                    }
                    return g.First();
                });
        }

        public TalismanDataSO GetTalismanByID(string id)
        {
            if (_talismanMap == null) {
                InitializeMap();
            }

            if (string.IsNullOrEmpty(id) || _talismanMap == null) return null;
            _talismanMap.TryGetValue(id, out TalismanDataSO talisman);
            return talisman;
        }
        
        /*
        HOW TO USE:
        1. Create one instance of this ScriptableObject in your project (e.g., in a "Data" or "ScriptableObjects" folder).
        2. Name it "TalismanDatabase".
        3. In the Inspector, lock the inspector (click the padlock icon in the top right).
        4. In the Project window, select all of your TalismanDataSO assets.
        5. Drag all selected TalismanDataSO assets onto the "All Talismans" list in the Inspector.
        6. This database is now ready to be used by the GameProgressionSystem to load talisman data from saved IDs.
        */
    }
}
