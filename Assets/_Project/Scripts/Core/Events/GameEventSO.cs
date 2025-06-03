// In Assets/_Project/Scripts/Core/Events/GameEventSO.cs
namespace PetalsOfHope.Core.Events
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using PetalsOfHope.Core.Events.Base;

    [CreateAssetMenu(menuName = "Petals of Hope/Events/Game Event", fileName = "NewGameEventSO")]
    public class GameEventSO : BaseEventSO
    {
        private readonly List<Action> _listeners = new List<Action>();

        public event Action OnEventRaised; // Optional: for C# code direct subscription if needed

        public void Raise()
        {
            // Iterate backwards to allow unsubscription during invocation
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i]?.Invoke();
            }
            OnEventRaised?.Invoke(); // Invoke C# event
        }

        public void RegisterListener(Action listener)
        {
            if (!_listeners.Contains(listener))
            {
                _listeners.Add(listener);
            }
        }

        public void UnregisterListener(Action listener)
        {
            if (_listeners.Contains(listener))
            {
                _listeners.Remove(listener);
            }
        }
    }
}
