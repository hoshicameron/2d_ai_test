# Task ID: 3.2.6
# Parent Task ID: 3.2
# Title: Configure AI State ScriptableObject Assets
# Status: completed
# Dependencies: 3.2.3, 3.2.4, 3.2.5 # Concrete AI State scripts
# Priority: critical
# Estimated Effort: S
# Assignee: Unassigned

# Description:
Create and configure the ScriptableObject assets for the implemented AI states (`PatrolState`, `ChaseState`, `AttackState`). Link them appropriately for transitions.

# Details:
1.  **Navigate to `Assets/_Project/ScriptableObjects/AI/States/`** (or a similar organized folder).
2.  **Create `PatrolState` Asset(s):**
    *   Right-click -> `Create > Petals of Hope/AI/States/Patrol State`.
    *   Name: e.g., `Wolf_PatrolStateSO`.
    *   Configure:
        *   `Waypoint Wait Time`: e.g., 1.5s.
        *   `Player Layer Mask`: Select the layer the Player character is on.
        *   `Chase State`: Assign the `Wolf_ChaseStateSO` (to be created next).
3.  **Create `ChaseState` Asset(s):**
    *   Right-click -> `Create > Petals of Hope/AI/States/Chase State`.
    *   Name: e.g., `Wolf_ChaseStateSO`.
    *   Configure:
        *   `Lose Player Time`: e.g., 3s.
        *   `Attack State`: Assign the `Wolf_AttackStateSO` (to be created next).
        *   `Patrol State`: Assign the `Wolf_PatrolStateSO` created above.
4.  **Create `AttackState` Asset(s):**
    *   Right-click -> `Create > Petals of Hope/AI/States/Attack State`.
    *   Name: e.g., `Wolf_AttackStateSO`.
    *   Configure:
        *   `Attack Duration`: e.g., 1.0s (tune with animation).
        *   `Attack Cooldown`: e.g., 2.0s.
        *   `Chase State`: Assign the `Wolf_ChaseStateSO`.
5.  **Repeat for other enemy types if they need different state parameters:**
    *   E.g., `Spider_PatrolStateSO`, `ArcherElf_AttackStateSO`. For now, focus on one set for a generic enemy or the first enemy type (e.g., Wolf).
6.  **Assign Initial State to Enemy Prefab:**
    *   On the enemy prefab (e.g., Wolf), find the AI `StateMachine` component.
    *   Drag the appropriate `_PatrolStateSO` (e.g., `Wolf_PatrolStateSO`) to its `Initial State` field.

# Acceptance Criteria:
- ScriptableObject assets for `PatrolState`, `ChaseState`, and `AttackState` are created.
- Parameters for these state assets (wait times, timers, transition links) are configured with initial values.
- State assets are correctly linked for transitions (e.g., Patrol links to Chase, Chase links to Attack and Patrol, Attack links to Chase).
- At least one enemy prefab is configured to use these AI state assets, with an initial state assigned.

# Test Strategy:
- This task is primarily configuration. Testing occurs when these assets are used by an enemy in-game (covered by testing of individual states and Task 3.3.3 Enemy Types & Prefabs).
- Manual Verification: Inspect each SO asset to ensure fields are set and references to other state SOs are correct.

# Notes/Questions:
- This step makes the AI system data-driven, allowing easy tuning of AI behaviors by designers without code changes.
- One set of state assets can be shared by multiple enemies of the same type, or unique sets can be created for variations.