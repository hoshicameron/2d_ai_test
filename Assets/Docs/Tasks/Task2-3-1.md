# Task ID: 2.3.1
# Parent Task ID: 2.3
# Title: Implement BaseState (Abstract Class)
# Status: pending
# Dependencies: 1.1.2, 1.1.4 # Folder structure and namespace
# Priority: critical
# Estimated Effort: S
# Assignee: Unassigned

# Description:
Implement `BaseState.cs`, an abstract class that defines the common structure and lifecycle methods for all states in the state machine system.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/Core/StateMachine/BaseState.cs`
2.  **Namespace:** `PetalsOfHope.Core.StateMachine`
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/Core/StateMachine/BaseState.cs
    namespace PetalsOfHope.Core.StateMachine
    {
        // Forward declare StateMachine to resolve circular dependency if BaseState needs it early.
        // Or pass it via constructor/methods.
        // public abstract class BaseState<TStateMachine> where TStateMachine : StateMachine
        // For simplicity, as per plan, let's use a non-generic StateMachine reference.
        public abstract class BaseState
        {
            protected StateMachine stateMachine;
            // Optional: Add a reference to the owner GameObject or a specific controller type if common
            // protected GameObject owner;

            // Constructor to pass the StateMachine instance (and optionally owner)
            // This makes 'stateMachine' available as soon as the state is instantiated.
            public BaseState(StateMachine stateMachine)
            {
                this.stateMachine = stateMachine;
            }

            /// <summary>
            /// Called when the state machine enters this state.
            /// </summary>
            public abstract void Enter();

            /// <summary>
            /// Called when the state machine exits this state.
            /// </summary>
            public abstract void Exit();

            /// <summary>
            /// Called every frame while this state is active (from MonoBehaviour.Update).
            /// </summary>
            public abstract void Update();

            /// <summary>
            /// Called every fixed physics frame while this state is active (from MonoBehaviour.FixedUpdate).
            /// </summary>
            public abstract void FixedUpdate();

            // Optional: Add methods for handling animation events or collisions if desired at base level
            // public virtual void OnAnimationEvent(string eventName) {}
            // public virtual void OnCollisionEnter(Collision collision) {}
        }
    }
    ```

# Acceptance Criteria:
- `BaseState.cs` file is created at the specified location and namespace.
- The class is abstract.
- It includes a `protected StateMachine stateMachine;` field.
- It includes a constructor `public BaseState(StateMachine stateMachine)` to initialize the `stateMachine` field.
- It defines abstract methods: `Enter()`, `Exit()`, `Update()`, and `FixedUpdate()`.
- The script compiles without errors.

# Test Strategy:
- This class is abstract and will be tested via its concrete implementations and the `StateMachine` that uses them.
- Manual code review for correctness.

# Notes/Questions:
- The plan mentions `protected StateMachine stateMachine;`. Making this injectable via the constructor is a common and clean pattern.
- The generic version `BaseState<TStateMachine>` could be considered if different types of state machines (e.g., `PlayerStateMachine`, `AIStateMachine`) need to pass specific derived `StateMachine` types to their states. For now, following the simpler non-generic `StateMachine` reference as implied by the plan.
- The `owner` (GameObject or specific controller) could also be passed via constructor if states commonly need direct access beyond what `stateMachine.gameObject` provides.