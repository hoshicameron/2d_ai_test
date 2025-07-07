# Task ID: 3.2.4
# Parent Task ID: 3.2
# Title: Implement Concrete AI States (ChaseState)
# Status: pending
# Dependencies: 3.2.1, 3.2.2, 1.2.8 # AI.Core.State, AI.StateMachine, EventSO assets
# Priority: critical
# Estimated Effort: L
# Assignee: Unassigned

# Description:
Implement `ChaseState.cs` as a concrete `AI.Core.State` ScriptableObject. This state will make the enemy move towards the detected player. It transitions to `AttackState` if the player is in attack range, or back to `PatrolState` if the player is lost.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/AI/States/ChaseState.cs`
2.  **Namespace:** `PetalsOfHope.AI.States`
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/AI/States/ChaseState.cs
    namespace PetalsOfHope.AI.States
    {
        using UnityEngine;
        using PetalsOfHope.AI.Core; // For State and AI.StateMachine

        [CreateAssetMenu(menuName = "Petals of Hope/AI/States/Chase State", fileName = "NewChaseStateSO")]
        public class ChaseState : State
        {
            [Header("Chase Parameters")]
            [Tooltip("Time after which the player is considered 'lost' if not seen/within range.")]
            public float losePlayerTime = 3.0f;
            // Attack range will come from EnemyStatsSO (e.g., a field named 'attackRange')
            // Detection range might still be relevant for re-confirming player presence.

            [Header("Transitions")]
            [Tooltip("State to transition to when player is in attack range.")]
            public State attackState; // Assign AttackState SO asset here
            [Tooltip("State to transition to when player is lost.")]
            public State patrolState; // Assign PatrolState SO asset here

            // Runtime data
            private float _losePlayerTimer;
            private PetalsOfHope.AI.Core.StateMachine _owner;

            public override void EnterState(PetalsOfHope.AI.Core.StateMachine ownerStateMachine)
            {
                _owner = ownerStateMachine;
                // Debug.Log($"{_owner.Enemy.gameObject.name} entering Chase State.");
                _owner.AnimationController.Play("Run"); // Or use EnemyStatsSO chaseAnimHash/parameter

                _losePlayerTimer = losePlayerTime;
            }

            public override void ExecuteState(PetalsOfHope.AI.Core.StateMachine ownerStateMachine)
            {
                if (ownerStateMachine.PlayerTransform == null)
                {
                    // No player, can't chase. Transition to Patrol.
                    if (patrolState != null) ownerStateMachine.ChangeState(patrolState);
                    else Debug.LogWarning("PatrolState not set in ChaseState SO, cannot transition when player is null.", this);
                    return;
                }

                // 1. Check if Player is in Attack Range
                float distanceToPlayer = Vector2.Distance(ownerStateMachine.Enemy.transform.position, ownerStateMachine.PlayerTransform.position);
                float attackRange = ownerStateMachine.Enemy.Stats.attackRange; // Assuming attackRange in EnemyStatsSO

                if (distanceToPlayer <= attackRange)
                {
                    // Optionally, check Line of Sight for attack
                    if (IsPlayerInLineOfSight(ownerStateMachine) && attackState != null)
                    {
                         ownerStateMachine.ChangeState(attackState);
                         return;
                    }
                }

                // 2. Check if Player is Lost (either out of detection range or timer runs out)
                if (!IsPlayerStillDetected(ownerStateMachine))
                {
                    _losePlayerTimer -= Time.deltaTime;
                    if (_losePlayerTimer <= 0f)
                    {
                        if (patrolState != null) ownerStateMachine.ChangeState(patrolState);
                        else Debug.LogWarning("PatrolState not set in ChaseState SO, cannot transition when player lost.", this);
                        return;
                    }
                }
                else
                {
                    _losePlayerTimer = losePlayerTime; // Reset timer if player is detected
                }

                // 3. Move Towards Player
                Vector2 direction = (ownerStateMachine.PlayerTransform.position - ownerStateMachine.Enemy.transform.position).normalized;
                ownerStateMachine.Enemy.Rigidbody.velocity = direction * ownerStateMachine.Enemy.Stats.chaseSpeed;
                
                // Play run/chase animation (might already be playing from EnterState if it's a loop)
                // ownerStateMachine.AnimationController.Play("Run"); // Ensure it's playing

                // Flip sprite
                FlipSprite(direction.x, ownerStateMachine.Enemy.transform);
            }
            
            private bool IsPlayerStillDetected(PetalsOfHope.AI.Core.StateMachine ownerStateMachine)
            {
                if (ownerStateMachine.PlayerTransform == null) return false;
                // Using detectionRange from stats. PatrolState has its own playerLayerMask field.
                // Consider having playerLayerMask on EnemyStatsSO or AI.StateMachine for consistency.
                // For now, let's assume ChaseState might use a slightly different check or re-uses PatrolState's mask if available.
                // A simple distance check is often enough once already in Chase state.
                float distance = Vector2.Distance(ownerStateMachine.Enemy.transform.position, ownerStateMachine.PlayerTransform.position);
                return distance <= ownerStateMachine.Enemy.Stats.detectionRange; // Or a larger "chase persistence range"
            }

            private bool IsPlayerInLineOfSight(PetalsOfHope.AI.Core.StateMachine ownerStateMachine)
            {
                if (ownerStateMachine.PlayerTransform == null) return false;
                
                // Define a layer mask that includes obstacles but not the enemy itself.
                // This mask would typically be configured on the enemy or AI.StateMachine.
                // For simplicity, assume a default "Obstacle" layer exists.
                LayerMask sightBlockingLayers = LayerMask.GetMask("Obstacles", "Ground"); // Example layers

                RaycastHit2D hit = Physics2D.Linecast(
                    ownerStateMachine.Enemy.transform.position, 
                    ownerStateMachine.PlayerTransform.position, 
                    sightBlockingLayers
                );

                // If the raycast hits nothing, or it hits the player, then LOS is clear.
                // (This assumes Player is not on an obstacle layer that blocks its own detection)
                // A more robust check would be: if (hit.collider != null && hit.transform == ownerStateMachine.PlayerTransform)
                if (hit.collider == null || (hit.transform == ownerStateMachine.PlayerTransform)) 
                {
                    return true;
                }
                // Debug.Log($"LOS to player blocked by {hit.collider?.name}");
                return false;
            }


            private void FlipSprite(float directionX, Transform enemyTransform)
            {
                if (directionX > 0.01f && enemyTransform.localScale.x < 0f)
                {
                    enemyTransform.localScale = new Vector3(Mathf.Abs(enemyTransform.localScale.x), enemyTransform.localScale.y, enemyTransform.localScale.z);
                }
                else if (directionX < -0.01f && enemyTransform.localScale.x > 0f)
                {
                    enemyTransform.localScale = new Vector3(-Mathf.Abs(enemyTransform.localScale.x), enemyTransform.localScale.y, enemyTransform.localScale.z);
                }
            }

            public override void ExitState(PetalsOfHope.AI.Core.StateMachine ownerStateMachine)
            {
                // Debug.Log($"{ownerStateMachine.Enemy.gameObject.name} exiting Chase State.");
                // Optionally stop movement if transitioning to a state that doesn't immediately override velocity (like Patrol sometimes)
                // ownerStateMachine.Enemy.Rigidbody.velocity = Vector2.zero; 
            }
        }
    }
    ```

