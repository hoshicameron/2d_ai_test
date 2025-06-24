using UnityEngine;
using UnityEngine.Events;

namespace PetalsOfHope.Core.Events.Listeners
{
    [AddComponentMenu("Petals of Hope/Event Listeners/Int Event Listener")]
    public class IntEventListener : TypedEventListener<IntEventSO, UnityIntEvent, int> {}
    [System.Serializable] public class UnityIntEvent : UnityEvent<int> {}
}