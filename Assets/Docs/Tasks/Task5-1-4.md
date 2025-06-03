# Task ID: 5.1.4
# Parent Task ID: 5.1
# Title: Implement PauseMenuController and View
# Status: pending
# Dependencies: 5.1.1 (UIManager), 4.2.1 (SceneLoader)
# Priority: high
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement `PauseMenuController.cs` and its associated UGUI view. The Pause Menu should allow resuming the game, accessing options, or returning to the Main Menu.

# Details:
1.  **Create `PauseMenuView.cs` (UGUI specific):**
    *   File Location: `Assets/_Project/Scripts/UI/PauseMenu/PauseMenuView.cs`
    *   Namespace: `PetalsOfHope.UI.PauseMenu`
        ```csharp
        // In Assets/_Project/Scripts/UI/PauseMenu/PauseMenuView.cs
        namespace PetalsOfHope.UI.PauseMenu
        {
            using UnityEngine;
            using UnityEngine.UI;

            public class PauseMenuView : MonoBehaviour
            {
                [Header("Pause Menu Buttons")]
                public Button resumeButton;
                public Button optionsButton;
                public Button mainMenuButton;
            }
        }
        ```
2.  **Create `PauseMenuController.cs`:**
    *   File Location: `Assets/_Project/Scripts/UI/PauseMenu/PauseMenuController.cs`
    *   Namespace: `PetalsOfHope.UI.PauseMenu`
        ```csharp
        // In Assets/_Project/Scripts/UI/PauseMenu/PauseMenuController.cs
        namespace PetalsOfHope.UI.PauseMenu
        {
            using UnityEngine;
            // using PetalsOfHope.Systems.SceneManagement; // For SceneLoader (if needed directly, usually UIManager handles)

            [RequireComponent(typeof(PauseMenuView))]
            public class PauseMenuController : MonoBehaviour
            {
                private PauseMenuView _view;

                private void Awake()
                {
                    _view = GetComponent<PauseMenuView>();
                }

                private void Start()
                {
                    _view.resumeButton?.onClick.AddListener(OnResumeClicked);
                    _view.optionsButton?.onClick.AddListener(OnOptionsClicked);
                    _view.mainMenuButton?.onClick.AddListener(OnMainMenuClicked);
                }

                private void OnResumeClicked()
                {
                    Debug.Log("Resume Clicked");
                    UIManager.Instance?.TogglePauseMenu(); // UIManager handles unpausing and closing this panel
                }

                private void OnOptionsClicked()
                {
                    Debug.Log("Options Clicked from Pause Menu");
                    UIManager.Instance?.ShowOptionsMenu();
                    // gameObject.SetActive(false); // Optionally hide pause menu while options are open
                                                 // Or UIManager can handle layering.
                }

                private void OnMainMenuClicked()
                {
                    Debug.Log("Main Menu Clicked from Pause Menu");
                    Time.timeScale = 1f; // Ensure time is unpaused before leaving level
                    // Optional: Save game state before returning to main menu?
                    // SaveLoadManager.Instance?.SaveGame(); 

                    UIManager.Instance?.ClosePanel(gameObject); // Close pause menu first
                    // UIManager.Instance?.HideHUD(); // Ensure HUD is hidden
                    SceneLoader.Instance?.LoadMainMenu(); // Load the main menu scene
                }
            }
        }
        ```
3.  **Create Pause Menu Panel Prefab (UGUI):**
    *   Location: `Assets/_Project/Prefabs/UI/PauseMenuPanel.prefab`
    *   Panel GameObject `PauseMenuPanel` with buttons for "Resume", "Options", "Main Menu".
    *   Attach `PauseMenuView.cs` and `PauseMenuController.cs`. Assign buttons.
4.  **Integrate with `UIManager`:**
    *   Assign `PauseMenuPanel.prefab` to `UIManager._pauseMenuPanel`.
    *   `UIManager.TogglePauseMenu()` will show/hide this panel.

# Acceptance Criteria:
- `PauseMenuView.cs` and `PauseMenuController.cs` are implemented.
- `PauseMenuPanel.prefab` (UGUI) is created.
- "Resume" button calls `UIManager.Instance.TogglePauseMenu()` to unpause and close the menu.
- "Options" button opens the Options Menu (via `UIManager.Instance.ShowOptionsMenu()`).
- "Main Menu" button:
    - Restores `Time.timeScale` to 1.
    - Loads the Main Menu scene using `SceneLoader`.
    - Closes the Pause Menu.
- Pause Menu is correctly shown/hidden by `UIManager.TogglePauseMenu()`.

# Test Strategy:
- Manual Testing:
    - During gameplay in a level, trigger pause (e.g., press Escape). Verify Pause Menu appears.
    - **Resume:** Click, verify game unpauses, menu closes, HUD reappears, gameplay input active.
    - **Options:** Click, verify Options Menu opens.
    - **Main Menu:** Click, verify `Time.timeScale` is restored, Main Menu scene loads.

# Notes/Questions:
- The `UIManager` handles the core pause/unpause logic (Time.timeScale, input map switching). `PauseMenuController` just handles button actions within the pause menu.
- Decision whether to save game when returning to Main Menu from pause is a design choice.