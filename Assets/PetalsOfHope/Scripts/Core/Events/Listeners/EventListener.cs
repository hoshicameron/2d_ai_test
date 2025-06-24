// In Assets/_Project/Scripts/Core/Events/Listeners/EventListener.cs
namespace PetalsOfHope.Core.Events.Listeners
{
    using UnityEngine;
    using UnityEngine.Events;
    using PetalsOfHope.Core.Events;

    [AddComponentMenu("Petals of Hope/Event Listeners/Event Listener")]
    public class EventListener : MonoBehaviour
    {
        [Tooltip("Event to register to.")]
        [SerializeField] private GameEventSO _eventSO;

        [Tooltip("Response to invoke when EventSO is raised.")]
        [SerializeField] private UnityEvent _onEventRaisedResponse;

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

        private void Respond()
        {
            _onEventRaisedResponse?.Invoke();
        }

        // Optional: For editor scripting or dynamic assignment
        public GameEventSO EventSO
        {
            get => _eventSO;
            set
            {
                // Unregister from old event if any
                if (_eventSO != null)
                {
                    _eventSO.UnregisterListener(Respond);
                }
                _eventSO = value;
                // Register to new event if assigned
                if (_eventSO != null && enabled) // Only register if component is enabled
                {
                    _eventSO.RegisterListener(Respond);
                }
            }
        }

        public UnityEvent OnEventRaisedResponse => _onEventRaisedResponse;
    }
}
