using UnityEngine;
using UnityEngine.Events;

namespace PetalsOfHope.Core.Events.Listeners
{
    [AddComponentMenu("Petals of Hope/Event Listeners/Vector3 Event Listener")]
    public class Vector3EventListener : TypedEventListener<Vector3EventSO, UnityVector3Event, Vector3> {}
    [System.Serializable] public class UnityVector3Event : UnityEvent<Vector3> {}
}