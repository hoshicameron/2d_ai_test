# Task ID: 2.1.2
# Parent Task ID: 2.1
# Title: Implement InputReader ScriptableObject
# Status: completed
# Dependencies: 2.1.1, 1.2.3, 1.2.8 # PlayerInputActions asset, TypedEventSO, Event SO Assets
# Priority: critical
# Estimated Effort: L
# Assignee: Unassigned

# Description:
Implement `InputReader.cs` as a ScriptableObject that initializes `PlayerInputActions`, sets up callbacks for input actions, and raises `GameEventSO` or `TypedEventSO<T>` events in response to input.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/Core/Input/InputReader.cs`
2.  **Namespace:** `PetalsOfHope.Core.Input`
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/Core/Input/InputReader.cs
    namespace PetalsOfHope.Core.Input
    {
        using UnityEngine;
        using UnityEngine.InputSystem;
        using PetalsOfHope.Core.Events; // For GameEventSO and TypedEventSO derivatives

        [CreateAssetMenu(menuName = "Petals of Hope/Input/Input Reader", fileName = "NewInputReaderSO")]
        public class InputReader : ScriptableObject, PlayerInputActions.IGameplayActions, PlayerInputActions.IUIActions
        {
            // Gameplay Events
            [Header("Gameplay Input Events")]
            public TypedEventSO<Vector2> MoveEvent;         // For continuous movement input
            public GameEventSO JumpEvent;                   // For jump pressed
            public GameEventSO JumpCancelledEvent;          // For jump released (variable jump height)
            public GameEventSO DashEvent;                   // For dash pressed
            public GameEventSO InteractEvent;               // For interact pressed

            // UI Events
            [Header("UI Input Events")]
            public TypedEventSO<Vector2> UINavigateEvent;   // For UI navigation
            public GameEventSO UISubmitEvent;               // For UI submit
            public GameEventSO UICancelEvent;               // For UI cancel
            // public GameEventSO UIPauseEvent; // Consider adding a dedicated Pause event if needed globally

            private PlayerInputActions _playerInputActions;

            private void OnEnable()
            {
                if (_playerInputActions == null)
                {
                    _playerInputActions = new PlayerInputActions();

                    _playerInputActions.Gameplay.SetCallbacks(this);
                    _playerInputActions.UI.SetCallbacks(this);
                }
                // Default to gameplay input, or UI input if that's the game's initial state (e.g. main menu)
                // EnableGameplayInput(); // Or based on game state
            }

            private void OnDisable()
            {
                DisableAllInput();
            }

            public void EnableGameplayInput()
            {
                _playerInputActions.UI.Disable();
                _playerInputActions.Gameplay.Enable();
                Debug.Log("Gameplay Input Enabled");
            }

            public void EnableUIInput()
            {
                _playerInputActions.Gameplay.Disable();
                _playerInputActions.UI.Enable();
                Debug.Log("UI Input Enabled");
            }

            public void DisableAllInput()
            {
                _playerInputActions.Gameplay.Disable();
                _playerInputActions.UI.Disable();
                Debug.Log("All Input Disabled");
            }

            // IGameplayActions Implementation
            public void OnMove(InputAction.CallbackContext context)
            {
                MoveEvent?.Raise(context.ReadValue<Vector2>());
            }

            public void OnJump(InputAction.CallbackContext context)
            {
                if (context.phase == InputActionPhase.Performed)
                    JumpEvent?.Raise();
                else if (context.phase == InputActionPhase.Canceled)
                    JumpCancelledEvent?.Raise();
            }

            public void OnDash(InputAction.CallbackContext context)
            {
                if (context.phase == InputActionPhase.Performed)
                    DashEvent?.Raise();
            }

            public void OnInteract(InputAction.CallbackContext context)
            {
                if (context.phase == InputActionPhase.Performed)
                    InteractEvent?.Raise();
            }

            // IUIActions Implementation
            public void OnNavigate(InputAction.CallbackContext context)
            {
                UINavigateEvent?.Raise(context.ReadValue<Vector2>());
            }

            public void OnSubmit(InputAction.CallbackContext context)
            {
                if (context.phase == InputActionPhase.Performed)
                    UISubmitEvent?.Raise();
            }

            public void OnCancel(InputAction.CallbackContext context)
            {
                if (context.phase == InputActionPhase.Performed)
                    UICancelEvent?.Raise();
            }
        }
    }
    ```

# Acceptance Criteria:
- `InputReader.cs` ScriptableObject is created and implements `PlayerInputActions.IGameplayActions` and `PlayerInputActions.IUIActions`.
- It has serialized fields for `TypedEventSO<Vector2>` (Move, Navigate) and `GameEventSO` (Jump, JumpCancelled, Dash, Interact, Submit, Cancel).
- `OnEnable` initializes `PlayerInputActions` and sets callbacks.
- Input action callbacks (e.g., `OnMove`, `OnJump`) correctly raise their associated ScriptableObject events with payload if necessary.
- `Jump` action differentiates between `Performed` (JumpEvent) and `Canceled` (JumpCancelledEvent).
- Methods `EnableGameplayInput()`, `EnableUIInput()`, and `DisableAllInput()` correctly switch or disable action maps.
- Script compiles without errors.

# Test Strategy:
- Manual Verification (in conjunction with Task 2.1.3):
    - Create an `InputReader` SO asset.
    - Assign the pre-created event SOs (from Task 1.2.8, or create new ones like `PlayerMoveInputEventSO`, `PlayerJumpEventSO`, etc.) to the `InputReader`'s fields.
    - Use a test scene with `EventListener` components listening to these events and logging to the console.
    - Call `EnableGameplayInput()` or `EnableUIInput()` on the `InputReader` instance (e.g., from a test script).
    - Press keys/gamepad buttons defined in `PlayerInputActions` and verify the correct events are logged by listeners.
    - Test switching between action maps.

# Notes/Questions:
- `JumpCancelledEvent` is added to support variable jump height (player releases jump button mid-air).
- The `InputReader` needs to be initialized, typically by some game manager that holds a reference to its SO asset and calls `EnableGameplayInput()` or `EnableUIInput()` at appropriate times (e.g., game start, opening a menu).
- `OnEnable` is a good place to initialize `_playerInputActions`, but the actual enabling of an action map should be deliberate (e.g., by a game manager). Added a commented out `EnableGameplayInput()` as an example.
- The `InputReader` SO asset itself needs to be created in the project (Task 2.1.3).