// In Assets/_Project/Tests/EditMode/Events/TypedEventSOTests.cs
namespace PetalsOfHope.Tests.EditMode.Events
{
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools;
    using PetalsOfHope.Core.Events;

    public class TypedEventSOTests
    {
        private IntEventSO _typedEvent;
        private int _receivedPayload;
        private bool _eventRaised;

        [SetUp]
        public void Setup()
        {
            _typedEvent = ScriptableObject.CreateInstance<IntEventSO>();
            _receivedPayload = -1;
            _eventRaised = false;
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(_typedEvent);
        }

        [Test]
        public void Raise_InvokesRegisteredListener_WithCorrectPayload()
        {
            // Arrange
            const int expectedPayload = 42;
            _typedEvent.RegisterListener(payload => {
                _receivedPayload = payload;
                _eventRaised = true;
            });

            // Act
            _typedEvent.Raise(expectedPayload);


            // Assert
            Assert.IsTrue(_eventRaised, "Listener was not invoked when event was raised.");
            Assert.AreEqual(expectedPayload, _receivedPayload, "Listener did not receive the correct payload.");
        }

        [Test]
        public void Raise_DoesNotInvokeUnregisteredListener()
        {
            // Arrange
            void Listener(int payload) => _eventRaised = true;
            
            _typedEvent.RegisterListener(Listener);
            _typedEvent.UnregisterListener(Listener);

            // Act
            _typedEvent.Raise(42);


            // Assert
            Assert.IsFalse(_eventRaised, "Unregistered listener was invoked when it shouldn't have been.");
        }

        [Test]
        public void Raise_WithNoListeners_DoesNotThrowException()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => _typedEvent.Raise(42), 
                "Raising an event with no listeners should not throw an exception.");
        }

        [Test]
        public void RegisterListener_DuplicateRegistration_IsHandled()
        {
            // Arrange
            int callCount = 0;
            void Listener(int payload) => callCount++;
            
            // Act
            _typedEvent.RegisterListener(Listener);
            _typedEvent.RegisterListener(Listener); // Register same listener again
            _typedEvent.Raise(42);

            // Assert
            Assert.AreEqual(1, callCount, "Listener should only be called once even if registered multiple times.");
        }
    }
}
