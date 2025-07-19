using UnityEngine;
using PetalsOfHope.Core.Events;
using PetalsOfHope.Data.Collectibles;

namespace PetalsOfHope.Gameplay.LevelElements
{
    [RequireComponent(typeof(Collider2D))]
    public class TalismanCollectible : MonoBehaviour
    {
        [Header("Data")]
        [Tooltip("The specific Talisman that this object represents.")]
        [SerializeField] private TalismanDataSO talismanData;

        [Header("Event Raisers")]
        [Tooltip("The event channel to raise when this talisman is collected.")]
        [SerializeField] private CollectibleEventSO onCollectibleCollected;

        private void Awake()
        {
            // Ensure the collider is a trigger so the player can pass through it
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // We only care if the Player touches this
            if (other.CompareTag("Player"))
            {
                Collect();
            }
        }

        private void Collect()
        {
            if (talismanData == null || onCollectibleCollected == null)
            {
                Debug.LogWarning("TalismanCollectible is not fully configured.", this);
                return;
            }

            // Raise the event, passing our specific talisman data as the payload
            onCollectibleCollected.Raise(talismanData);

            // Remove the collectible from the scene
            Destroy(gameObject);
        }
        
        /*
        HOW TO USE:
        1. Create a GameObject in your scene to represent the collectible talisman (e.g., a sprite of the item).
        2. Add a Collider2D component (like a BoxCollider2D or CircleCollider2D) and check "Is Trigger".
        3. Attach this "TalismanCollectible" script to the GameObject.
        4. In the Inspector, assign the specific "TalismanDataSO" asset for this collectible (e.g., "Talisman_Dash").
        5. Assign the one-and-only "OnCollectibleCollected" event channel asset.
        6. When the player touches this object, it will notify the GameProgressionSystem and disappear.
        */
    }
}
