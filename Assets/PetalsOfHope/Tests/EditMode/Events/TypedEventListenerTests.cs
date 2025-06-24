// In Assets/_Project/Tests/EditMode/Events/TypedEventListenerTests.cs
namespace PetalsOfHope.Tests.EditMode.Events
{
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.TestTools;
    using PetalsOfHope.Core.Events;
    using PetalsOfHope.Core.Events.Listeners;

    public class TypedEventListenerTests
    {
        private IntEventSO _typedEvent;
        private GameObject _listenerObject;
        private IntEventListener _typedEventListener;
        private int _receivedPayload;
        private bool _eventRaised;

        [SetUp]
        public void Setup()
        {
            // Create test event
            _typedEvent = ScriptableObject.CreateInstance<IntEventSO>();
            
            // Create listener GameObject and component
            _listenerObject = new GameObject("TestTypedListener");
            _typedEventListener = _listenerObject.AddComponent<IntEventListener>();
            
            // Set up test event using reflection
            var eventField = typeof(IntEventListener).BaseType
                .GetField("_eventSO", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            eventField.SetValue(_typedEventListener, _typedEvent);
            
            // Setup test response
            var response = new UnityEvent<int>();
            response.AddListener(payload => {
                _receivedPayload = payload;
                _eventRaised = true;
            });
            
            // Use reflection to set the private field
            var responseField = typeof(IntEventListener).BaseType
                .GetField("_onEventRaisedResponse", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            responseField.SetValue(_typedEventListener, response);
            
            _eventRaised = false;
            _receivedPayload = -1;
        }

        [TearDown]
        public void Teardown()
        {
            // Cleanup
            if (_listenerObject != null)
                Object.DestroyImmediate(_listenerObject);
                
            if (_typedEvent != null)
                Object.DestroyImmediate(_typedEvent);
        }

        [Test]
        public void OnEnable_RegistersToTypedEvent()
        {
            // Arrange
            const int testPayload = 42;
            
            // Act - Enable the component to trigger OnEnable
            _typedEventListener.enabled = true;
            _typedEvent.Raise(testPayload);

            // Assert
            Assert.IsTrue(_eventRaised, "Event was not raised on the typed listener after registration.");
            Assert.AreEqual(testPayload, _receivedPayload, "Incorrect payload received by the listener.");
        }

        [Test]
        public void OnDisable_UnregistersFromTypedEvent()
        {
            // Arrange - Enable then disable the component
            _typedEventListener.enabled = true;
            _typedEventListener.enabled = false;
            _eventRaised = false;
            _receivedPayload = -1;

            // Act
            _typedEvent.Raise(42);

            // Assert
            Assert.IsFalse(_eventRaised, "Event was still raised after typed listener was unregistered.");
            Assert.AreEqual(-1, _receivedPayload, "Payload was still received after unregistering.");
        }

        [Test]
        public void Respond_InvokesTypedUnityEvent_WithPayload()
        {
            // Arrange
            int receivedValue = -1;
            var response = new UnityEvent<int>();
            response.AddListener(value => receivedValue = value);
            
            // Use reflection to set the private field
            var field = typeof(IntEventListener).BaseType
                .GetField("_onEventRaisedResponse", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field.SetValue(_typedEventListener, response);

            // Act - Call the private Respond method with reflection
            var method = typeof(IntEventListener).BaseType
                .GetMethod("Respond", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method.Invoke(_typedEventListener, new object[] { 42 });

            // Assert
            Assert.AreEqual(42, receivedValue, "UnityEvent was not invoked with the correct payload.");
        }
    }
}
