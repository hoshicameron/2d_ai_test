using NUnit.Framework;
using PetalsOfHope.Core.StateMachine;
using UnityEngine;
using UnityEngine.TestTools;

namespace PetalsOfHope.Tests.StateMachine
{
    [TestFixture]
    public class StateMachineTests
    {
        private GameObject _testGameObject;
        private Core.StateMachine.StateMachine _stateMachine;
        private MockState _mockState1;
        private MockState _mockState2;

        [SetUp]
        public void SetUp()
        {
            // Create a test GameObject and add the StateMachine component
            _testGameObject = new GameObject("TestStateMachine");
            _stateMachine = _testGameObject.AddComponent<Core.StateMachine.StateMachine>();
            
            // Create mock states
            _mockState1 = new MockState(_stateMachine);
            _mockState2 = new MockState(_stateMachine);
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up
            Object.DestroyImmediate(_testGameObject);
        }

        [Test]
        public void Initialize_SetsCurrentStateAndCallsEnter()
        {
            // Act
            _stateMachine.Initialize(_mockState1);

            // Assert
            Assert.AreEqual(_mockState1, _stateMachine.CurrentState);
            Assert.AreEqual(1, _mockState1.EnterCount);
            Assert.AreEqual(0, _mockState1.ExitCount);
        }

        [Test]
        public void Initialize_WithNullState_ThrowsError()
        {
            // Arrange
            LogAssert.Expect(LogType.Error, $"StateMachine: Cannot initialize with a null starting state.");

            // Act
            _stateMachine.Initialize(null);

            // Assert
            Assert.IsNull(_stateMachine.CurrentState);
        }

        [Test]
        public void ChangeState_CallsExitOnOldAndEnterOnNew()
        {
            // Arrange
            _stateMachine.Initialize(_mockState1);
            _mockState1.ResetCounters();

            // Act
            _stateMachine.ChangeState(_mockState2);


            // Assert
            Assert.AreEqual(1, _mockState1.ExitCount);
            Assert.AreEqual(1, _mockState2.EnterCount);
            Assert.AreEqual(_mockState2, _stateMachine.CurrentState);
        }

        [Test]
        public void ChangeState_ToNull_LogsError()
        {
            // Arrange
            _stateMachine.Initialize(_mockState1);
            LogAssert.Expect(LogType.Error, $"StateMachine: Cannot change to a null state.");

            // Act
            _stateMachine.ChangeState(null);

            // Assert
            Assert.AreEqual(_mockState1, _stateMachine.CurrentState);
        }


        [Test]
        public void ChangeState_WhenNotInitialized_InitializesWithNewState()
        {
            // Act
            _stateMachine.ChangeState(_mockState1);

            // Assert
            Assert.AreEqual(_mockState1, _stateMachine.CurrentState);
            Assert.AreEqual(1, _mockState1.EnterCount);
        }

        [Test]
        public void ChangeState_ToSameState_DoesNotReEnter()
        {
            // Arrange
            _stateMachine.Initialize(_mockState1);
            _mockState1.ResetCounters();

            // Act
            _stateMachine.ChangeState(_mockState1);


            // Assert
            Assert.AreEqual(0, _mockState1.ExitCount);
            Assert.AreEqual(0, _mockState1.EnterCount);
        }


        [Test]
        public void Update_CallsCurrentStateUpdate()
        {
            // Arrange
            _stateMachine.Initialize(_mockState1);
            _mockState1.ResetCounters();

            // Act - Simulate Unity's Update
            _stateMachine.SendMessage("Update");

            // Assert
            Assert.AreEqual(1, _mockState1.UpdateCount);
        }


        [Test]
        public void FixedUpdate_CallsCurrentStateFixedUpdate()
        {
            // Arrange
            _stateMachine.Initialize(_mockState1);
            _mockState1.ResetCounters();

            // Act - Simulate Unity's FixedUpdate
            _stateMachine.SendMessage("FixedUpdate");


            // Assert
            Assert.AreEqual(1, _mockState1.FixedUpdateCount);
        }


        [Test]
        public void OnDestroy_CallsExitOnCurrentState()
        {
            // Arrange
            _stateMachine.Initialize(_mockState1);
            _mockState1.ResetCounters();

            // Act - Simulate Unity's OnDestroy
            _stateMachine.SendMessage("OnDestroy");

            // Assert
            Assert.AreEqual(1, _mockState1.ExitCount);
        }


        [Test]
        public void StateMachine_NotInitialized_UpdateAndFixedUpdateDoNothing()
        {
            // Act - Simulate Unity's Update and FixedUpdate without initialization
            _stateMachine.SendMessage("Update");
            _stateMachine.SendMessage("FixedUpdate");

            // Assert - No errors should occur and no methods should be called on null state
            // (This is implicitly tested by the fact that the test completes without errors)
            Assert.IsTrue(true);
        }

        // Mock state for testing
        public class MockState : BaseState
        {
            public int EnterCount { get; private set; }
            public int ExitCount { get; private set; }
            public int UpdateCount { get; private set; }
            public int FixedUpdateCount { get; private set; }

            public MockState(Core.StateMachine.StateMachine stateMachine) : base(stateMachine) { }

            public override void Enter() => EnterCount++;
            public override void Exit() => ExitCount++;
            public override void Update() => UpdateCount++;
            public override void FixedUpdate() => FixedUpdateCount++;

            public void ResetCounters()
            {
                EnterCount = 0;
                ExitCount = 0;
                UpdateCount = 0;
                FixedUpdateCount = 0;
            }
        }
    }
}
