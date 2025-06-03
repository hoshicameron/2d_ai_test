using UnityEngine;
using UnityEngine.Events;

namespace PetalsOfHope.Core.Events.Listeners
{
    [AddComponentMenu("Petals of Hope/Event Listeners/GameObject Event Listener")]
    public class GameObjectEventListener : TypedEventListener<GameObjectEventSO, UnityGameObjectEvent, GameObject> {}
    [System.Serializable] public class UnityGameObjectEvent : UnityEvent<GameObject> {}
}