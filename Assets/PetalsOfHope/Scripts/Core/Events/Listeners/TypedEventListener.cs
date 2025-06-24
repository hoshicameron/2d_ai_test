using UnityEngine;
using UnityEngine.Events;

// In Assets/_Project/Scripts/Core/Events/Listeners/TypedEventListener.cs
namespace PetalsOfHope.Core.Events.Listeners
{
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
}
