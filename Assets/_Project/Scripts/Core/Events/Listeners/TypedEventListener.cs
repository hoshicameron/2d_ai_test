// In Assets/_Project/Scripts/Core/Events/Listeners/TypedEventListener.cs
namespace PetalsOfHope.Core.Events.Listeners
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using PetalsOfHope.Core.Events;

    // Base non-generic class for editor purposes
    public abstract class BaseTypedEventListener : MonoBehaviour {}

    // Main generic class
    public abstract class TypedEventListener<TEvent, TUnityEvent, TArg0> : BaseTypedEventListener
        where TEvent : TypedEventSO<TArg0>
        where TUnityEvent : UnityEvent<TArg0>
    {
        [Tooltip("Typed Event to register to.")]
        [SerializeField] private TEvent _eventSO;

        [Tooltip("Response to invoke when EventSO is raised.")]
        [SerializeField] private TUnityEvent _onEventRaisedResponse;

        private void OnEnable()
        {
            if (_eventSO != null)
            {
                _eventSO.RegisterListener(Respond);
            }
        }

        private void OnDisable()
        {
            if (_eventSO != null)
            {
                _eventSO.UnregisterListener(Respond);
            }
        }

        private void Respond(TArg0 value)
        {
            _onEventRaisedResponse?.Invoke(value);
        }

        public TEvent EventSO
        {
            get => _eventSO;
            set
            {
                if (_eventSO != null) _eventSO.UnregisterListener(Respond);
                _eventSO = value;
                if (_eventSO != null && enabled) _eventSO.RegisterListener(Respond);
            }
        }
        public TUnityEvent OnEventRaisedResponse => _onEventRaisedResponse;
    }

    // Concrete implementations for common types
    
    [System.Serializable] public class UnityIntEvent : UnityEvent<int> {}
    [AddComponentMenu("Petals of Hope/Event Listeners/Int Event Listener")]
    public class IntEventListener : TypedEventListener<IntEventSO, UnityIntEvent, int> {}

    [System.Serializable] public class UnityFloatEvent : UnityEvent<float> {}
    [AddComponentMenu("Petals of Hope/Event Listeners/Float Event Listener")]
    public class FloatEventListener : TypedEventListener<FloatEventSO, UnityFloatEvent, float> {}

    [System.Serializable] public class UnityStringEvent : UnityEvent<string> {}
    [AddComponentMenu("Petals of Hope/Event Listeners/String Event Listener")]
    public class StringEventListener : TypedEventListener<StringEventSO, UnityStringEvent, string> {}

    [System.Serializable] public class UnityBoolEvent : UnityEvent<bool> {}
    [AddComponentMenu("Petals of Hope/Event Listeners/Bool Event Listener")]
    public class BoolEventListener : TypedEventListener<BoolEventSO, UnityBoolEvent, bool> {}

    [System.Serializable] public class UnityVector2Event : UnityEvent<Vector2> {}
    [AddComponentMenu("Petals of Hope/Event Listeners/Vector2 Event Listener")]
    public class Vector2EventListener : TypedEventListener<Vector2EventSO, UnityVector2Event, Vector2> {}

    [System.Serializable] public class UnityVector3Event : UnityEvent<Vector3> {}
    [AddComponentMenu("Petals of Hope/Event Listeners/Vector3 Event Listener")]
    public class Vector3EventListener : TypedEventListener<Vector3EventSO, UnityVector3Event, Vector3> {}

    [System.Serializable] public class UnityGameObjectEvent : UnityEvent<GameObject> {}
    [AddComponentMenu("Petals of Hope/Event Listeners/GameObject Event Listener")]
    public class GameObjectEventListener : TypedEventListener<GameObjectEventSO, UnityGameObjectEvent, GameObject> {}
}
