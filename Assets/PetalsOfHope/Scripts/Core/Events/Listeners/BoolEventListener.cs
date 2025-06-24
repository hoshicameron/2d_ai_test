using UnityEngine;
using UnityEngine.Events;

namespace PetalsOfHope.Core.Events.Listeners
{
    [AddComponentMenu("Petals of Hope/Event Listeners/Bool Event Listener")]
    public class BoolEventListener : TypedEventListener<BoolEventSO, UnityBoolEvent, bool> {}
    [System.Serializable] public class UnityBoolEvent : UnityEvent<bool> {}
}