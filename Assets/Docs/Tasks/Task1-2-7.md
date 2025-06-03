# Task ID: 1.2.7
# Parent Task ID: 1.2
# Title: Unit Test Event Propagation
# Status: completed
# Dependencies: 1.2.2, 1.2.3, 1.2.4, 1.2.5 # Event SOs and Listeners
# Priority: high
# Estimated Effort: L
# Assignee: Unassigned

# Description:
Implement unit tests for the event bus system using the Unity Test Framework. Tests should cover listener registration, unregistration, and the correct invocation of events (both parameterless and typed) and their listeners.

# Details:
1.  **Setup Unity Test Framework:**
    *   Ensure the "Test Framework" package is installed via `Window > Package Manager`.
    *   Create test assemblies:
        *   `Assets/_Project/Tests/EditMode/PetalsOfHope.Core.Events.EditModeTests.asmdef`
        *   `Assets/_Project/Tests/PlayMode/PetalsOfHope.Core.Events.PlayModeTests.asmdef` (if play mode specific tests are needed for listeners)
2.  **Edit Mode Tests for `GameEventSO`:**
    *   Create test script: `Assets/_Project/Tests/EditMode/Events/GameEventSOTests.cs`
    *   **Test Scenarios:**
        *   `RegisterListener_AddsListenerCorrectly`: Verify listener is added.
        *   `UnregisterListener_RemovesListenerCorrectly`: Verify listener is removed.
        *   `Raise_InvokesRegisteredListener`: Verify listener's method is called.
        *   `Raise_DoesNotInvokeUnregisteredListener`: Verify unreg'd listener is not called.
        *   `Raise_WithNoListeners_DoesNotThrowException`.
        *   `RegisterListener_DuplicateRegistration_IsHandled`: Ensure listener is not added multiple times or behaves correctly.
    *   **Example Test Method (Conceptual):**
        ```csharp
        // In GameEventSOTests.cs
        [Test]
        public void Raise_InvokesRegisteredListener()
        {
            var gameEvent = ScriptableObject.CreateInstance<GameEventSO>();
            bool eventRaised = false;
            Action listener = () => eventRaised = true;

            gameEvent.RegisterListener(listener);
            gameEvent.Raise();

            Assert.IsTrue(eventRaised, "Listener was not invoked when event was raised.");
        }
        ```
3.  **Edit Mode Tests for `TypedEventSO<T>`:**
    *   Create test script: `Assets/_Project/Tests/EditMode/Events/TypedEventSOTests.cs`
    *   Use concrete instances like `IntEventSO` for testing.
    *   **Test Scenarios (similar to `GameEventSO` but with payload):**
        *   `RegisterListener_AddsTypedListenerCorrectly`.
        *   `UnregisterListener_RemovesTypedListenerCorrectly`.
        *   `Raise_InvokesRegisteredTypedListener_WithCorrectPayload`.
        *   `Raise_DoesNotInvokeUnregisteredTypedListener`.
        *   `Raise_WithNoTypedListeners_DoesNotThrowException`.
    *   **Example Test Method (Conceptual):**
        ```csharp
        // In TypedEventSOTests.cs
        [Test]
        public void Raise_InvokesRegisteredTypedListener_WithCorrectPayload()
        {
            var typedEvent = ScriptableObject.CreateInstance<IntEventSO>();
            int receivedPayload = -1;
            Action<int> listener = (payload) => receivedPayload = payload;
            int expectedPayload = 10;

            typedEvent.RegisterListener(listener);
            typedEvent.Raise(expectedPayload);

            Assert.AreEqual(expectedPayload, receivedPayload, "Listener was not invoked with the correct payload.");
        }
        ```
4.  **Play Mode Tests for `EventListener` and `TypedEventListener<T>` (Optional but Recommended):**
    *   Create test script: `Assets/_Project/Tests/PlayMode/Events/EventListenerTests.cs`
    *   These tests would involve instantiating GameObjects with listener components.
    *   **Test Scenarios:**
        *   `EventListener_RespondsToGameEventSO_WhenEnabled`.
        *   `EventListener_DoesNotRespond_WhenDisabled`.
        *   `EventListener_DoesNotRespond_AfterEventUnregistered`.
        *   `TypedEventListener_RespondsToTypedEventSO_WithPayload_WhenEnabled`.
    *   Use `UnityTest` attribute and `yield return null;` for coroutine-based tests if needed.

# Acceptance Criteria:
- Unit test scripts are created for `GameEventSO` and `TypedEventSO<T>` (using concrete examples like `IntEventSO`).
- Tests cover registration, unregistration, event raising, and payload delivery for typed events.
- All implemented unit tests pass successfully in the Unity Test Runner.
- (If implemented) Play mode tests for listener components pass.
- Test scripts are organized within the `Assets/_Project/Tests/` folder structure with appropriate assembly definitions.

# Test Strategy:
- Run all tests using the Unity Test Runner (`Window > General > Test Runner`).
- Ensure tests are executed in both Edit Mode and Play Mode (if Play Mode tests are written).
- Review test coverage to ensure critical paths of the event system are tested.

# Notes/Questions:
- `ScriptableObject.CreateInstance<T>()` is used to create event instances for testing without needing asset files.
- Play Mode tests for `EventListener` components are more complex as they require scene setup (GameObjects, components). Consider if Edit Mode tests for the SOs themselves provide sufficient confidence for this stage. The plan asks for "Unit test event propagation (listener registration, unregistration, raising events)", which primarily targets the SOs.
- Mocking/stubbing might be useful for more complex listener interactions if they call out to other services.