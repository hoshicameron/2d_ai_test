# Task ID: 3.3.2
# Parent Task ID: 3.3
# Title: Implement SpiderEnemy Script and Prefab
# Status: pending
# Dependencies: 3.1.2, 3.2.6, 1.3.5 # EnemyBase, AI State SOs, EnemyStatsSO assets
# Priority: medium # After Wolf to prove pattern
# Estimated Effort: L
# Assignee: Unassigned

# Description:
Implement the `SpiderEnemy.cs` script for specific spider behaviors (e.g., ceiling hanging, drop, climb). Create a Spider enemy prefab.

# Details:
1.  **Create `SpiderEnemy.cs` Script:**
    *   File Location: `Assets/_Project/Scripts/Enemies/Types/SpiderEnemy.cs`
    *   Namespace: `PetalsOfHope.Enemies.Types`
    *   Implementation:
        ```csharp
        // In Assets/_Project/Scripts/Enemies/Types/SpiderEnemy.cs
        namespace PetalsOfHope.Enemies.Types
        {
            using UnityEngine;
            using PetalsOfHope.Enemies.Core;
            // using PetalsOfHope.AI.States; // For potential custom Spider states

            public class SpiderEnemy : EnemyBase
            {
                // Spider-specific parameters
                [Header("Spider Specifics")]
                public float dropSpeed = 10f;
                public float climbSpeed = 2f;
                // public bool canHangFromCeiling = true; // Configure via AI States instead

                // Custom states for Spider if needed:
                // public SpiderHangingStateSO HangingStateAsset;
                // public SpiderDroppingStateSO DroppingStateAsset;
                // public SpiderClimbingStateSO ClimbingStateAsset;

                protected override void Awake()
                {
                    base.Awake();
                    // Spider-specific Awake
                }
                
                // Override methods or add new ones for unique spider behaviors
                // e.g., methods to initiate drop or climb, check for ceilings/walls.
            }
        }
        ```
2.  **Create Spider AI States (if unique behaviors are not covered by generic states):**
    *   **`SpiderHangingStateSO`**:
        *   Enemy stays on ceiling, perhaps sways. Detects player below. Transitions to `SpiderDroppingStateSO`.
    *   **`SpiderDroppingStateSO`**:
        *   Enemy falls quickly. Transitions to a ground-based state (e.g., a generic `ChaseState` or `SpiderGroundPatrolStateSO`) upon landing.
    *   **`SpiderClimbingStateSO`**: (If spider can climb walls/ceilings after dropping)
        *   Moves along surfaces.
    *   If these are implemented, create SO assets for them and link transitions.
    *   Alternatively, generic `PatrolState` might be adapted if "waypoints" can be on ceilings, and "attack" is simply dropping.

3.  **Create Spider Animator Controller:**
    *   Location: `Assets/_Project/Art/Animations/Enemies/Spider/SpiderAnimatorController.controller`
    *   States: `Idle_Ceiling`, `Drop_Anticipation`, `Falling`, `Idle_Ground`, `Walk_Ground`, `Attack_Ground`, `Climb`, `Hurt`, `Death`.
    *   Parameters and transitions.

4.  **Create Spider Prefab:**
    *   Location: `Assets/_Project/Prefabs/Enemies/SpiderPrefab.prefab`
    *   Setup similarly to Wolf prefab: Sprite, `SpiderEnemy.cs` (or `EnemyBase`), `Rigidbody2D`, `Collider2D`, `Animator` (with `SpiderAnimatorController`), `CoreAnimation`, AI `StateMachine`.
    *   **AI `StateMachine` Configuration:**
        *   Initial State: Could be `SpiderHangingStateSO` if implemented, or a `PatrolState` adapted for ceiling.
    *   **`SpiderEnemy` (Script) Configuration:**
        *   Assign `SpiderEnemyStatsSO` (create if distinct from generic `EnemyStatsSO`).
        *   Assign events.
    *   Spider's `Rigidbody2D` might have `gravityScale = 0` while hanging/climbing, then `gravityScale = 1` (or custom) when dropping/on ground. This would be managed by its AI states.

# Acceptance Criteria:
- `SpiderEnemy.cs` script exists.
- Custom AI states for hanging, dropping, climbing are implemented if needed, along with their SO assets.
- `SpiderAnimatorController` with relevant animations/states.
- `SpiderPrefab.prefab` is configured.
- Spider enemy exhibits its unique behaviors (e.g., starts on ceiling, drops when player is below, then perhaps patrols/attacks on ground).
- Spider can be damaged and dies.

# Test Strategy:
- Place `SpiderPrefab` in a test scene with ceilings and ground, and the player.
- Verify spider's initial state (e.g., hanging).
- Test player moving under spider: verify drop behavior.
- Observe behavior after landing (patrol, chase, attack).
- Test climbing if implemented.
- Test combat interactions.

# Notes/Questions:
- Spider behavior can be complex. Start with a simplified version (e.g., hang and drop, then basic ground movement).
- Managing gravity scale and physics for ceiling hang/climb vs. ground movement is a key challenge for spider AI states. States will need to modify `Rigidbody2D.gravityScale` and potentially `Rigidbody2D.isKinematic`.
- For "ceiling hanging", the spider might need a trigger collider above it to detect ceilings, or its patrol path might be defined on the ceiling.