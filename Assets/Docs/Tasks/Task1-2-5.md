# Task ID: 1.2.5
# Parent Task ID: 1.2
# Title: Implement TypedEventListener<T> (Generic)
# Status: pending
# Dependencies: 1.2.3
# Priority: critical
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement `TypedEventListener<T>.cs`, a generic MonoBehaviour component that listens to a `TypedEventSO<T>` and invokes a `UnityEvent<T>` in response, passing the payload.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/Core/Events/Listeners/TypedEventListener.cs`
2.  **Namespace:** `PetalsOfHope.Core.Events.Listeners`
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/Core/Events/Listeners/TypedEventListener.cs
    namespace PetalsOfHope.Core.Events.Listeners
    {
        using UnityEngine;
        using UnityEngine.Events;

        // This base class is non-generic and helps with custom editors if needed in the future.
        // For now, it's minimal.
        public abstract class BaseTypedEventListener : MonoBehaviour {}

        // The main generic class.
        // Concrete non-generic derived classes will be needed for Inspector usage
        // unless a custom editor is written for TypedEventListener<T> itself.
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

        // Concrete derived classes for common types to be usable in Inspector:

        // Example: IntEventListener
        // We need to define a concrete UnityEvent type if it doesn't exist for int.
        [System.Serializable] public class UnityIntEvent : UnityEvent<int> {}
        // No CreateAssetMenu needed on listeners, they are components.
        // AddComponentMenu can be used instead.
        [AddComponentMenu("Petals of Hope/Event Listeners/Int Event Listener")]
        public class IntEventListener : TypedEventListener<IntEventSO, UnityIntEvent, int> {}

        // Example: Vector2EventListener
        [System.Serializable] public class UnityVector2Event : UnityEvent<Vector2> {}
        [AddComponentMenu("Petals of Hope/Event Listeners/Vector2 Event Listener")]
        public class Vector2EventListener : TypedEventListener<Vector2EventSO, UnityVector2Event, Vector2> {}

        // Example: StringEventListener
        [System.Serializable] public class UnityStringEvent : UnityEvent<string> {}
        [AddComponentMenu("Petals of Hope/Event Listeners/String Event Listener")]
        public class StringEventListener : TypedEventListener<StringEventSO, UnityStringEvent, string> {}

        // Example: BoolEventListener
        [System.Serializable] public class UnityBoolEvent : UnityEvent<bool> {}
        [AddComponentMenu("Petals of Hope/Event Listeners/Bool Event Listener")]
        public class BoolEventListener : TypedEventListener<BoolEventSO, UnityBoolEvent, bool> {}

        // Example: GameObjectEventListener
        [System.Serializable] public class UnityGameObjectEvent : UnityEvent<GameObject> {}
        [AddComponentMenu("Petals of Hope/Event Listeners/GameObject Event Listener")]
        public class GameObjectEventListener : TypedEventListener<GameObjectEventSO, UnityGameObjectEvent, GameObject> {}
    }
    ```
4.  **Functionality:**
    *   Generic base class `TypedEventListener<TEvent, TUnityEvent, TArg0>` handles the core logic.
    *   Concrete derived classes (e.g., `IntEventListener`) make the component usable from the Unity Editor's "Add Component" menu and allow `TypedEventSO<T>` and `UnityEvent<T>` to be serialized correctly.
    *   Registers/unregisters from the assigned `TypedEventSO<T>`.
    *   Invokes a `UnityEvent<T>` with the payload when the event is raised.

# Acceptance Criteria:
- `TypedEventListener.cs` file is created at the specified location.
- The abstract generic class `TypedEventListener<TEvent, TUnityEvent, TArg0>` is correctly implemented.
- Concrete derived classes for common types (e.g., `IntEventListener`, `Vector2EventListener`) are provided and work in the editor.
- These listeners correctly register/unregister and invoke their respective `UnityEvent<T>` with the payload.
- Scripts compile without errors.
- Concrete listeners are accessible via `AddComponentMenu`.

# Test Strategy:
- Manual Verification:
    - For each concrete listener type (e.g., `IntEventListener`):
        - Create a corresponding `TypedEventSO<T>` asset (e.g., `IntEventSO`).
        - Add the listener component (e.g., `IntEventListener`) to a GameObject.
        - Assign the typed event asset.
        - Configure the `UnityEvent<T>` response (e.g., to a method that takes an `int` and logs it).
        - Create a simple script/button to raise the typed event with a test value.
        - Enter Play mode, raise the event, and verify the response is triggered with the correct payload.
- Integration Testing (covered in Task 1.2.7 or later system tests).

# Notes/Questions:
- The structure `TypedEventListener<TEvent, TUnityEvent, TArg0>` is a common pattern for handling generic `MonoBehaviour` listeners in Unity, where `TEvent` is the specific `TypedEventSO<T>` derivative (like `IntEventSO`), `TUnityEvent` is the specific `UnityEvent<T>` derivative (like `UnityIntEvent`), and `TArg0` is the payload type (like `int`).
- `BaseTypedEventListener` is a placeholder for potential future commonalities or custom editor targets.
- `[AddComponentMenu]` makes these easier to find in the editor.