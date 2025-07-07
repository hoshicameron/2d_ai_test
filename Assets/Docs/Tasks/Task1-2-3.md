# Task ID: 1.2.3
# Parent Task ID: 1.2
# Title: Implement TypedEventSO<T> (Generic)
# Status: pending
# Dependencies: 1.2.1
# Priority: critical
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement `TypedEventSO<T>.cs`, a generic ScriptableObject for events that carry a data payload of type `T`.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/Core/Events/TypedEventSO.cs`
2.  **Namespace:** `PetalsOfHope.Core.Events`
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/Core/Events/TypedEventSO.cs
    namespace PetalsOfHope.Core.Events
    {
        using System;
        using System.Collections.Generic;
        using UnityEngine;
        using PetalsOfHope.Core.Events.Base; // For BaseEventSO

        // Note: Generic ScriptableObjects cannot be directly created via [CreateAssetMenu]
        // without a concrete derived class. We'll need specific typed event assets or a custom editor.
        // For now, derived classes will have the [CreateAssetMenu] attribute.
        // Alternatively, users can create non-generic derived classes for common types like IntEventSO, StringEventSO etc.
        // and put [CreateAssetMenu] on those.

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

        // Example of concrete derived classes that CAN have CreateAssetMenu
        // These can be placed in a separate file or at the end of TypedEventSO.cs initially.
        // Example: IntEventSO
        [CreateAssetMenu(menuName = "Petals of Hope/Events/Typed Events/Int Event", fileName = "NewIntEventSO")]
        public class IntEventSO : TypedEventSO<int> {}

        // Example: Vector2EventSO
        [CreateAssetMenu(menuName = "Petals of Hope/Events/Typed Events/Vector2 Event", fileName = "NewVector2EventSO")]
        public class Vector2EventSO : TypedEventSO<Vector2> {}

        // Example: StringEventSO
        [CreateAssetMenu(menuName = "Petals of Hope/Events/Typed Events/String Event", fileName = "NewStringEventSO")]
        public class StringEventSO : TypedEventSO<string> {}

        // Example: BoolEventSO
        [CreateAssetMenu(menuName = "Petals of Hope/Events/Typed Events/Bool Event", fileName = "NewBoolEventSO")]
        public class BoolEventSO : TypedEventSO<bool> {}

        // Example: GameObjectEventSO
        [CreateAssetMenu(menuName = "Petals of Hope/Events/Typed Events/GameObject Event", fileName = "NewGameObjectEventSO")]
        public class GameObjectEventSO : TypedEventSO<GameObject> {}
    }
    ```
4.  **Functionality:**
    *   Abstract class inheriting from `BaseEventSO`, generic on type `T`.
    *   Provides `Raise(T value)` method.
    *   Provides `RegisterListener(Action<T> listener)` and `UnregisterListener(Action<T> listener)`.
    *   Includes a C# `event Action<T> OnEventRaised`.
    *   Concrete, non-generic derived classes (e.g., `IntEventSO`, `Vector2EventSO`) are provided with `[CreateAssetMenu]` attributes to allow easy asset creation for common types.

# Acceptance Criteria:
- `TypedEventSO.cs` file is created at the specified location.
- The class `TypedEventSO<T>` is abstract, generic, inherits from `BaseEventSO`, and is in the `PetalsOfHope.Core.Events` namespace.
- It includes `Raise(T value)`, `RegisterListener(Action<T> listener)`, and `UnregisterListener(Action<T> listener)` methods.
- It uses a `List<Action<T>>` to manage listeners.
- At least a few example concrete derived classes (e.g., `IntEventSO`, `Vector2EventSO`) are provided with `[CreateAssetMenu]` attributes.
- The script compiles without errors.

# Test Strategy:
- Unit Testing (covered in Task 1.2.7):
    - Test registration, unregistration, and invocation with various data types (using the concrete derived classes).
    - Test payload delivery.
- Manual Verification:
    - Create assets of the concrete derived typed events (e.g., `IntEventSO`) in the Unity Editor using the context menu.
    - Inspect the assets.

# Notes/Questions:
- Generic `ScriptableObject` classes (`TypedEventSO<T>`) cannot have `[CreateAssetMenu]` directly. The common pattern is to create non-generic derived classes for specific types that you need assets for, and put the attribute on those. This approach is used here.
- More concrete types can be added as needed (e.g., `FloatEventSO`, `CustomClassEventSO`).