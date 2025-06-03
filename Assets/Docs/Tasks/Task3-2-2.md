# Task ID: 3.2.2
# Parent Task ID: 3.2
# Title: Define AI StateMachine MonoBehaviour
# Status: pending
# Dependencies: 3.1.2, 2.2, 3.2.1 # EnemyBase, AnimationController, AI.Core.State
# Priority: critical
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement `StateMachine.cs` within the `PetalsOfHope.AI` namespace. This MonoBehaviour will be responsible for managing an enemy's AI states. It will hold references to `EnemyBase`, `AnimationController`, the initial AI state, and the current AI state.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/AI/Core/StateMachine.cs` (Note: Plan shows `AI/Core/StateMachine.cs`, not just `AI/StateMachine.cs`. Let's stick to `AI/Core/` to group core AI logic)
2.  **Namespace:** `PetalsOfHope.AI` (or `PetalsOfHope.AI.Core` to match folder path) - Let's use `PetalsOfHope.AI.Core` for consistency with `State.cs`.
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/AI/Core/StateMachine.cs
    namespace PetalsOfHope.AI.Core // Matching folder structure
    {
        using UnityEngine;
        using PetalsOfHope.Enemies.Core; // For EnemyBase
        using CoreAnimation = PetalsOfHope.Core.Animation.AnimationController; // Alias

        [RequireComponent(typeof(EnemyBase))]
        // AnimationController is already required by EnemyBase, but explicit here shows AI's dependency.
        [RequireComponent(typeof(CoreAnimation))]
        public class StateMachine : MonoBehaviour // This is the AI.StateMachine
        {
            [Header("AI Configuration")]
            [Tooltip("The initial state for this AI.")]
            [SerializeField] private State _initialState; // AI.Core.State ScriptableObject

            // Public properties to be accessed by AI States
            public EnemyBase Enemy { get; private set; }
            public CoreAnimation AnimationController { get; private set; }
            // Add other common components states might need, e.g., Rigidbody2D, Player reference
            public Transform PlayerTransform { get; private set; } // Example: if player is often targeted

            private State _currentState; // AI.Core.State
            public State CurrentState => _currentState;

            private bool _isInitialized = false;

            private void Awake()
            {
                Enemy = GetComponent<EnemyBase>();
                AnimationController = GetComponent<CoreAnimation>();

                // Find player target (simple approach, can be made more robust)
                GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
                if (playerGameObject != null)
                {
                    PlayerTransform = playerGameObject.transform;
                }
                else
                {
                    Debug.LogWarning("AI StateMachine: Player GameObject not found by tag 'Player'. Some states might not function.", this);
                }
            }

            private void Start()
            {
                if (_initialState == null)
                {
                    Debug.LogError("AI StateMachine: Initial state is not set!", this);
                    enabled = false; // Disable AI if no initial state
                    return;
                }
                Initialize(_initialState);
            }
            
            private void Initialize(State startingState)
            {
                _currentState = startingState;
                _currentState.EnterState(this);
                _isInitialized = true;
            }

            private void Update()
            {
                if (!_isInitialized || _currentState == null) return;
                _currentState.ExecuteState(this);

                // Optional: Handle transitions centrally if states return a new state from ExecuteState or a CheckTransitions method.
                // State nextState = _currentState.CheckTransitions(this);
                // if (nextState != null)
                // {
                //    ChangeState(nextState);
                // }
            }

            public void ChangeState(State newState)
            {
                if (newState == null)
                {
                    Debug.LogError("AI StateMachine: Cannot change to a null state.", this);
                    return;
                }

                if (!_isInitialized)
                {
                     Debug.LogWarning("AI StateMachine not initialized. Call Initialize(startingState) first. Initializing with this new state.", this);
                    Initialize(newState);
                    return;
                }

                if (_currentState == newState && _currentState != null) // Check _currentState != null for safety
                {
                    // Depending on design, you might want to re-enter the same state or do nothing.
                    // For SO states, `_currentState == newState` compares asset references. If states are instantiated, this logic might differ.
                    // Debug.LogWarning($"AI StateMachine attempting to change to the same state asset: {newState.name}. Re-entering.", this);
                    // _currentState.ExitState(this); // Exit current
                    // _currentState.EnterState(this); // Re-enter current
                    return; // Or simply do nothing if it's the same SO asset.
                }
                
                _currentState?.ExitState(this);
                // Debug.Log($"AI {Enemy.gameObject.name} Exited state: {_currentState?.name ?? "None"}. Entering state: {newState.name}", this);
                _currentState = newState;
                _currentState.EnterState(this);
            }

            private void OnDestroy()
            {
                 if (_isInitialized && _currentState != null)
                {
                    _currentState.ExitState(this);
                    _currentState = null;
                }
            }
        }
    }
    ```

# Acceptance Criteria:
- `StateMachine.cs` (AI version) MonoBehaviour is created in `Assets/_Project/Scripts/AI/Core/`.
- It requires `EnemyBase` and `CoreAnimation` components.
- It holds references to `EnemyBase Enemy` and `CoreAnimation AnimationController` (public properties).
- It has a serialized field `_initialState` of type `AI.Core.State` (the SO base class).
- `Start()` initializes the machine with `_initialState` by calling `_initialState.EnterState(this)`.
- `Update()` calls `_currentState.ExecuteState(this)`.
- `ChangeState(State newState)` method correctly calls `ExitState()` on the old state and `EnterState()` on the new state.
- Handles null initial state and null new state with errors.
- `PlayerTransform` is found (or warning logged).
- `OnDestroy` ensures current AI state is exited.
- Script compiles without errors.

# Test Strategy:
- Unit/Integration Testing:
    - Create mock/stub `AI.Core.State` SO assets.
    - Attach the AI `StateMachine` to an `EnemyBase` mock/test object.
    - Test initialization: verify `_initialState.EnterState()` is called.
    - Test `ChangeState()`: verify `ExitState()` and `EnterState()` calls on respective mock states.
    - Test `Update()` calls `ExecuteState()` on the current mock state.
- This will be further tested with concrete AI states (Patrol, Chase, Attack).

# Notes/Questions:
- This AI `StateMachine` is distinct from the `Core.StateMachine.StateMachine` used for the player. It's tailored for AI, particularly if using `ScriptableObject` states.
- The `PlayerTransform` is a common dependency for AI; finding it by tag is a simple approach. More robust solutions might use an event system or a service locator.
- Logic for handling transitions (e.g., `_currentState.CheckTransitions(this)`) can be added to `Update()` if states are designed to return potential next states. Alternatively, states themselves call `ownerStateMachine.ChangeState()` when their conditions are met. The latter is more common for simpler state machines and is assumed for now.
- Re-entering the same `ScriptableObject` state asset might mean calling `ExitState` then `EnterState` on it, or simply doing nothing. The current implementation does nothing if the `_currentState` SO asset is the same as `newState` SO asset. This might need adjustment if states have internal, non-SO state that needs resetting on re-entry.