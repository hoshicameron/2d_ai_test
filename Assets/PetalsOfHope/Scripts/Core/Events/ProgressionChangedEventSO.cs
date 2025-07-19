using UnityEngine;

namespace PetalsOfHope.Core.Events
{
    [CreateAssetMenu(menuName = "Petals of Hope/Events/Game Event", fileName = "NewProgressionChangedEventSO")]
    public class ProgressionChangedEventSO : GameEventSO
    {
        // This class intentionally left blank.
        // It inherits all functionality from the generic GameEventSO.
        // The purpose of this concrete class is to allow the creation of this specific event type
        // as a ScriptableObject asset directly from the Unity Editor's "Create" menu.
    }
}
