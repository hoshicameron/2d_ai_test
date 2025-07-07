# Task ID: 1.2.1
# Parent Task ID: 1.2
# Title: Implement BaseEventSO
# Status: completed
# Dependencies: 1.1.2, 1.1.4 # Folder structure and namespace
# Priority: critical
# Estimated Effort: S
# Assignee: Unassigned

# Description:
Implement an abstract `BaseEventSO.cs` ScriptableObject that will serve as the base class for all specific event ScriptableObjects in the event bus system.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/Core/Events/Base/BaseEventSO.cs`
2.  **Namespace:** `PetalsOfHope.Core.Events` (or `PetalsOfHope.Core.Events.Base` as per plan)
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/Core/Events/Base/BaseEventSO.cs
    namespace PetalsOfHope.Core.Events.Base // Plan indicates /Base/ for this file.
    {
        using UnityEngine;

        /// <summary>
        /// Base class for all ScriptableObject-based events.
        /// Provides a description field for editor documentation.
        /// </summary>
        public abstract class BaseEventSO : ScriptableObject
        {
            [TextArea(3,10)]
            [SerializeField]
            private string _developerDescription = "";
        }
    }
    ```
4.  **Purpose:**
    *   This class doesn't have functional logic for raising or listening itself but can hold common properties or methods for all events in the future (e.g., developer notes, debugging tools).
    *   The `_developerDescription` field helps in documenting the purpose of each event asset in the Inspector.

# Acceptance Criteria:
- `BaseEventSO.cs` file is created at the specified location.
- The class is abstract, inherits from `ScriptableObject`, and is within the `PetalsOfHope.Core.Events.Base` namespace.
- The class includes a `_developerDescription` field with `[TextArea]` and `[SerializeField]` attributes.
- The script compiles without errors.

# Test Strategy:
- Manual Verification:
    - Check the script content for correctness.
    - Ensure it compiles in Unity.
    - (Later) Verify that derived event SOs inherit from this base class.

# Notes/Questions:
- The plan puts `BaseEventSO.cs` in `_Project/Scripts/Core/Events/Base/`. The namespace should reflect this, e.g. `PetalsOfHope.Core.Events.Base`. Confirmed based on plan.