# Task ID: 1.2.8
# Parent Task ID: 1.2
# Title: Create Initial EventSO Assets
# Status: completed
# Dependencies: 1.2.2, 1.2.3 # Event SO definitions
# Priority: medium
# Estimated Effort: S
# Assignee: Unassigned

# Description:
Create a set of initial `GameEventSO` and `TypedEventSO<T>` (e.g., `IntEventSO`) assets in the project under `_Project/ScriptableObjects/Events/`. These will be used by various game systems.

# Details:
1.  **Navigate to Folder:** `Assets/_Project/ScriptableObjects/Events/`
2.  **Create `GameEventSO` Assets:**
    *   Right-click in the Project window -> `Create > Petals of Hope/Events/Game Event`.
    *   Examples (names are indicative, can be refined based on specific needs discovered later):
        *   `PlayerJumpedEventSO`
        *   `PlayerLandedEventSO`
        *   `PlayerDiedEventSO`
        *   `EnemyDiedEventSO`
        *   `PauseGameEventSO`
        *   `ResumeGameEventSO`
        *   `LevelStartedEventSO`
        *   `LevelCompletedEventSO`
3.  **Create `TypedEventSO<T>` Assets:**
    *   Right-click in the Project window -> `Create > Petals of Hope/Events/Typed Events/...`
    *   Examples:
        *   **IntEventSO:**
            *   `PlayerHealthChangedEventSO` (payload: new health, or remaining health and max health struct)
            *   `ScoreUpdatedEventSO` (payload: new score)
            *   `DamageDealtEventSO` (payload: damage amount)
        *   **Vector2EventSO:**
            *   `PlayerMoveInputEventSO` (payload: move vector)
            *   `CameraShakeRequestedEventSO` (payload: intensity/duration vector)
        *   **StringEventSO:**
            *   `ShowNotificationEventSO` (payload: message string)
            *   `SceneLoadRequestEventSO` (payload: scene name)
        *   **GameObjectEventSO:**
            *   `EnemySpawnedEventSO` (payload: spawned enemy GameObject)
            *   `ItemCollectedEventSO` (payload: collected item GameObject)
        *   **BoolEventSO:**
            *   `ToggleSettingEventSO` (payload: new boolean state for a setting)
4.  **Naming Convention:**
    *   Use a clear and descriptive name for each event.
    *   End with `EventSO` (e.g., `PlayerJumpedEventSO`).
5.  **Developer Description:**
    *   For each created event asset, fill in the "Developer Description" field in the Inspector to briefly explain its purpose and when it's typically raised.

# Acceptance Criteria:
- A selection of commonly anticipated `GameEventSO` assets are created in `Assets/_Project/ScriptableObjects/Events/`.
- A selection of commonly anticipated `TypedEventSO<T>` assets (for types like int, Vector2, string, GameObject, bool) are created in the same folder.
- Each created event asset has a meaningful name and a developer description filled in.
- Assets are functional and can be assigned to listeners or raised by scripts.

# Test Strategy:
- Manual Verification:
    - Check the `Assets/_Project/ScriptableObjects/Events/` folder to ensure assets are present.
    - Select a few assets and verify their type and developer description in the Inspector.
    - (Later) As systems are implemented, these events will be naturally tested by assigning them and observing their behavior.

# Notes/Questions:
- The list of events provided is a starting point. More events will likely be identified and created as development progresses through other phases.
- For typed events like `PlayerHealthChangedEventSO`, consider if a simple `int` payload (current health) is sufficient, or if a struct/class (e.g., `HealthChangeData { int current; int max; int change; }`) would be more informative. The plan specifies `TypedEventSO<int>` for `playerHealthChangedEvent` under PlayerHealth, so an `int` payload is the current direction.