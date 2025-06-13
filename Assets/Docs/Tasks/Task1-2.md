# Task ID: 1.2
# Parent Task ID: 1
# Title: Event Bus System Implementation
# Status: completed
# Dependencies: 1.1.2, 1.1.4
# Priority: critical
# Estimated Effort: L (aggregate of subtasks)
# Assignee: Unassigned

# Description:
Implement a ScriptableObject-based event bus system for decoupled communication between different game systems. This includes base event types, generic typed events, and MonoBehaviour listeners.

# Details:
This system will reside primarily in the `PetalsOfHope.Core.Events` namespace. It involves:
- A base abstract ScriptableObject for all events.
- A parameterless event ScriptableObject.
- A generic typed event ScriptableObject for events with payloads.
- MonoBehaviour components to listen to these events from the scene and invoke UnityEvents.
- Editor tools for debugging.
- Unit tests.
- Creation of initial event assets.

Refer to subtasks 1.2.1 through 1.2.8 for specific implementation details.

# Acceptance Criteria:
- All subtasks (1.2.1 - 1.2.8) are completed.
- The event bus system allows for defining, raising, and listening to both parameterless and typed events.
- Editors can easily create and assign event assets.
- The system is unit tested.
- Debugging tools are available.

# Test Strategy:
- Integration testing: Ensure different game objects can communicate via the event bus without direct references (once other systems use it).
- Verify unit tests for event propagation pass.
- Use editor debugging tools to manually raise events and confirm listeners react.

# Notes/Questions:
- This is a foundational system heavily relied upon by other systems.