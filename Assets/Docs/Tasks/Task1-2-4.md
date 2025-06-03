# Task ID: 1.2.4
# Parent Task ID: 1.2
# Title: Implement EventListener (for GameEventSO)
# Status: completed
# Dependencies: 1.2.2
# Priority: critical
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement `EventListener.cs`, a MonoBehaviour component that listens to a `GameEventSO` (parameterless) and invokes a `UnityEvent` in response.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/Core/Events/Listeners/EventListener.cs`
2.  **Namespace:** `PetalsOfHope.Core.Events.Listeners`
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/Core/Events/Listeners/EventListener.cs
    namespace PetalsOfHope.Core.Events.Listeners
    {
        using UnityEngine;
        using UnityEngine.Events; // Required for UnityEvent

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
    ```
4.  **Functionality:**
    *   Allows designers/developers to link a `GameEventSO` asset in the Inspector.
    *   Exposes a `UnityEvent` in the Inspector to configure responses (e.g., call methods on other components).
    *   Automatically registers/unregisters from the `GameEventSO` in `OnEnable`/`OnDisable`.

# Acceptance Criteria:
- `EventListener.cs` file is created at the specified location.
- The class is a `MonoBehaviour` and is in the `PetalsOfHope.Core.Events.Listeners` namespace.
- It has a serialized field for a `GameEventSO`.
- It has a serialized field for a `UnityEvent` as a response.
- It correctly registers to the `GameEventSO` in `OnEnable` and unregisters in `OnDisable`.
- When the `GameEventSO` is raised, the `UnityEvent` response is invoked.
- The script compiles without errors.

# Test Strategy:
- Manual Verification:
    - Create a `GameEventSO` asset.
    - Create a GameObject in a scene, add an `EventListener` component to it.
    - Assign the `GameEventSO` asset to the listener.
    - Configure the `UnityEvent` response (e.g., to call `Debug.Log` or activate/deactivate another GameObject).
    - Create a simple script/button to manually raise the `GameEventSO`.
    - Enter Play mode, raise the event, and verify the `UnityEvent` response is triggered.
- Integration Testing (covered in Task 1.2.7 or later system tests): Ensure it works with `GameEventSO` properly.

# Notes/Questions:
- The `EventSO` property with a setter allows for dynamic reassignment of the event, ensuring proper unregistration from the old event and registration to the new one. This is good practice.
- Ensure the check for `_eventSO != null` is present before attempting to register/unregister.