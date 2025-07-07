# Task ID: 1.4.2
# Parent Task ID: 1.4
# Title: Define Save/Load EventSO Assets
# Status: completed
# Dependencies: 1.2.2, 1.2.8 # GameEventSO definition and asset creation task
# Priority: high
# Estimated Effort: S
# Assignee: Unassigned

# Description:
Define and create `GameEventSO` assets for save and load operations, such as `OnBeforeSave`, `OnAfterLoad`, `OnSaveFailed`, and `OnLoadFailed`.

# Details:
1.  **Navigate to Folder:** `Assets/_Project/ScriptableObjects/Events/Persistence/` (Create `Persistence` subfolder if it doesn't exist).
2.  **Create `GameEventSO` Assets:**
    *   Use the `Create > Petals of Hope/Events/Game Event` menu.
    *   **`OnBeforeSaveGameEventSO`**:
        *   Name: `OnBeforeSaveGameEventSO`
        *   Developer Description: "Raised just before the game state is captured and saved. Systems can use this to finalize any pending state."
    *   **`OnAfterSaveGameEventSO`**: (Added for completeness, plan only mentioned `OnAfterLoad`)
        *   Name: `OnAfterSaveGameEventSO`
        *   Developer Description: "Raised immediately after the game state has been successfully saved."
    *   **`OnAfterLoadGameEventSO`**:
        *   Name: `OnAfterLoadGameEventSO`
        *   Developer Description: "Raised after game state has been successfully loaded and restored. Systems can use this to initialize based on loaded data."
    *   **`OnSaveFailedEventSO`**:
        *   Name: `OnSaveFailedEventSO`
        *   Developer Description: "Raised if the save operation encounters an error."
        *   (Optional) Consider if this should be a `TypedEventSO<string>` to pass an error message. For now, sticking to `GameEventSO` as per plan's direct mention.
    *   **`OnLoadFailedEventSO`**:
        *   Name: `OnLoadFailedEventSO`
        *   Developer Description: "Raised if the load operation encounters an error (e.g., file not found, corrupted data)."
        *   (Optional) Consider if this should be a `TypedEventSO<string>` to pass an error message.

# Acceptance Criteria:
- `GameEventSO` assets for `OnBeforeSaveGameEventSO`, `OnAfterSaveGameEventSO`, `OnAfterLoadGameEventSO`, `OnSaveFailedEventSO`, and `OnLoadFailedEventSO` are created.
- These assets are stored in `Assets/_Project/ScriptableObjects/Events/Persistence/`.
- Each asset has a clear developer description.

# Test Strategy:
- Manual Verification:
    - Check the specified folder for the new event assets.
    - Inspect each asset to verify its name and description.
- These events will be tested when the `SaveLoadManager` (Task 1.4.5) is implemented and raises them.

# Notes/Questions:
- Added `OnAfterSaveGameEventSO` as it's a common and useful event in save/load flows.
- The plan mentions `OnBeforeSave` and `OnAfterLoad`. `OnSaveFailed` and `OnLoadFailed` are also explicitly mentioned.
- The choice between `GameEventSO` and `TypedEventSO<string>` for failure events depends on whether an error message payload is desired. The current implementation plan implies `GameEventSO`. This can be revisited.