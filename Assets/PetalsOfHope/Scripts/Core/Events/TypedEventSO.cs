// In Assets/_Project/Scripts/Core/Events/TypedEventSO.cs
namespace PetalsOfHope.Core.Events
{
    using System;
    using System.Collections.Generic;
    using PetalsOfHope.Core.Events.Base;

    public abstract class TypedEventSO<T> : BaseEventSO
    {
        private readonly List<Action<T>> _listeners = new();

        public event Action<T> OnEventRaised; // Optional: for C# code direct subscription

        public void Raise(T value)
        {
            // Iterate backwards to allow unsubscription during invocation
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i]?.Invoke(value);
            }
            OnEventRaised?.Invoke(value); // Invoke C# event
        }

        public void RegisterListener(Action<T> listener)
        {
            if (!_listeners.Contains(listener))
            {
                _listeners.Add(listener);
            }
        }

        public void UnregisterListener(Action<T> listener)
        {
            if (_listeners.Contains(listener))
            {
                _listeners.Remove(listener);
            }
        }
    }
}
