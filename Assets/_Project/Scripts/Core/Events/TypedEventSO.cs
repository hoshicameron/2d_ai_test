// In Assets/_Project/Scripts/Core/Events/TypedEventSO.cs
namespace PetalsOfHope.Core.Events
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using PetalsOfHope.Core.Events.Base;

    public abstract class TypedEventSO<T> : BaseEventSO
    {
        private readonly List<Action<T>> _listeners = new List<Action<T>>();

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

    // Concrete implementations for common types
    
    [CreateAssetMenu(menuName = "Petals of Hope/Events/Typed Events/Int Event", fileName = "NewIntEventSO")]
    public class IntEventSO : TypedEventSO<int> {}

    [CreateAssetMenu(menuName = "Petals of Hope/Events/Typed Events/Float Event", fileName = "NewFloatEventSO")]
    public class FloatEventSO : TypedEventSO<float> {}

    [CreateAssetMenu(menuName = "Petals of Hope/Events/Typed Events/String Event", fileName = "NewStringEventSO")]
    public class StringEventSO : TypedEventSO<string> {}

    [CreateAssetMenu(menuName = "Petals of Hope/Events/Typed Events/Bool Event", fileName = "NewBoolEventSO")]
    public class BoolEventSO : TypedEventSO<bool> {}

    [CreateAssetMenu(menuName = "Petals of Hope/Events/Typed Events/Vector2 Event", fileName = "NewVector2EventSO")]
    public class Vector2EventSO : TypedEventSO<Vector2> {}

    [CreateAssetMenu(menuName = "Petals of Hope/Events/Typed Events/Vector3 Event", fileName = "NewVector3EventSO")]
    public class Vector3EventSO : TypedEventSO<Vector3> {}

    [CreateAssetMenu(menuName = "Petals of Hope/Events/Typed Events/GameObject Event", fileName = "NewGameObjectEventSO")]
    public class GameObjectEventSO : TypedEventSO<GameObject> {}
}
