// In Assets/_Project/Tests/EditMode/Events/GameEventSOTests.cs
namespace PetalsOfHope.Tests.EditMode.Events
{
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools;
    using PetalsOfHope.Core.Events;

    public class GameEventSOTests
    {
        private GameEventSO _gameEvent;
        private bool _eventRaised;

        [SetUp]
        public void Setup()
        {
            _gameEvent = ScriptableObject.CreateInstance<GameEventSO>();
            _eventRaised = false;
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(_gameEvent);
        }

        [Test]
        public void Raise_InvokesRegisteredListener()
        {
            // Arrange
            _gameEvent.RegisterListener(() => _eventRaised = true);

            // Act
            _gameEvent.Raise();

            // Assert
            Assert.IsTrue(_eventRaised, "Listener was not invoked when event was raised.");
        }


        [Test]
        public void Raise_DoesNotInvokeUnregisteredListener()
        {
            // Arrange
            void Listener() => _eventRaised = true;
            
            _gameEvent.RegisterListener(Listener);
            _gameEvent.UnregisterListener(Listener);

            // Act
            _gameEvent.Raise();

            // Assert
            Assert.IsFalse(_eventRaised, "Unregistered listener was invoked when it shouldn't have been.");
        }

        [Test]
        public void Raise_WithNoListeners_DoesNotThrowException()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => _gameEvent.Raise(), 
                "Raising an event with no listeners should not throw an exception.");
        }

        [Test]
        public void RegisterListener_DuplicateRegistration_IsHandled()
        {
            // Arrange
            int callCount = 0;
            void Listener() => callCount++;
            
            // Act
            _gameEvent.RegisterListener(Listener);
            _gameEvent.RegisterListener(Listener); // Register same listener again
            _gameEvent.Raise();

            // Assert
            Assert.AreEqual(1, callCount, "Listener should only be called once even if registered multiple times.");
        }
    }
}
