using UnityEngine;

namespace PetalsOfHope.Gameplay.StateMachine
{
    /// <summary>
    /// A MonoBehaviour component that manages states and handles transitions between them.
    /// Attach this to any GameObject that needs state management.
    /// </summary>
    [AddComponentMenu("Petals of Hope/Core/State Machine")]
    public class StateMachine : MonoBehaviour
    {
        private BaseState _currentState;
        private bool _isInitialized;

        /// <summary>
        /// Gets the currently active state.
        /// </summary>
        public BaseState CurrentState => _currentState;

        /// <summary>
        /// Initializes the state machine with a starting state.
        /// </summary>
        /// <param name="startingState">The initial state for the state machine.</param>
        public void Initialize(BaseState startingState)
        {
            if (startingState == null)
            {
                Debug.LogError($"{nameof(StateMachine)}: Cannot initialize with a null starting state.", this);
                return;
            }

            _currentState = startingState;
            _currentState.Enter();
            _isInitialized = true;
            
            Debug.Log($"{nameof(StateMachine)}: Initialized with state {_currentState.GetType().Name}", this);
        }

        /// <summary>
        /// Changes the current state to the specified state.
        /// </summary>
        /// <param name="newState">The state to transition to.</param>
        public void ChangeState(BaseState newState)
        {
            if (newState == null)
            {
                Debug.LogError($"{nameof(StateMachine)}: Cannot change to a null state.", this);
                return;
            }

            if (!_isInitialized)
            {
                Debug.LogWarning($"{nameof(StateMachine)}: Not initialized. Initializing with the new state.", this);
                Initialize(newState);
                return;
            }

            if (_currentState == newState)
            {
                Debug.LogWarning($"{nameof(StateMachine)}: Already in the requested state {newState.GetType().Name}.", this);
                return;
            }

            Debug.Log($"{nameof(StateMachine)}: Changing state from {_currentState?.GetType().Name ?? "None"} to {newState.GetType().Name}", this);
            
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        public void UpdateState()
        {
            if (!_isInitialized || _currentState == null) return;
            _currentState.Update();
        }

        public void FixedUpdateState()
        {
            if (!_isInitialized || _currentState == null) return;
            _currentState.FixedUpdate();
        }

        private void OnDestroy()
        {
            if (_isInitialized && _currentState != null)
            {
                _currentState.Exit();
                _currentState = null;
                _isInitialized = false;
            }
        }

        /// <summary>
        /// Helper method to get the state machine from a GameObject, adding it if needed.
        /// </summary>
        public static StateMachine GetOrAddStateMachine(GameObject gameObject)
        {
            var stateMachine = gameObject.GetComponent<StateMachine>();
            return stateMachine != null ? stateMachine : gameObject.AddComponent<StateMachine>();
        }
    }
}
