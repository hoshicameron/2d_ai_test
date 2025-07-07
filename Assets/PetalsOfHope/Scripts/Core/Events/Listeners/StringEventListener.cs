using UnityEngine;
using UnityEngine.Events;

namespace PetalsOfHope.Core.Events.Listeners
{
    [AddComponentMenu("Petals of Hope/Event Listeners/String Event Listener")]
    public class StringEventListener : TypedEventListener<StringEventSO, UnityStringEvent, string> {}
    [System.Serializable] public class UnityStringEvent : UnityEvent<string> {}
}