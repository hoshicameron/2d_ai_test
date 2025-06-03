// In Assets/_Project/Scripts/Core/Events/Listeners/GameEventListener.cs
namespace PetalsOfHope.Core.Events.Listeners
{
    using UnityEngine;
    using UnityEngine.Events;
    using PetalsOfHope.Core.Events;

    [AddComponentMenu("Petals of Hope/Events/Game Event Listener")]
    public class GameEventListener : BaseTypedEventListener
    {
        [Tooltip("Game Event to register with.")]
        [SerializeField] private GameEventSO _eventSO;

        [Tooltip("Response to invoke when Event is raised.")]
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

        public GameEventSO EventSO
        {
            get => _eventSO;
            set
            {
                if (_eventSO != null)
                {
                    _eventSO.UnregisterListener(Respond);
                }
                
                _eventSO = value;
                
                if (_eventSO != null && enabled)
                {
                    _eventSO.RegisterListener(Respond);
                }
            }
        }

        // Helper method to raise the event from the UnityEvent
        public void RaiseEvent()
        {
            _eventSO?.Raise();
        }
    }
}
