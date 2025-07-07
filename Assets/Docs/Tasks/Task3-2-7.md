# Task ID: 3.2.7
# Parent Task ID: 3.2
# Title: Implement AI Event Raising (e.g., EnemyDetectedPlayerEventSO)
# Status: completed
# Dependencies: 3.2.3, 1.2.2, 1.2.8 # PatrolState, GameEventSO definition, EventSO assets
# Priority: medium
# Estimated Effort: S
# Assignee: Unassigned

# Description:
Integrate event raising into AI states as specified in the plan. For example, `PatrolState` should raise an `EnemyDetectedPlayerEventSO` when it detects the player.

# Details:
1.  **Create Event SO (if not already existing):**
    *   Navigate to `Assets/_Project/ScriptableObjects/Events/AI/` (create AI subfolder).
    *   Create a `GameEventSO` (or `TypedEventSO<GameObject>` if player reference is needed).
    *   Name: `EnemyDetectedPlayerEventSO`.
    *   Developer Description: "Raised when an AI enemy detects the player for the first time (e.g., in PatrolState)."
2.  **Add Event Field to relevant AI State SOs:**
    *   Modify `PatrolState.cs` (Task 3.2.3) to include a field for this event:
        ```csharp
        // In PatrolState.cs
        // ...
        [Header("Events")]
        [Tooltip("Event to raise when the player is first detected.")]
        public GameEventSO playerDetectedEvent; // Or TypedEventSO<GameObject>
        // ...
        ```
3.  **Raise Event in `PatrolState.ExecuteState()`:**
    *   Modify the `DetectPlayer()` method or the part of `ExecuteState()` that handles detection in `PatrolState.cs`:
        ```csharp
        // In PatrolState.ExecuteState() where player is detected and transition to ChaseState happens:
        // ...
        if (DetectPlayer())
        {
            playerDetectedEvent?.Raise(); // Or playerDetectedEvent?.Raise(_owner.PlayerTransform.gameObject);
            if (chaseState != null)
            {
                ownerStateMachine.ChangeState(chaseState);
            }
            // ...
            return;
        }
        // ...
        ```
4.  **Assign Event SO Asset:**
    *   Select the `PatrolState` SO asset (e.g., `Wolf_PatrolStateSO`).
    *   In the Inspector, drag the `EnemyDetectedPlayerEventSO` asset to the `Player Detected Event` field.
5.  **Consider other events:**
    *   `EnemyLostPlayerEventSO` (from ChaseState back to Patrol)
    *   `EnemyStartedAttackEventSO`
    *   These can be added similarly if needed.

# Acceptance Criteria:
- `EnemyDetectedPlayerEventSO` (or similar) is created and configured.
- `PatrolState.cs` has a field for this event and raises it when the player is first detected before transitioning to `ChaseState`.
- The `PatrolState` SO asset is updated to reference the actual `EnemyDetectedPlayerEventSO` asset.
- Event raising is functional and can be observed by listeners.

# Test Strategy:
- Manual/Integration Testing:
    - Create an `EventListener` in the scene that listens to `EnemyDetectedPlayerEventSO` and logs a message.
    - Let an enemy in `PatrolState` detect the Player.
    - Verify the log message appears, confirming the event was raised.
    - Verify the payload is correct if using a `TypedEventSO`.

# Notes/Questions:
- The plan specifically mentions "States should raise events via Event Bus (e.g., `EnemyDetectedPlayerEventSO`)". This task implements that.
- This allows other systems (UI, Audio, Game Manager) to react to AI events without direct coupling.