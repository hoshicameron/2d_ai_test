using UnityEngine;
using PetalsOfHope.Core.Events;
using PetalsOfHope.Interfaces;

namespace PetalsOfHope.UI.HUD
{
    /// <summary>
    /// Handles the logic for the HUD. Listens to game events and updates the HUDView.
    /// </summary>
    [RequireComponent(typeof(HUDView))]
    public class HUDController : MonoBehaviour
    {
        private HUDView view;

        [Header("Event Listeners")]
        [Tooltip("Event raised when the player's health changes. Expects an int payload for current health.")]
        [SerializeField] private IntEventSO playerHealthChangedEvent;
        [Tooltip("Event raised when a collectible is acquired.")]
        [SerializeField] private CollectibleEventSO collectibleCollectedEvent;

        // This controller will need a way to know the player's max health.
        // For now, we can assume a direct reference or a default value.
        private int maxPlayerHealth = 100; // Placeholder

        private void Awake()
        {
            view = GetComponent<HUDView>();
        }

        private void OnEnable()
        {
            playerHealthChangedEvent?.RegisterListener(OnPlayerHealthChanged);
            collectibleCollectedEvent?.RegisterListener(OnCollectibleCollected);
        }

        private void OnDisable()
        {
            playerHealthChangedEvent?.UnregisterListener(OnPlayerHealthChanged);
            collectibleCollectedEvent?.UnregisterListener(OnCollectibleCollected);
        }

        private void OnPlayerHealthChanged(int currentHealth)
        {
            // To properly update a slider, we need the max health.
            // This should be fetched from a PlayerStatsSO or a direct reference.
            // For now, using a placeholder.
            view.UpdatePlayerHealth(currentHealth, maxPlayerHealth);
        }

        private void OnCollectibleCollected(ICollectible collectible)
        {
            // This is a simplified example. A real implementation would likely
            // listen to an event from an InventorySystem that specifically tracks talisman counts.
            if (collectible != null)
            {
                // For now, we don't have a way to get the total count.
                // This logic will need to be updated once an InventorySystem is in place.
                // _view.UpdateTalismanCount(inventorySystem.TalismanCount);
            }
        }
        
        /*
        HOW TO USE:
        1. Attach this component to the same GameObject as the HUDView.
        2. Create and assign the required EventSO assets in the Inspector.
        3. The PlayerHealth component should raise the `_playerHealthChangedEvent` when the player takes damage or heals.
        4. An InventorySystem should raise an event when the talisman count changes, which this controller would listen to.
           (The current `OnCollectibleCollected` is a placeholder for that).
        */
    }
}
