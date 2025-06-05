using UnityEngine;
using PetalsOfHope.Core.StateMachine;

namespace PetalsOfHope.Tests
{
    /// <summary>
    /// Example script demonstrating how to use the StateMachine system.
    /// </summary>
    public class StateMachineExample : MonoBehaviour
    {
        [SerializeField] private KeyCode _nextStateKey = KeyCode.Space;
        
        private StateMachine _stateMachine;
        private IdleState _idleState;
        private WalkingState _walkingState;
        private JumpingState _jumpingState;

        private void Start()
        {
            // Get or add the StateMachine component
            _stateMachine = StateMachine.GetOrAddStateMachine(gameObject);
            
            // Create state instances
            _idleState = new IdleState(_stateMachine);
            _walkingState = new WalkingState(_stateMachine);
            _jumpingState = new JumpingState(_stateMachine);
            
            // Initialize the state machine with the idle state
            _stateMachine.Initialize(_idleState);
            
            Debug.Log("Press Space to cycle through states");
        }

        private void Update()
        {
            if (Input.GetKeyDown(_nextStateKey))
            {
                CycleToNextState();
            }
        }

        private void CycleToNextState()
        {
            if (_stateMachine.CurrentState == _idleState)
            {
                _stateMachine.ChangeState(_walkingState);
            }
            else if (_stateMachine.CurrentState == _walkingState)
            {
                _stateMachine.ChangeState(_jumpingState);
            }
            else
            {
                _stateMachine.ChangeState(_idleState);
            }
        }
    }

    #region Example State Implementations
    
    public class IdleState : BaseState
    {
        public IdleState(StateMachine stateMachine) : base(stateMachine) { }

        public override void Enter() => Debug.Log("Entering Idle State");
        public override void Exit() => Debug.Log("Exiting Idle State");
        public override void Update() { /* Idle logic */ }
        public override void FixedUpdate() { /* Physics-related idle logic */ }
    }

    public class WalkingState : BaseState
    {
        public WalkingState(StateMachine stateMachine) : base(stateMachine) { }

        public override void Enter() => Debug.Log("Entering Walking State");
        public override void Exit() => Debug.Log("Exiting Walking State");
        public override void Update() { /* Walking logic */ }
        public override void FixedUpdate() { /* Physics-related walking logic */ }
    }

    public class JumpingState : BaseState
    {
        public JumpingState(StateMachine stateMachine) : base(stateMachine) { }

        public override void Enter() => Debug.Log("Entering Jumping State");
        public override void Exit() => Debug.Log("Exiting Jumping State");
        public override void Update() { /* Jumping logic */ }
        public override void FixedUpdate() { /* Physics-related jumping logic */ }
    }
    
    #endregion
}
