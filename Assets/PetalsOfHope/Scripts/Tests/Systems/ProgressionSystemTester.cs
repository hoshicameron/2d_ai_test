using UnityEngine;
using PetalsOfHope.Core.Events;
using PetalsOfHope.Data.Collectibles;
using PetalsOfHope.Core.Events.Channels;

namespace PetalsOfHope.Tests.Systems
{
    /// <summary>
    /// A component for testing the GameProgressionSystem from the Inspector.
    /// </summary>
    public class ProgressionSystemTester : MonoBehaviour
    {
        [Header("Event Raisers")]
        [Tooltip("The channel to raise when a collectible is acquired.")]
        [SerializeField] private CollectibleEventSO onCollectibleCollected;

        [Header("Function Channels")]
        [Tooltip("The channel to query for unlocked abilities/talismans.")]
        [SerializeField] private AbilityCheckChannelSO abilityCheckChannel;

        [Header("Test Data")]
        [Tooltip("The specific Talisman to award when testing.")]
        [SerializeField] private TalismanDataSO talismanToAward;
        
        [Tooltip("The ID of the Talisman to check for.")]
        [SerializeField] private string talismanIDToCheck;

        [ContextMenu("Test: Award Talisman")]
        public void TestAwardTalisman()
        {
            if (onCollectibleCollected == null || talismanToAward == null)
            {
                Debug.LogError("Talisman Award Event Channel or Talisman To Award is not assigned.", this);
                return;
            }
            
            Debug.Log($"TEST: Raising event to award talisman: {talismanToAward.displayName} (ID: {talismanToAward.ID})");
            onCollectibleCollected.Raise(talismanToAward);
        }

        [ContextMenu("Test: Check for Talisman")]
        public void TestCheckForTalisman()
        {
            if (abilityCheckChannel == null)
            {
                Debug.LogError("Ability Check Channel is not assigned.", this);
                return;
            }
            if (string.IsNullOrEmpty(talismanIDToCheck))
            {
                Debug.LogError("Talisman ID To Check is empty.", this);
                return;
            }

            bool isUnlocked = abilityCheckChannel.Function?.Invoke(talismanIDToCheck) ?? false;
            
            Debug.Log($"TEST: Checking for talisman ID '{talismanIDToCheck}'. Result: {isUnlocked}");
        }
        
        /*
        HOW TO USE:
        1. Add this component to a GameObject in your test scene.
        2. Assign the "OnCollectibleCollected" event channel asset.
        3. Assign the "AbilityCheckChannel" asset.
        4. Assign a "TalismanDataSO" asset to the "Talisman To Award" field (e.g., your "Talisman_Dash" asset).
        5. Enter the ID of that same talisman into the "Talisman ID To Check" field (e.g., "TALISMAN_DASH").
        6. Enter Play Mode.
        7. In the Inspector, click the three dots (...) on this component and select "Test: Check for Talisman". The console should log "Result: False".
        8. Now, select "Test: Award Talisman". The console should log that the event was raised.
        9. Finally, select "Test: Check for Talisman" again. The console should now log "Result: True".
        */
    }
}
