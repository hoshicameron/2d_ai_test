using UnityEngine;
using UnityEngine.Events;

namespace PetalsOfHope.Core.Events.Listeners
{
    [AddComponentMenu("Petals of Hope/Event Listeners/Vector2 Event Listener")]
    public class Vector2EventListener : TypedEventListener<Vector2EventSO, UnityVector2Event, Vector2> {}
    [System.Serializable] public class UnityVector2Event : UnityEvent<Vector2> {}
}