# Task ID: 2.3.2
# Parent Task ID: 2.3
# Title: Implement StateMachine (MonoBehaviour)
# Status: completed
# Dependencies: 2.3.1 # BaseState definition
# Priority: critical
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement `StateMachine.cs`, a MonoBehaviour that manages the current state, calls its lifecycle methods (`Enter`, `Exit`, `Update`, `FixedUpdate`), and provides a method to change states.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/Core/StateMachine/StateMachine.cs`
2.  **Namespace:** `PetalsOfHope.Core.StateMachine`
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/Core/StateMachine/StateMachine.cs
    namespace PetalsOfHope.Core.StateMachine
    {
        using UnityEngine;

        public class StateMachine : MonoBehaviour
        {
            private BaseState _currentState;
            public BaseState CurrentState => _currentState; // Public getter for inspection/debugging

            private bool _isInitialized = false;

            /// <summary>
            /// Initializes the state machine with a starting state.
            /// Must be called before the state machine can operate.
            /// </summary>
            /// <param name="startingState">The initial state for the machine.</param>
            public void Initialize(BaseState startingState)
            {
                if (startingState == null)
                {
                    Debug.LogError("Cannot initialize StateMachine with a null startingState.", this);
                    return;
                }
                _currentState = startingState;
                _currentState.Enter();
                _isInitialized = true;
                // Debug.Log($"StateMachine initialized with state: {startingState.GetType().Name}", this);
            }

            /// <summary>
            /// Changes the current state of the machine.
            /// Calls Exit() on the current state and Enter() on the new state.
            /// </summary>
            /// <param name="newState">The state to transition to.</param>
            public void ChangeState(BaseState newState)
            {
                if (newState == null)
                {
                    Debug.LogError("Cannot change to a null state.", this);
                    return;
                }

                if (!_isInitialized)
                {
                    Debug.LogWarning("StateMachine not initialized. Call Initialize(startingState) first. Will initialize with this new state.", this);
                    Initialize(newState); // Or handle as an error
                    return;
                }
                
                if (_currentState == newState)
                {
                    // Debug.LogWarning($"Attempting to change to the same state: {newState.GetType().Name}. No transition performed.", this);
                    return; // Optionally allow re-entering same state if needed by calling Exit then Enter.
                }

                _currentState?.Exit();
                // Debug.Log($"Exited state: {_currentState?.GetType().Name ?? "None"}. Entering state: {newState.GetType().Name}", this);
                _currentState = newState;
                _currentState.Enter();
            }

            private void Update()
            {
                if (!_isInitialized || _currentState == null) return;
                _currentState.Update();
            }

            private void FixedUpdate()
            {
                if (!_isInitialized || _currentState == null) return;
                _currentState.FixedUpdate();
            }

            private void OnDestroy()
            {
                // Ensure the current state's Exit method is called when the StateMachine is destroyed
                // This is important for cleanup if the GameObject is destroyed while a state is active.
                if (_isInitialized && _currentState != null)
                {
                    _currentState.Exit();
                    _currentState = null;
                }
            }
        }
    }
    ```

# Acceptance Criteria:
- `StateMachine.cs` MonoBehaviour is created at the specified location and namespace.
- It contains a private field `_currentState` of type `BaseState` and a public getter.
- It has an `Initialize(BaseState startingState)` method that sets the initial state and calls its `Enter()` method.
- It has a `ChangeState(BaseState newState)` method that calls `Exit()` on the old state (if any) and `Enter()` on the new state.
- `Update()` and `FixedUpdate()` methods on the `StateMachine` call the respective methods of the `_currentState` if it's not null and the machine is initialized.
- `OnDestroy()` calls `Exit()` on the current state.
- Handles null states and uninitialized machine with warnings/errors.
- Script compiles without errors.

# Test Strategy:
- Unit Testing (Task 2.3.4):
    - Create mock `BaseState` implementations.
    - Test `Initialize()`: verify `Enter()` is called on the starting state.
    - Test `ChangeState()`: verify `Exit()` is called on the old state and `Enter()` on the new state.
    - Test that `Update()` and `FixedUpdate()` on `StateMachine` correctly call the methods on the current mock state.
    - Test changing to the same state (should ideally not re-enter unless designed to).
    - Test `OnDestroy()` calls `Exit()`.
- Manual/Integration Testing:
    - Create a GameObject, attach `StateMachine.cs`.
    - Create simple `BaseState` derived classes (e.g., `TestStateA`, `TestStateB`) that log messages in `Enter`, `Exit`, `Update`.
    - In a separate script, get the `StateMachine` component, instantiate states (passing the `StateMachine` instance to their constructors), and call `Initialize()` and `ChangeState()` at various times (e.g., on key press).
    - Observe console logs to verify correct state lifecycle execution.

# Notes/Questions:
- The `Initialize()` method is crucial; the state machine shouldn't operate before it's called.
- Added a check in `ChangeState` to prevent changing to the same state by default, as this is often unintended. If re-entering the same state is a desired behavior, this check can be modified or removed.
- The `OnDestroy()` method ensures proper cleanup of the current state when the GameObject owning the `StateMachine` is destroyed.
- The public getter for `CurrentState` is useful for debugging and external systems that might need to query the current state type (though direct interaction should be minimized in favor of event-driven or component-based communication).