# Task ID: 3.2.1
# Parent Task ID: 3.2
# Title: Define AI State Base Class (ScriptableObject or Class)
# Status: completed
# Dependencies: 2.3.2 # Core StateMachine (for reference to AI's SM if it derives)
# Priority: critical
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Define `State.cs` within the `PetalsOfHope.AI.Core` namespace. This will be an abstract `ScriptableObject` (or class) that serves as the base for all AI-specific states.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/AI/Core/State.cs`
2.  **Namespace:** `PetalsOfHope.AI.Core`
3.  **Implementation (as ScriptableObject, as per plan's suggestion):**
    ```csharp
    // In Assets/_Project/Scripts/AI/Core/State.cs
    namespace PetalsOfHope.AI.Core
    {
        using UnityEngine;
        // Forward declare AI.StateMachine to avoid compilation order issues.
        // The actual AI.StateMachine type will be used as parameter.
        // using PetalsOfHope.AI; 

        public abstract class State : ScriptableObject
        {
            /// <summary>
            /// Called when the AI StateMachine enters this state.
            /// </summary>
            /// <param name="ownerStateMachine">The AI StateMachine that owns this state instance.</param>
            public abstract void EnterState(StateMachine ownerStateMachine);

            /// <summary>
            /// Called every frame while this state is active.
            /// Contains the main logic for the state.
            /// </summary>
            /// <param name="ownerStateMachine">The AI StateMachine that owns this state instance.</param>
            public abstract void ExecuteState(StateMachine ownerStateMachine);

            /// <summary>
            /// Called when the AI StateMachine exits this state.
            /// </summary>
            /// <param name="ownerStateMachine">The AI StateMachine that owns this state instance.</param>
            public abstract void ExitState(StateMachine ownerStateMachine);

            // Optional: Method to check for transitions.
            // public virtual State CheckTransitions(StateMachine ownerStateMachine) { return null; }
            // Transitions can also be handled within ExecuteState.
        }
    }
    ```
    *   **Clarification on `StateMachine` type**: The plan refers to `StateMachine owner` where `owner` is an `AI.StateMachine`. The `AI.StateMachine` will be defined in Task 3.2.2.
    *   The methods take `PetalsOfHope.AI.StateMachine` as parameter, not `PetalsOfHope.Core.StateMachine.StateMachine`.

# Acceptance Criteria:
- `State.cs` (AI state base) is created as an abstract `ScriptableObject` (or class if preferred, but SO matches plan) in the specified location and namespace.
- It defines abstract methods: `EnterState(AI.StateMachine owner)`, `ExecuteState(AI.StateMachine owner)`, and `ExitState(AI.StateMachine owner)`.
- The script compiles without errors (pending `AI.StateMachine` definition).

# Test Strategy:
- This abstract class will be tested via its concrete implementations (Patrol, Chase, Attack states) and the AI `StateMachine`.
- Code review for method signatures and structure.

# Notes/Questions:
- Using `ScriptableObject` for states allows designers to create and configure different variations of AI behaviors as assets (e.g., "CautiousPatrolState", "AggressivePatrolState").
- The `ownerStateMachine` parameter in state methods will provide access to the enemy's components (`EnemyBase`, `AnimationController`, etc.) via the AI `StateMachine`.
- The plan clearly distinguishes this `AI.Core.State` from `Core.StateMachine.BaseState`.