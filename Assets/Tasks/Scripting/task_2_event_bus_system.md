# Task ID: 2
# Parent Task ID: None
# Title: Event Bus System
# Status: completed
# Dependencies: 1
# Priority: critical
# Estimated Effort: M
# Assignee: AI
# Completed By: Cascade
# Completion Date: 2025-05-25

# Description:
Implement the Event Bus System using ScriptableObjects to manage game events.

# Details:
1. Implement `BaseEventSO.cs` (abstract `ScriptableObject`) in `_Project/Scripts/Core/Events/Base/`.
2. Implement `GameEventSO.cs` (parameterless, `Action OnEventRaised`) in `_Project/Scripts/Core/Events/`.
3. Implement `TypedEventSO<T>.cs` (generic, `Action<T> OnEventRaised`) in `_Project/Scripts/Core/Events/`.
4. Implement `EventListener.cs` (MonoBehaviour, listens to `GameEventSO`, invokes `UnityEvent`) in `_Project/Scripts/Core/Events/Listeners/`.
5. Implement `TypedEventListener<T>.cs` (MonoBehaviour, listens to `TypedEventSO<T>`, invokes `UnityEvent<T>`).
6. Create editor tools for event debugging (e.g., "Raise" button in Inspector).
7. Unit test event propagation (listener registration, unregistration, raising events).
8. Create initial `EventSO` assets in `_Project/ScriptableObjects/Events/`.

# Acceptance Criteria:
- `BaseEventSO`, `GameEventSO`, and `TypedEventSO<T>` are implemented correctly.
- `EventListener` and `TypedEventListener<T>` are implemented and functional.
- Editor tools for event debugging are available.
- Unit tests for event propagation pass.
- Initial `EventSO` assets are created.

# Test Strategy:
- Unit tests for event handling and propagation.
- Manual testing of event raising and listening in the Unity Editor.

# Implementation Details:
- Implemented `BaseEventSO` as the base class for all events
- Created `GameEventSO` for parameterless events
- Implemented `TypedEventSO<T>` for generic typed events
- Added `EventListener` and `TypedEventListener<T>` components for scene-based event handling
- Created editor tools for debugging events in the Inspector
- Added comprehensive unit tests for event functionality
- Documented usage in README.md with examples and best practices

# Notes/Questions:
- Event system is now ready for use throughout the project
- Editor tools provide visual debugging of event subscribers
- All components include XML documentation for IntelliSense support
- Follow the best practices outlined in the README for optimal usage
