// In Assets/_Project/Tests/EditMode/Events/EventListenerTests.cs
namespace PetalsOfHope.Tests.EditMode.Events
{
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.TestTools;
    using PetalsOfHope.Core.Events;
    using PetalsOfHope.Core.Events.Listeners;

    public class EventListenerTests
    {
        private GameEventSO _gameEvent;
        private GameObject _listenerObject;
        private EventListener _eventListener;
        private bool _eventRaised;

        [SetUp]
        public void Setup()
        {
            // Create test event
            _gameEvent = ScriptableObject.CreateInstance<GameEventSO>();
            
            // Create listener GameObject and component
            _listenerObject = new GameObject("TestListener");
            _eventListener = _listenerObject.AddComponent<EventListener>();
            
            // Set up test event
            _eventListener.EventSO = _gameEvent;
            
            // Setup test response
            var response = new UnityEvent();
            response.AddListener(() => _eventRaised = true);
            
            // Use reflection to set the private field since it's not exposed
            var field = typeof(EventListener).GetField("_onEventRaisedResponse", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field.SetValue(_eventListener, response);
            
            _eventRaised = false;
        }

        [TearDown]
        public void Teardown()
        {
            // Cleanup
            if (_listenerObject != null)
                Object.DestroyImmediate(_listenerObject);
                
            if (_gameEvent != null)
                Object.DestroyImmediate(_gameEvent);
        }

        [Test]
        public void OnEnable_RegistersToEvent()
        {
            // Act - Enable the component to trigger OnEnable
            _eventListener.enabled = true;
            _gameEvent.Raise();

            // Assert
            Assert.IsTrue(_eventRaised, "Event was not raised on the listener after registration.");
        }

        [Test]
        public void OnDisable_UnregistersFromEvent()
        {
            // Arrange - Enable then disable the component
            _eventListener.enabled = true;
            _eventListener.enabled = false;
            _eventRaised = false;

            // Act
            _gameEvent.Raise();

            // Assert
            Assert.IsFalse(_eventRaised, "Event was still raised after listener was unregistered.");
        }

        [Test]
        public void Respond_InvokesUnityEvent()
        {
            // Arrange
            bool wasCalled = false;
            var response = new UnityEvent();
            response.AddListener(() => wasCalled = true);
            
            // Use reflection to set the private field
            var field = typeof(EventListener).GetField("_onEventRaisedResponse", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field.SetValue(_eventListener, response);

            // Act - Call the private Respond method
            var method = typeof(EventListener).GetMethod("Respond", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method.Invoke(_eventListener, null);

            // Assert
            Assert.IsTrue(wasCalled, "UnityEvent was not invoked when Respond was called.");
        }
    }
}
