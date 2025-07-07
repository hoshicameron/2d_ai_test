# Task ID: 2.3.4
# Parent Task ID: 2.3
# Title: Unit Test State Machine Logic
# Status: completed
# Dependencies: 2.3.1, 2.3.2 # StateMachine and BaseState
# Priority: high
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement unit tests for the `StateMachine` system using the Unity Test Framework. Tests should cover initialization, transitions, and the correct invocation of state lifecycle methods (`Enter`, `Exit`, `Update`, `FixedUpdate`).

# Details:
1.  **Setup Test Environment:**
    *   Ensure Unity Test Framework is installed.
    *   Use an Edit Mode test assembly (e.g., `PetalsOfHope.Core.StateMachine.EditModeTests.asmdef` in `Assets/_Project/Tests/EditMode/`).
2.  **Create Mock/Stub `BaseState`:**
    *   Create a helper class for tests, e.g., `MockState.cs`, inheriting from `BaseState`.
    *   This mock state will record calls to its lifecycle methods and allow checking their invocation.
        ```csharp
        // Example MockState (can be an inner class in the test file)
        public class MockState : BaseState
        {
            public int EnterCount { get; private set; }
            public int ExitCount { get; private set; }
            public int UpdateCount { get; private set; }
            public int FixedUpdateCount { get; private set; }
            public StateMachine OwningStateMachine => base.stateMachine; // Expose for testing

            public MockState(StateMachine sm) : base(sm) {}

            public override void Enter() { EnterCount++; }
            public override void Exit() { ExitCount++; }
            public override void Update() { UpdateCount++; }
            public override void FixedUpdate() { FixedUpdateCount++; }
            public void ResetCounters() 
            {
                EnterCount = 0; ExitCount = 0; UpdateCount = 0; FixedUpdateCount = 0;
            }
        }
        ```
3.  **Create Test Script `StateMachineTests.cs`:**
    *   File Location: `Assets/_Project/Tests/EditMode/StateMachine/StateMachineTests.cs`
    *   **Test Scenarios:**
        *   `Initialize_SetsCurrentStateAndCallsEnter`: Verify `CurrentState` is set and `Enter()` is called once on the initial state.
        *   `ChangeState_CallsExitOnOldAndEnterOnNew`: Verify `Exit()` on the old state and `Enter()` on the new state are each called once.
        *   `ChangeState_ToNull_LogsErrorAndDoesNotChange`: Verify behavior with null new state.
        *   `Update_CallsCurrentStateUpdate`: Verify `StateMachine.Update()` calls `_currentState.Update()`.
        *   `FixedUpdate_CallsCurrentStateFixedUpdate`: Verify `StateMachine.FixedUpdate()` calls `_currentState.FixedUpdate()`.
        *   `StateMachine_NotInitialized_UpdateAndFixedUpdateDoNothing`: Verify no calls if not initialized.
        *   `ChangeState_WhenNotInitialized_InitializesWithNewState`: Verify this specific behavior.
        *   `ChangeState_ToSameState_DoesNotReEnterByDefault`: Verify no Exit/Enter calls if changing to the same state instance.
        *   `OnDestroy_CallsExitOnCurrentState`: Requires testing MonoBehaviour lifecycle, might be simpler to test manually or with Play Mode test if Edit Mode setup is tricky for `OnDestroy`. (Often `OnDestroy` behavior for pure C# logic within it is tested by directly calling it if possible, or by ensuring the stateful members it acts on are as expected before/after).

4.  **Test Implementation Example (Conceptual):**
    ```csharp
    // In StateMachineTests.cs
    using NUnit.Framework;
    using UnityEngine;
    using PetalsOfHope.Core.StateMachine;

    public class StateMachineTests
    {
        private GameObject _smGameObject;
        private StateMachine _stateMachine;
        private MockState _mockState1;
        private MockState _mockState2;

        [SetUp]
        public void SetUp()
        {
            _smGameObject = new GameObject("TestStateMachineGO");
            _stateMachine = _smGameObject.AddComponent<StateMachine>();
            _mockState1 = new MockState(_stateMachine);
            _mockState2 = new MockState(_stateMachine);
        }

        [TearDown]
        public void TearDown()
        {
            GameObject.DestroyImmediate(_smGameObject);
        }

        [Test]
        public void Initialize_SetsCurrentStateAndCallsEnter()
        {
            _stateMachine.Initialize(_mockState1);

            Assert.AreEqual(_mockState1, _stateMachine.CurrentState);
            Assert.AreEqual(1, _mockState1.EnterCount);
        }

        [Test]
        public void ChangeState_CallsExitOnOldAndEnterOnNew()
        {
            _stateMachine.Initialize(_mockState1);
            _mockState1.ResetCounters(); // Reset after Initialize's Enter call

            _stateMachine.ChangeState(_mockState2);

            Assert.AreEqual(1, _mockState1.ExitCount);
            Assert.AreEqual(1, _mockState2.EnterCount);
            Assert.AreEqual(_mockState2, _stateMachine.CurrentState);
        }
        
        [Test]
        public void Update_CallsCurrentStateUpdate()
        {
            _stateMachine.Initialize(_mockState1);
            _smGameObject.SendMessage("Update"); // Simulate MonoBehaviour Update call

            Assert.AreEqual(1, _mockState1.UpdateCount);
        }

        // ... more tests ...
    }
    ```

# Acceptance Criteria:
- Unit test script `StateMachineTests.cs` is created.
- A `MockState` or similar test double is used to verify state lifecycle calls.
- Tests cover initialization, state transitions (including to null or same state), `Update`/`FixedUpdate` propagation, and behavior when uninitialized.
- All implemented unit tests pass in the Unity Test Runner.

# Test Strategy:
- Run tests via Unity Test Runner (`Window > General > Test Runner`).
- Ensure `GameObject.DestroyImmediate` is used in `TearDown` for Edit Mode tests that create GameObjects.
- Simulate MonoBehaviour lifecycle methods like `Update` and `FixedUpdate` using `SendMessage` or by manually calling if the StateMachine methods are public (though they are typically private and called by Unity). For `Update`/`FixedUpdate`, directly calling is fine if the StateMachine's own `Update`/`FixedUpdate` are simple passthroughs. The current implementation has them as private, so `SendMessage` is a way, or make them internal and use `[InternalsVisibleTo]` for the test assembly. For simplicity, if the `StateMachine`'s Update/FixedUpdate methods were made public/internal for testing this would be easier, but testing via `SendMessage` also works for `MonoBehaviour` methods.

# Notes/Questions:
- Simulating `Update` and `FixedUpdate` calls in Edit Mode tests can be done with `SendMessage("Update")` on the GameObject, or by making the StateMachine's `Update`/`FixedUpdate` methods `internal` and using `[assembly: InternalsVisibleTo("YourTestAssemblyName")]` to call them directly from tests. The `SendMessage` approach is cleaner for respecting MonoBehaviour encapsulation.
- Testing `OnDestroy` in Edit Mode can be tricky. It might be easier to verify the state before a hypothetical destroy (e.g. `_currentState.Exit()` was called) by invoking it manually if the logic within is the main concern, or rely on integration/manual tests for full lifecycle including destruction.