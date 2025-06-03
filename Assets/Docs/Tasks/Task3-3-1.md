# Task ID: 3.3.1
# Parent Task ID: 3.3
# Title: Implement WolfEnemy Script and Prefab
# Status: pending
# Dependencies: 3.1.2, 3.2.6, 1.3.5 # EnemyBase, AI State SOs, WolfEnemyStatsSO
# Priority: high
# Estimated Effort: L
# Assignee: Unassigned

# Description:
Implement the `WolfEnemy.cs` script deriving from `EnemyBase` for specific wolf behaviors (e.g., patrol and charge). Create a Wolf enemy prefab with all necessary components and configurations.

# Details:
1.  **Create `WolfEnemy.cs` Script:**
    *   File Location: `Assets/_Project/Scripts/Enemies/Types/WolfEnemy.cs`
    *   Namespace: `PetalsOfHope.Enemies.Types`
    *   Implementation:
        ```csharp
        // In Assets/_Project/Scripts/Enemies/Types/WolfEnemy.cs
        namespace PetalsOfHope.Enemies.Types
        {
            using UnityEngine;
            using PetalsOfHope.Enemies.Core; // For EnemyBase
            // Potentially using specific states if Wolf has unique ones not covered by generic SOs
            // using PetalsOfHope.AI.States;

            public class WolfEnemy : EnemyBase
            {
                // Wolf-specific parameters or behaviors can be added here if needed.
                // For example, a charge attack might be initiated from a custom WolfAttackState or by overriding
                // parts of the generic AttackState logic if it's flexible enough.

                // For now, WolfEnemy might just use the standard EnemyBase behavior
                // with its AI states configured via EnemyStatsSO and AI.StateMachine.

                protected override void Awake()
                {
                    base.Awake(); // Important to call base class Awake
                    // Wolf-specific Awake logic here
                }

                protected override void Start()
                {
                    base.Start();
                    // Wolf-specific Start logic here
                }

                // Example of overriding a method from EnemyBase
                // protected override void OnDamaged()
                // {
                //     base.OnDamaged();
                //     // Wolf-specific reaction to damage, e.g., a short stun or howl
                // }

                // If Wolf has a unique "Charge" behavior, it might be triggered by a specific AI state
                // or a modification to the AttackState.
                // public void StartCharge() { /* ... */ }
                // public void ExecuteCharge() { /* ... */ }
                // public void EndCharge() { /* ... */ }
            }
        }
        ```
    *   Initially, `WolfEnemy.cs` might be very simple, relying on the AI states configured on its `AI.StateMachine` component for behavior. Specific overrides can be added as distinct Wolf behaviors are defined. The plan mentions "Patrol and charge behavior". The charge could be a specialized attack state.

2.  **Create Wolf Animator Controller:**
    *   Location: `Assets/_Project/Art/Animations/Enemies/Wolf/WolfAnimatorController.controller`
    *   Define states: `Idle`, `Walk`, `Run` (Chase), `Attack` (e.g., Bite/Charge), `Hurt`, `Death`.
    *   Define parameters: e.g., `IsMoving` (bool), `IsChasing` (bool), `AttackTrigger` (trigger), `HurtTrigger` (trigger), `DeathTrigger` (trigger).
    *   Set up transitions between these animation states. (These are Unity Animator states, not AI states).

3.  **Create Wolf Prefab:**
    *   Location: `Assets/_Project/Prefabs/Enemies/WolfPrefab.prefab`
    *   Create an empty GameObject, name it `WolfEnemy`.
    *   Add a child GameObject for Sprite/Visuals. Assign a placeholder wolf sprite.
    *   **Components on `WolfEnemy` root:**
        *   `WolfEnemy.cs` (or `EnemyBase.cs` if WolfEnemy script is empty initially).
        *   `Rigidbody2D`: Configure `Body Type` (Dynamic), `Mass`, `Gravity Scale`. Freeze Z rotation.
        *   `CapsuleCollider2D` (or `BoxCollider2D`): Adjust to fit sprite.
        *   `Animator`: Assign the `WolfAnimatorController`.
        *   `PetalsOfHope.Core.Animation.AnimationController` (our script).
        *   `PetalsOfHope.AI.Core.StateMachine` (our AI StateMachine script).
            *   **Initial State:** Assign the `Wolf_PatrolStateSO` (created in Task 3.2.6).
        *   `PetalsOfHope.AI.Components.PatrolPath`: Add and define waypoints if it patrols.
    *   **Configure `WolfEnemy` (Script) Component:**
        *   `Stats`: Assign `WolfEnemyStatsSO` (created in Task 1.3.5 or updated).
        *   `Enemy Died Event SO`: Assign a general `EnemyDiedEventSO` or a wolf-specific one.
    *   **Configure `EnemyStatsSO` for Wolf (`WolfEnemyStatsSO`):**
        *   Ensure `maxHealth`, `patrolSpeed`, `chaseSpeed`, `detectionRange`, `damage`, `attackRange` are set.
        *   (New) Add `attackAnimNameOrHash` (string or int), `walkAnimNameOrHash`, etc., if AI states use these to tell `Core.Animation.AnimationController` what to play. Or, AI states might directly use hardcoded animation names like "Attack", "Walk". For flexibility, `EnemyStatsSO` could hold these. Example: `public string attackAnimationName = "Attack";`

4.  **Implement "Charge" Behavior (If distinct from standard attack):**
    *   This might involve creating a `WolfChargeStateSO` inheriting from `AI.Core.State`.
    *   This state would accelerate the wolf towards the player, deal damage on impact, and have a cooldown.
    *   The `Wolf_AttackStateSO` assigned to the Wolf could be this `WolfChargeStateSO`.
    *   Or, the generic `AttackState` could be configured with parameters that make it a charge (e.g. high movement speed during attack duration). For now, assume the generic `AttackState` is used, and "charge" is just how its attack visually appears.

# Acceptance Criteria:
- `WolfEnemy.cs` script exists (can be minimal initially if behavior is fully driven by generic AI states).
- `WolfAnimatorController` is created with basic animation states (Idle, Walk, Run, Attack, Hurt, Death) and parameters.
- `WolfPrefab.prefab` is created and correctly configured with all necessary components, `WolfEnemyStatsSO`, and AI states (initial state linked to `Wolf_PatrolStateSO` which links to `Wolf_ChaseStateSO` and `Wolf_AttackStateSO`).
- Wolf enemy patrols using `PatrolPath` waypoints.
- Wolf detects player, transitions to chase, and moves towards player.
- Wolf transitions to attack when player is in range, plays attack animation, and (conceptually) deals damage.
- Wolf can be damaged and dies, playing death animation.
- "Charge behavior" is represented, either as a specific implementation or a visual style of the standard attack.

# Test Strategy:
- Place `WolfPrefab` in a test scene with the player.
- Observe patrol behavior, detection of player, transition to chase.
- Verify chase speed and animation.
- Test player entering attack range: wolf should attack.
- Test player damaging wolf: wolf should show hurt feedback (if any) and die when health is depleted.
- Verify death animation and object cleanup.

# Notes/Questions:
- The "charge behavior" for the wolf needs clarification. Is it a special move, or just its standard attack pattern? Assuming for now it's the visual style of its attack, handled by the generic `AttackState` and Wolf's attack animation. If it's a mechanically distinct charge (e.g., move fast, hit hard, longer cooldown), a new `WolfChargeStateSO` would be appropriate.
- Animation names in AI states or `EnemyStatsSO` need to match those in `WolfAnimatorController`.