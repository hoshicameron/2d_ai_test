using UnityEngine;
using PetalsOfHope.Contracts;

namespace PetalsOfHope.Core.Events
{
    [CreateAssetMenu(menuName = "Petals of Hope/Events/Typed Events/Collectible Event", fileName = "NewCollectibleEventSO")]
    public class CollectibleEventSO : TypedEventSO<ICollectible>
    {
        /*
        HOW TO USE:
        1. Create an instance of this asset for each type of collectible event you need (e.g., "OnTalismanCollected").
        2. Systems that award collectibles (e.g., a boss reward script, a collectible pickup script) will raise this event, passing the ScriptableObject of the collected item (e.g., a TalismanDataSO).
        3. Systems that need to know about the collection (e.g., GameProgressionSystem, UI Notifiers) will listen to this event.
        */
    }
}
