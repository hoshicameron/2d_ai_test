# Task ID: 3.2.3
# Parent Task ID: 3.2
# Title: Implement Concrete AI States (PatrolState)
# Status: completed
# Dependencies: 3.2.1, 3.2.2, 1.2.8 # AI.Core.State, AI.StateMachine, EventSO assets
# Priority: critical
# Estimated Effort: L
# Assignee: Unassigned

# Description:
Implement `PatrolState.cs` as a concrete `AI.Core.State` ScriptableObject. This state will make the enemy move between predefined waypoints and detect the player, transitioning to `ChaseState` if the player is found.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/AI/States/PatrolState.cs`
2.  **Namespace:** `PetalsOfHope.AI.States`
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/AI/States/PatrolState.cs
    namespace PetalsOfHope.AI.States
    {
        using UnityEngine;
        using PetalsOfHope.AI.Core; // For State and AI.StateMachine
        // using PetalsOfHope.Core.Events; // For EnemyDetectedPlayerEventSO

        [CreateAssetMenu(menuName = "Petals of Hope/AI/States/Patrol State", fileName = "NewPatrolStateSO")]
        public class PatrolState : State
        {
            [Header("Patrol Parameters")]
            [Tooltip("Time to wait at each waypoint.")]
            public float waypointWaitTime = 1.0f;
            // Waypoints could be defined here, or on the Enemy, or dynamically found.
            // For simplicity, let's assume EnemyBase or a dedicated PatrolPath component provides waypoints.

            [Header("Detection")]
            [Tooltip("Layer mask for detecting the player.")]
            public LayerMask playerLayerMask; // Should be set on the SO asset
            // Detection range will come from EnemyStatsSO via ownerStateMachine.Enemy.Stats.detectionRange

            [Header("Transitions")]
            [Tooltip("State to transition to when player is detected.")]
            public State chaseState; // Assign ChaseState SO asset here

            // Runtime data (not serialized for SO, re-initialized in EnterState)
            private Transform[] _waypoints;
            private int _currentWaypointIndex;
            private float _waitTimer;
            private bool _isWaiting;
            
            // Reference to the specific AI StateMachine
            private PetalsOfHope.AI.Core.StateMachine _owner;


            public override void EnterState(PetalsOfHope.AI.Core.StateMachine ownerStateMachine)
            {
                _owner = ownerStateMachine;
                // Debug.Log($"{_owner.Enemy.gameObject.name} entering Patrol State.");
                _owner.AnimationController.Play("Walk"); // Or use EnemyStatsSO patrolAnimHash

                // Get waypoints from a component on the enemy, e.g., PatrolPath.cs
                PatrolPath patrolPath = _owner.Enemy.GetComponent<PatrolPath>();
                if (patrolPath != null && patrolPath.waypoints.Length > 0)
                {
                    _waypoints = patrolPath.waypoints;
                }
                else
                {
                    // Fallback: use enemy's starting position as a single "waypoint" (stand still)
                    _waypoints = new Transform[] { _owner.Enemy.transform };
                    Debug.LogWarning($"{_owner.Enemy.gameObject.name} PatrolState: No PatrolPath component found or no waypoints. Patrolling in place.", _owner.Enemy);
                }
                
                _currentWaypointIndex = FindClosestWaypointIndex();
                _waitTimer = 0f;
                _isWaiting = false;
                MoveToWaypoint();
            }
            
            private int FindClosestWaypointIndex()
            {
                if (_waypoints == null || _waypoints.Length == 0) return 0;
                
                int closestIndex = 0;
                float minDistance = float.MaxValue;
                for (int i = 0; i < _waypoints.Length; i++)
                {
                    if (_waypoints[i] == null) continue;
                    float distance = Vector2.Distance(_owner.Enemy.transform.position, _waypoints[i].position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestIndex = i;
                    }
                }
                return closestIndex;
            }


            public override void ExecuteState(PetalsOfHope.AI.Core.StateMachine ownerStateMachine)
            {
                // 1. Check for Player Detection
                if (DetectPlayer())
                {
                    // Raise EnemyDetectedPlayerEventSO? (Plan item)
                    // Example: ownerStateMachine.EnemyDetectedPlayerEvent?.Raise(ownerStateMachine.PlayerTransform.gameObject);
                    if (chaseState != null)
                    {
                        ownerStateMachine.ChangeState(chaseState);
                    }
                    else
                    {
                        Debug.LogWarning("ChaseState not set in PatrolState SO, cannot transition.", this);
                    }
                    return;
                }

                // 2. Handle Patrolling Logic
                if (_isWaiting)
                {
                    _waitTimer -= Time.deltaTime;
                    if (_waitTimer <= 0f)
                    {
                        _isWaiting = false;
                        _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
                        MoveToWaypoint();
                    }
                }
                else // Moving towards waypoint
                {
                    if (_waypoints[_currentWaypointIndex] == null) { // Waypoint might have been destroyed
                         _isWaiting = true; // Force re-evaluation or skip
                         _waitTimer = 0.1f; // Small delay before trying next
                         return;
                    }

                    float distanceToWaypoint = Vector2.Distance(_owner.Enemy.transform.position, _waypoints[_currentWaypointIndex].position);
                    if (distanceToWaypoint < 0.5f) // Threshold to consider waypoint reached
                    {
                        _isWaiting = true;
                        _waitTimer = waypointWaitTime;
                        _owner.AnimationController.Play("Idle"); // Or use EnemyStatsSO idleAnimHash
                        _owner.Enemy.Rigidbody.velocity = Vector2.zero; // Stop at waypoint
                    }
                    else
                    {
                        MoveToWaypoint(); // Continue moving
                    }
                }
            }
            
            private void MoveToWaypoint()
            {
                if (_waypoints == null || _waypoints.Length == 0 || _waypoints[_currentWaypointIndex] == null) return;

                Vector2 direction = (_waypoints[_currentWaypointIndex].position - _owner.Enemy.transform.position).normalized;
                _owner.Enemy.Rigidbody.velocity = direction * _owner.Enemy.Stats.patrolSpeed;
                _owner.AnimationController.Play("Walk"); // Or parameter based on speed

                // Flip sprite to face direction
                if (direction.x > 0.01f && _owner.Enemy.transform.localScale.x < 0f)
                {
                    _owner.Enemy.transform.localScale = new Vector3(Mathf.Abs(_owner.Enemy.transform.localScale.x), _owner.Enemy.transform.localScale.y, _owner.Enemy.transform.localScale.z);
                }
                else if (direction.x < -0.01f && _owner.Enemy.transform.localScale.x > 0f)
                {
                    _owner.Enemy.transform.localScale = new Vector3(-Mathf.Abs(_owner.Enemy.transform.localScale.x), _owner.Enemy.transform.localScale.y, _owner.Enemy.transform.localScale.z);
                }
            }

            private bool DetectPlayer()
            {
                if (_owner.PlayerTransform == null) return false;

                // Simple distance check
                // float distanceToPlayer = Vector2.Distance(_owner.Enemy.transform.position, _owner.PlayerTransform.position);
                // if (distanceToPlayer < _owner.Enemy.Stats.detectionRange) { ... }

                // Physics2D.OverlapCircle for detection
                Collider2D playerCollider = Physics2D.OverlapCircle(_owner.Enemy.transform.position, _owner.Enemy.Stats.detectionRange, playerLayerMask);
                if (playerCollider != null)
                {
                    // Could add Line of Sight check here if needed:
                    // RaycastHit2D hit = Physics2D.Linecast(_owner.Enemy.transform.position, _owner.PlayerTransform.position, ~_owner.Enemy.gameObject.layer); // Ignore self
                    // if (hit.collider == playerCollider) return true;
                    return true;
                }
                return false;
            }

            public override void ExitState(PetalsOfHope.AI.Core.StateMachine ownerStateMachine)
            {
                // Debug.Log($"{ownerStateMachine.Enemy.gameObject.name} exiting Patrol State.");
                // Stop movement when exiting patrol, unless transition is to Chase which will set its own velocity
                ownerStateMachine.Enemy.Rigidbody.velocity = Vector2.zero;
            }

            // Helper component for defining waypoints (place on Enemy GameObject)
            // Could be a separate file: PatrolPath.cs
            // [System.Serializable]
            // public class PatrolPath : MonoBehaviour { public Transform[] waypoints; }
        }

        // Create PatrolPath.cs separately
        // File: Assets/_Project/Scripts/AI/Components/PatrolPath.cs
        // namespace PetalsOfHope.AI.Components {
        //     using UnityEngine;
        //     public class PatrolPath : MonoBehaviour {
        //         public Transform[] waypoints;
        //         void OnDrawGizmosSelected() { /* Draw lines between waypoints */ }
        //     }
        // }
    }
    ```
    **Note:** `PatrolPath.cs` component needs to be created separately (see comment in code).

