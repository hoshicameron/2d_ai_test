# Task ID: 1.2.6
# Parent Task ID: 1.2
# Title: Create Editor Tools for Event Debugging
# Status: completed
# Dependencies: 1.2.2, 1.2.3
# Priority: medium
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Create custom editor scripts to aid in debugging ScriptableObject events, specifically by adding a "Raise" button in the Inspector for `GameEventSO` and potentially `TypedEventSO<T>` instances.

# Details:
1.  **Create `GameEventSOEditor.cs`:**
    *   File Location: `Assets/_Project/Editor/Events/GameEventSOEditor.cs`
    *   Namespace: `PetalsOfHope.Editor.Events`
    *   Implementation:
        ```csharp
        // In Assets/_Project/Editor/Events/GameEventSOEditor.cs
        namespace PetalsOfHope.Editor.Events
        {
            using UnityEditor;
            using UnityEngine;
            using PetalsOfHope.Core.Events; // For GameEventSO

            [CustomEditor(typeof(GameEventSO))]
            public class GameEventSOEditor : Editor
            {
                public override void OnInspectorGUI()
                {
                    base.OnInspectorGUI(); // Draw default inspector

                    GUI.enabled = Application.isPlaying; // Enable button only in Play Mode

                    GameEventSO gameEvent = (GameEventSO)target;
                    if (GUILayout.Button("Raise Event"))
                    {
                        if (Application.isPlaying)
                        {
                            gameEvent.Raise();
                        }
                        else
                        {
                            Debug.LogWarning("Cannot raise GameEventSO from editor when not in Play Mode. Start the game to test event raising.");
                        }
                    }
                    GUI.enabled = true;
                }
            }
        }
        ```
2.  **Create `TypedEventSOEditor.cs` (for specific types):**
    *   Since `TypedEventSO<T>` is generic, a direct editor for it is complex. Instead, create editors for the common concrete derived types (e.g., `IntEventSOEditor`).
    *   File Location: `Assets/_Project/Editor/Events/IntEventSOEditor.cs` (and similar for other types)
    *   Namespace: `PetalsOfHope.Editor.Events`
    *   Implementation for `IntEventSOEditor`:
        ```csharp
        // In Assets/_Project/Editor/Events/IntEventSOEditor.cs
        namespace PetalsOfHope.Editor.Events
        {
            using UnityEditor;
            using UnityEngine;
            using PetalsOfHope.Core.Events; // For IntEventSO

            [CustomEditor(typeof(IntEventSO))]
            public class IntEventSOEditor : Editor
            {
                private int _payloadValue; // Value to send with the event

                public override void OnInspectorGUI()
                {
                    base.OnInspectorGUI(); // Draw default inspector

                    IntEventSO typedEvent = (IntEventSO)target;

                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Debug Raise (Play Mode Only)", EditorStyles.boldLabel);

                    GUI.enabled = Application.isPlaying;

                    _payloadValue = EditorGUILayout.IntField("Payload Value:", _payloadValue);

                    if (GUILayout.Button("Raise Event with Payload"))
                    {
                        if (Application.isPlaying)
                        {
                            typedEvent.Raise(_payloadValue);
                        }
                        else
                        {
                            Debug.LogWarning("Cannot raise TypedEventSO from editor when not in Play Mode. Start the game to test event raising.");
                        }
                    }
                    GUI.enabled = true;
                }
            }
        }
        ```
    *   Implement similar custom editors for other common `TypedEventSO` derivatives like `Vector2EventSOEditor`, `StringEventSOEditor` etc., providing appropriate input fields for their payload types.

3.  **Assembly Definition for Editor Scripts:**
    *   Create an Assembly Definition file in `Assets/_Project/Editor/` named `PetalsOfHope.Editor.asmdef`.
    *   Configure it to reference `PetalsOfHope.Runtime.asmdef` (or whatever the main runtime assembly is named, created in task 1.1.4).
    *   Ensure "Editor" is selected in the "Platforms" section of the asmdef inspector (usually UnityEditor only).

# Acceptance Criteria:
- Custom editor scripts are created for `GameEventSO` and common `TypedEventSO` derivatives (e.g., `IntEventSO`).
- When a `GameEventSO` asset is selected in the Unity Editor (during Play Mode), a "Raise Event" button is visible in the Inspector and functional.
- When a common `TypedEventSO` asset (e.g., `IntEventSO`) is selected (during Play Mode), an appropriate input field for its payload and a "Raise Event with Payload" button are visible and functional.
- Buttons are disabled or show a warning if used outside of Play Mode.
- Editor scripts are placed in an `Editor` folder with a corresponding Assembly Definition file.
- Scripts compile without errors.

# Test Strategy:
- Manual Verification:
    - Select a `GameEventSO` asset in the Inspector during Play Mode. Click the "Raise Event" button and verify any listeners respond.
    - Select an `IntEventSO` asset in the Inspector during Play Mode. Enter a value, click "Raise Event with Payload", and verify listeners respond with the correct payload.
    - Verify buttons are disabled or give feedback when not in Play Mode.
    - Check that editor scripts do not cause compile errors and are correctly located.

# Notes/Questions:
- The `GUI.enabled = Application.isPlaying;` ensures that events are only raised from the editor when the game is running, which is generally safer and more representative of runtime behavior.
- Creating a generic editor for `TypedEventSO<T>` that can handle any `T` is significantly more complex and might involve reflection or custom property drawers for type `T`. Starting with specific common types is a practical approach.
- The Assembly Definition for editor scripts is important to separate editor code from runtime code and manage dependencies.