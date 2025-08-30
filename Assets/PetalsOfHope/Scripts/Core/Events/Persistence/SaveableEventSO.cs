using UnityEngine;
using PetalsOfHope.Contracts;

namespace PetalsOfHope.Core.Events
{
    /// <summary>
    /// An event channel specifically for passing ISaveable references.
    /// Used for dynamic registration and unregistration with the SaveLoadManager.
    /// </summary>
    [CreateAssetMenu(menuName = "Petals of Hope/Events/Persistence/Saveable Event", fileName = "NewSaveableEventSO")]
    public class SaveableEventSO : TypedEventSO<ISaveable>
    {
    }
}