# Acceptance Criteria:
- `PatrolState.cs` ScriptableObject is created, inherits from `AI.Core.State`.
- `EnterState()` initializes patrol (e.g., finds waypoints, starts moving to the first one, plays walk animation). Waypoints are retrieved from a `PatrolPath` component on the enemy.
- `ExecuteState()`:
    - Calls `DetectPlayer()`. If player detected and `chaseState` is set, transitions to `chaseState`.
    - If not detecting player, moves enemy between waypoints using `EnemyStatsSO.patrolSpeed`.
    - Waits for `waypointWaitTime` at each waypoint.
    - Flips enemy sprite to face movement direction.
- `DetectPlayer()` uses `Physics2D.OverlapCircle` with `EnemyStatsSO.detectionRange` and `playerLayerMask`.
- `ExitState()` stops enemy movement.
- Script compiles without errors. A `PatrolPath.cs` component is defined to hold waypoint data.

# Test Strategy:
- Create a `PatrolState` SO asset in the project. Configure `waypointWaitTime`, `playerLayerMask`, and assign a (yet to be created) `ChaseState` SO asset.
- Create an enemy prefab with `EnemyBase`, AI `StateMachine`, and `PatrolPath` component. Define waypoints for `PatrolPath`.
- Assign the `PatrolState` SO as the initial state for the AI `StateMachine`.
- In a test scene with the enemy and a Player (on the correct layer for `playerLayerMask`):
    - Verify enemy patrols between waypoints, waits, and plays walk/idle animations.
    - Verify sprite flipping.
    - Move Player into detection range: verify enemy transitions to `ChaseState` (or logs intent if ChaseState not fully implemented).
    - Debug `detectionRange` with Gizmos (e.g., in `EnemyBase` or AI `StateMachine` `OnDrawGizmosSelected`).

# Notes/Questions:
- `PatrolPath.cs` component is crucial for this state. It should be a simple MonoBehaviour attached to the enemy, holding an array of `Transform`s. Task 3.3.3 might create such a component. For now, its existence is assumed. I will add a sub-task for it if not covered later.
- Animation names ("Walk", "Idle") are placeholders. These should ideally be configurable (e.g., in `EnemyStatsSO`) or use hashes.
- Player detection uses `OverlapCircle`. Line of sight (LOS) checks can be added for more complex detection.
- The plan mentions "States should raise events via Event Bus (e.g., `EnemyDetectedPlayerEventSO`)". This should be added to `DetectPlayer()` if true.