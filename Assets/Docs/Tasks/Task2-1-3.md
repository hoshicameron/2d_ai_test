# Task ID: 2.1.3
# Parent Task ID: 2.1
# Title: Create and Configure InputReader SO Asset & Test Input
# Status: pending
# Dependencies: 2.1.2, 1.2.8 # InputReader script, EventSO assets
# Priority: critical
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Create a ScriptableObject asset instance of the `InputReader` and link the necessary `EventSO` assets to its fields. Test basic input propagation with keyboard and gamepad.

# Details:
1.  **Create `InputReader` SO Asset:**
    *   Navigate to `Assets/_Project/ScriptableObjects/Input/`.
    *   Right-click -> `Create > Petals of Hope/Input/Input Reader`.
    *   Name it `PlayerInputReaderSO` (or similar).
2.  **Configure Event Links:**
    *   Select the `PlayerInputReaderSO` asset.
    *   In the Inspector, assign the corresponding `GameEventSO` and `TypedEventSO<T>` assets to its public event fields.
    *   Example Event SOs (ensure these exist or create them as per Task 1.2.8 or new ones specific to input):
        *   `MoveEvent`: Assign a `Vector2EventSO` (e.g., `PlayerMoveInputEventSO`).
        *   `JumpEvent`: Assign a `GameEventSO` (e.g., `PlayerJumpPressedEventSO`).
        *   `JumpCancelledEvent`: Assign a `GameEventSO` (e.g., `PlayerJumpCancelledEventSO`).
        *   `DashEvent`: Assign a `GameEventSO` (e.g., `PlayerDashPressedEventSO`).
        *   `InteractEvent`: Assign a `GameEventSO` (e.g., `PlayerInteractPressedEventSO`).
        *   `UINavigateEvent`: Assign a `Vector2EventSO` (e.g., `UINavigateInputEventSO`).
        *   `UISubmitEvent`: Assign a `GameEventSO` (e.g., `UISubmitPressedEventSO`).
        *   `UICancelEvent`: Assign a `GameEventSO` (e.g., `UICancelPressedEventSO`).
3.  **Create Test Scene for Input:**
    *   Create a new scene: `Assets/_Project/Scenes/Testing/InputTestScene.unity`.
    *   Create an empty GameObject named `_InputTestManager`.
    *   Add a simple script `InputTestManager.cs` to it:
        ```csharp
        // In Assets/_Project/Scripts/Core/Input/InputTestManager.cs (example location)
        namespace PetalsOfHope.Core.Input.Test
        {
            using UnityEngine;

            public class InputTestManager : MonoBehaviour
            {
                public InputReader playerInputReader; // Assign your SO asset here

                void Start()
                {
                    if (playerInputReader == null)
                    {
                        Debug.LogError("InputReader SO not assigned to InputTestManager!");
                        return;
                    }
                    // Start with gameplay input enabled, or UI, depending on what you want to test first.
                    playerInputReader.EnableGameplayInput();
                }

                // Optional: methods to switch input modes via UI buttons in the test scene
                public void SetGameplayInput() => playerInputReader?.EnableGameplayInput();
                public void SetUIInput() => playerInputReader?.EnableUIInput();
                public void DisableAllInput() => playerInputReader?.DisableAllInput();
            }
        }
        ```
    *   Assign your `PlayerInputReaderSO` to the `playerInputReader` field on the `_InputTestManager` GameObject.
4.  **Add Event Listeners for Debugging:**
    *   On `_InputTestManager` or separate GameObjects, add `EventListener` and `TypedEventListener<T>` components.
    *   Configure these listeners to listen to the events assigned in `PlayerInputReaderSO`.
    *   For their responses, use `UnityEvent.AddListener` to call `Debug.Log` or simple methods that print the input value.
        *   Example for MoveEvent (Vector2): Create a `Vector2EventListener`, assign the `PlayerMoveInputEventSO`. For the response, create a public method `LogMove(Vector2 moveVal)` in `InputTestManager` that does `Debug.Log("Move: " + moveVal);` and link it.
5.  **Test Input:**
    *   Enter Play Mode in `InputTestScene`.
    *   Test Keyboard inputs for Gameplay actions: WASD, Space, Left Shift, E.
    *   Test Gamepad inputs for Gameplay actions: Left Stick, Button South, Button West, Button North.
    *   Verify console logs show correct events being raised with correct data.
    *   If UI buttons are added to switch modes, test `EnableUIInput()`. Then test UI inputs (Navigate, Submit, Cancel) with both keyboard and gamepad.

# Acceptance Criteria:
- An `InputReader` SO asset (`PlayerInputReaderSO`) is created and configured with all necessary `EventSO` assets.
- A test scene (`InputTestScene`) is created to demonstrate input handling.
- In the test scene, keyboard inputs for `Move`, `Jump` (pressed/cancelled), `Dash`, `Interact` trigger the corresponding events via `InputReader`.
- In the test scene, gamepad inputs for `Move`, `Jump` (pressed/cancelled), `Dash`, `Interact` trigger the corresponding events.
- Input mode switching (`EnableGameplayInput`, `EnableUIInput`) functions correctly, enabling/disabling appropriate actions.
- UI inputs (`Navigate`, `Submit`, `Cancel`) trigger events when UI input mode is active.

# Test Strategy:
- As detailed in step 5 ("Test Input") above.
- Pay attention to `JumpCancelledEvent` being triggered on key/button release for Jump.
- Check `MoveEvent` payload values (Vector2) for correctness (e.g., (0,1) for W, (-1,0) for A).

# Notes/Questions:
- This task confirms the entire input pipeline from raw hardware input -> Unity Input System -> `PlayerInputActions` -> `InputReader` -> Event Bus -> Event Listeners.
- Ensure the EventSO assets used here have unique, descriptive names to avoid confusion (e.g., `PlayerMoveInputEventSO` rather than a generic `Vector2EventSO` used for other purposes).