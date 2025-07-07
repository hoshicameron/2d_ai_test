# Task ID: 2.3.3
# Parent Task ID: 2.3
# Title: (Optional) Implement Transition System Base Classes
# Status: completed
# Dependencies: 2.3.1, 2.3.2 # BaseState and StateMachine
# Priority: low
# Estimated Effort: M
# Assignee: Unassigned

# Description:
(Optional) Implement a base `Transition.cs` class and a sample `InputTransition.cs` to demonstrate a more data-driven or condition-based approach to state transitions.

# Details:
This task is optional as per the Implementation Plan. If undertaken:
1.  **`Transition.cs` (Base Class):**
    *   File Location: `Assets/_Project/Scripts/Core/StateMachine/Transitions/Transition.cs` (Create `Transitions` subfolder)
    *   Namespace: `PetalsOfHope.Core.StateMachine.Transitions`
    *   This class would define the basic structure for a transition.
    *   Could be a `ScriptableObject` to define transitions in the editor, or a regular C# class.
    *   **Properties:**
        *   `targetState`: The `BaseState` (or type/reference to it) to transition to if conditions are met.
        *   `conditions`: A list of `ICondition` objects (another interface/abstract class to define).
    *   **Methods:**
        *   `ShouldTransition(StateMachine owner)`: Checks all conditions. Returns true if all conditions met.

2.  **`ICondition.cs` (Interface or Abstract Class):**
    *   File Location: `Assets/_Project/Scripts/Core/StateMachine/Transitions/Conditions/ICondition.cs`
    *   Namespace: `PetalsOfHope.Core.StateMachine.Transitions.Conditions`
    *   Method: `IsMet(StateMachine owner)`: Returns true if this specific condition is met.

3.  **`InputTransition.cs` (Example Concrete Transition):**
    *   File Location: `Assets/_Project/Scripts/Core/StateMachine/Transitions/InputTransition.cs`
    *   Namespace: `PetalsOfHope.Core.StateMachine.Transitions`
    *   This would inherit from `Transition` and use a specific `InputCondition`.

4.  **`InputCondition.cs` (Example Concrete Condition):**
    *   File Location: `Assets/_Project/Scripts/Core/StateMachine/Transitions/Conditions/InputCondition.cs`
    *   Namespace: `PetalsOfHope.Core.StateMachine.Transitions.Conditions`
    *   Implements `ICondition`.
    *   Checks for a specific input action (e.g., jump button pressed). It might need access to an `InputReader` or similar.

5.  **Modify `BaseState` or `StateMachine`:**
    *   `BaseState` could hold a list of `Transition` objects.
    *   In `BaseState.Update()`, iterate through its transitions. If `transition.ShouldTransition()` is true, call `stateMachine.ChangeState(transition.targetState)`.
    *   Alternatively, the `StateMachine` itself could manage transitions associated with states.

# Acceptance Criteria:
- (If implemented) `Transition.cs` base class (or SO) and `ICondition.cs` interface are defined.
- (If implemented) An example `InputTransition.cs` and `InputCondition.cs` are created.
- (If implemented) `BaseState` or `StateMachine` is modified to use this transition system.
- (If implemented) Transitions can be defined (either in code or as assets) and trigger state changes based on conditions.

# Test Strategy:
- Manual/Integration Testing:
    - Extend the `StateMachine` test setup from Task 2.3.2.
    - Create states with defined transitions and conditions (e.g., an `InputTransition` that changes state on a key press).
    - Verify that the state machine automatically transitions when conditions are met.

# Notes/Questions:
- This provides a more flexible and often more designer-friendly way to manage state transitions, especially for complex AI or player controllers.
- The implementation can vary widely (ScriptableObject-based, pure C#).
- For the scope of "Petals of Hope" as outlined, the simpler direct `stateMachine.ChangeState(new ConcreteState(this.stateMachine))` calls within states might be sufficient and is often how Unity state machines start. The plan lists this as "(Optional) Implement Transition.cs base class and InputTransition.cs example."
- Given this is optional and adds complexity, it might be deferred unless explicitly requested or found necessary later. For now, I will mark it as low priority and assume the simpler explicit `ChangeState` calls will be used in player/enemy states.