using UnityEngine;
using UnityEngine.Events;

namespace PetalsOfHope.Core.Events.Listeners
{
    [AddComponentMenu("Petals of Hope/Event Listeners/Float Event Listener")]
    public class FloatEventListener : TypedEventListener<FloatEventSO, UnityFloatEvent, float> {}
    [System.Serializable] public class UnityFloatEvent : UnityEvent<float> {}
}