# Acceptance Criteria:
- `ChaseState.cs` ScriptableObject is created, inherits from `AI.Core.State`.
- `EnterState()`:
    - Plays chase animation.
    - Initializes `_losePlayerTimer`.
- `ExecuteState()`:
    - If player in `attackRange` (from `EnemyStatsSO`) and (optionally) line of sight, transitions to `attackState`.
    - If player is lost (e.g., `_losePlayerTimer` expires or player out of a wider detection/persistence range), transitions to `patrolState`.
    - Moves enemy towards player using `EnemyStatsSO.chaseSpeed`.
    - Flips enemy sprite to face player.
- `ExitState()` is present.
- Script compiles without errors. `EnemyStatsSO` is assumed to have `attackRange` (e.g., float).

# Test Strategy:
- Create a `ChaseState` SO asset. Configure `losePlayerTime`, assign (yet to be created) `AttackState` SO and existing `PatrolState` SO.
- In an enemy prefab's AI `StateMachine`, ensure `PatrolState` can transition to this `ChaseState` SO.
- Test in a scene:
    - Let enemy detect Player and enter `ChaseState`.
    - Verify enemy moves towards Player at `chaseSpeed` and plays chase animation.
    - Verify sprite flipping.
    - Move Player into `attackRange`: verify transition to `AttackState` (or log if not fully implemented).
    - Move Player out of detection/LOS for `losePlayerTime`: verify enemy transitions back to `PatrolState`.
    - Test LOS for attack range check.

# Notes/Questions:
- `EnemyStatsSO` needs an `attackRange` field. (e.g., `public float attackRange = 1.5f;`).
- Line of sight (LOS) check (`IsPlayerInLineOfSight`) is added as a common refinement for chase/attack conditions. The layers it checks against (`sightBlockingLayers`) would ideally be configurable.
- The logic for "losing" the player can be refined (e.g., last known position, wider persistence range than initial detection range).
- Animation names ("Run") are placeholders.