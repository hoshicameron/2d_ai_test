# Task ID: 3.2.5
# Parent Task ID: 3.2
# Title: Implement Concrete AI States (AttackState)
# Status: pending
# Dependencies: 3.2.1, 3.2.2, 1.2.8 # AI.Core.State, AI.StateMachine, EventSO assets
# Priority: critical
# Estimated Effort: L
# Assignee: Unassigned

# Description:
Implement `AttackState.cs` as a concrete `AI.Core.State` ScriptableObject. This state will make the enemy perform its attack logic, trigger attack animation, deal damage, manage attack cooldown, and then transition back to `ChaseState` (or another appropriate state).

# Details:
1.  **File Location:** `Assets/_Project/Scripts/AI/States/AttackState.cs`
2.  **Namespace:** `PetalsOfHope.AI.States`
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/AI/States/AttackState.cs
    namespace PetalsOfHope.AI.States
    {
        using UnityEngine;
        using PetalsOfHope.AI.Core; // For State and AI.StateMachine
        using PetalsOfHope.Interfaces; // For IDamageable

        [CreateAssetMenu(menuName = "Petals of Hope/AI/States/Attack State", fileName = "NewAttackStateSO")]
        public class AttackState : State
        {
            [Header("Attack Parameters")]
            [Tooltip("Duration of the attack animation/action before damage is applied or state ends.")]
            public float attackDuration = 1.0f; // Or get from animation clip
            [Tooltip("Time after an attack before another attack can be initiated.")]
            public float attackCooldown = 1.5f; // Or get from EnemyStatsSO

            [Header("Transitions")]
            [Tooltip("State to transition to after attack and cooldown.")]
            public State chaseState; // Assign ChaseState SO asset here

            // Runtime data
            private float _attackTimer;
            private float _cooldownTimer;
            private bool _hasAttacked; // To ensure damage is applied only once per attack action
            private PetalsOfHope.AI.Core.StateMachine _owner;


            public override void EnterState(PetalsOfHope.AI.Core.StateMachine ownerStateMachine)
            {
                _owner = ownerStateMachine;
                // Debug.Log($"{_owner.Enemy.gameObject.name} entering Attack State.");
                
                // Stop movement
                _owner.Enemy.Rigidbody.velocity = Vector2.zero;
                
                // Face the player
                if (ownerStateMachine.PlayerTransform != null)
                {
                    float directionToPlayerX = ownerStateMachine.PlayerTransform.position.x - ownerStateMachine.Enemy.transform.position.x;
                    FlipSprite(directionToPlayerX, ownerStateMachine.Enemy.transform);
                }

                _owner.AnimationController.Play("Attack"); // Or use EnemyStatsSO attackAnimHash/parameter
                                                        // This animation should ideally dictate the attackDuration via an animation event.

                _attackTimer = attackDuration;
                _cooldownTimer = attackCooldown; // Cooldown starts after attack finishes.
                _hasAttacked = false;
            }

            public override void ExecuteState(PetalsOfHope.AI.Core.StateMachine ownerStateMachine)
            {
                _attackTimer -= Time.deltaTime;

                // Apply damage at a specific point in the animation/attack duration
                // This could be triggered by an Animation Event on the "Attack" animation clip.
                // For simplicity here, let's assume damage is applied halfway through attackDuration if not event-driven.
                if (!_hasAttacked && _attackTimer <= attackDuration / 2f) // Example: damage point
                {
                    PerformAttack(ownerStateMachine);
                    _hasAttacked = true;
                }

                if (_attackTimer <= 0f) // Attack action/animation finished
                {
                    // Attack finished, now wait for cooldown
                    _cooldownTimer -= Time.deltaTime;
                    if (_cooldownTimer <= 0f)
                    {
                        // Cooldown finished, transition back
                        if (chaseState != null) ownerStateMachine.ChangeState(chaseState);
                        else Debug.LogWarning("ChaseState not set in AttackState SO, cannot transition post-cooldown.", this);
                    }
                    else
                    {
                        // Still in cooldown, perhaps play an idle or post-attack recovery animation
                        // For now, just waits. Could play "Idle" animation.
                        // ownerStateMachine.AnimationController.Play("Idle"); 
                    }
                }
            }

            private void PerformAttack(PetalsOfHope.AI.Core.StateMachine ownerStateMachine)
            {
                // Debug.Log($"{ownerStateMachine.Enemy.gameObject.name} performs attack!");
                if (ownerStateMachine.PlayerTransform == null) return;

                // Check distance again, player might have moved out of range
                float distanceToPlayer = Vector2.Distance(ownerStateMachine.Enemy.transform.position, ownerStateMachine.PlayerTransform.position);
                // Use a slightly larger range for applying damage than for initiating attack, or same attackRange.
                float effectiveAttackRange = ownerStateMachine.Enemy.Stats.attackRange * 1.1f; // Example: 10% leeway

                if (distanceToPlayer <= effectiveAttackRange)
                {
                    IDamageable playerDamageable = ownerStateMachine.PlayerTransform.GetComponent<IDamageable>();
                    if (playerDamageable != null)
                    {
                        playerDamageable.TakeDamage(ownerStateMachine.Enemy.Stats.damage);
                    }
                }
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
                // Debug.Log($"{ownerStateMachine.Enemy.gameObject.name} exiting Attack State.");
            }
        }
    }
    ```

# Acceptance Criteria:
- `AttackState.cs` ScriptableObject is created, inherits from `AI.Core.State`.
- `EnterState()`:
    - Stops enemy movement.
    - Makes enemy face the player.
    - Plays attack animation.
    - Initializes timers for `attackDuration` and `attackCooldown`.
- `ExecuteState()`:
    - Manages `attackTimer`. Calls `PerformAttack()` once at an appropriate time during the attack (e.g., halfway through `attackDuration` or via animation event).
    - After `attackTimer` expires, manages `attackCooldown` timer.
    - After `attackCooldown` expires, transitions to `chaseState`.
- `PerformAttack()`:
    - Checks if player is still in effective attack range.
    - If so, gets `IDamageable` from player and calls `TakeDamage(EnemyStatsSO.damage)`.
- `ExitState()` is present.
- Script compiles without errors.

# Test Strategy:
- Create an `AttackState` SO asset. Configure `attackDuration`, `attackCooldown`, and assign `ChaseState` SO.
- In an enemy prefab's AI `StateMachine`, ensure `ChaseState` can transition to this `AttackState` SO.
- Test in a scene:
    - Let enemy enter `AttackState` (e.g., by Player moving into attack range from `ChaseState`).
    - Verify enemy stops, faces Player, plays attack animation.
    - Verify `PerformAttack()` is called: Player (if implementing `IDamageable` and has `PlayerHealth`) takes damage.
    - Verify enemy waits for `attackCooldown` then transitions back to `ChaseState`.
    - Test scenario where Player moves out of effective attack range during attack windup (damage should not be applied).

# Notes/Questions:
- `EnemyStatsSO` needs `damage` field (already defined in plan for it).
- Using Animation Events to call `PerformAttack()` from the attack animation clip is a more precise way to time damage application than relying on `attackDuration / 2f`. This task assumes a timer-based approach for simplicity, but animation events are preferred for polish.
- `attackCooldown` could also be a parameter in `EnemyStatsSO` for per-enemy tuning.
- The state could play an "Idle" or "Recovery" animation during the cooldown period if desired.