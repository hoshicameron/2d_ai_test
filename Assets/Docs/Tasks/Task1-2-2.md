# Task ID: 1.2.2
# Parent Task ID: 1.2
# Title: Implement GameEventSO (Parameterless)
# Status: pending
# Dependencies: 1.2.1
# Priority: critical
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement `GameEventSO.cs`, a ScriptableObject for parameterless events, allowing game systems to raise and listen to events without data payloads.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/Core/Events/GameEventSO.cs`
2.  **Namespace:** `PetalsOfHope.Core.Events`
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/Core/Events/GameEventSO.cs
    namespace PetalsOfHope.Core.Events
    {
        using System;
        using System.Collections.Generic;
        using UnityEngine;
        using PetalsOfHope.Core.Events.Base; // For BaseEventSO

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
    ```
4.  **Functionality:**
    *   Inherits from `BaseEventSO`.
    *   Provides a `Raise()` method to invoke the event.
    *   Provides `RegisterListener(Action listener)` and `UnregisterListener(Action listener)` methods for C# scripts to subscribe/unsubscribe.
    *   Uses `[CreateAssetMenu]` attribute for easy asset creation in Unity Editor.
    *   Includes a C# `event Action OnEventRaised` for systems that prefer direct C# event subscription over MonoBehaviour listeners, though the primary mechanism will be the `EventListener` component.

# Acceptance Criteria:
- `GameEventSO.cs` file is created at the specified location.
- The class inherits from `BaseEventSO` and is in the `PetalsOfHope.Core.Events` namespace.
- It includes `Raise`, `RegisterListener`, and `UnregisterListener` methods.
- It uses a `List<Action>` to manage listeners.
- It has the `[CreateAssetMenu]` attribute.
- The script compiles without errors.

# Test Strategy:
- Unit Testing (covered in Task 1.2.7):
    - Test registration and unregistration of listeners.
    - Test that `Raise()` invokes registered listeners.
    - Test that unregistering a listener prevents it from being invoked.
    - Test raising with no listeners.
- Manual Verification:
    - Create a `GameEventSO` asset in the Unity Editor using the context menu.
    - Inspect the asset.

# Notes/Questions:
- The plan mentions `Action OnEventRaised`. This has been interpreted as a C# event in addition to the internal list, providing flexibility. The primary listener mechanism (EventListener.cs) will use the `RegisterListener/UnregisterListener` methods.
- Iterating backwards in `Raise()` is important if a listener might unregister itself or another listener during its execution.