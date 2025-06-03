# Task ID: 5.1.3
# Parent Task ID: 5.1
# Title: Implement MainMenuController and View
# Status: pending
# Dependencies: 5.1.1 (UIManager), 4.2.1 (SceneLoader), 1.4.5 (SaveLoadManager for "Load Game" button)
# Priority: high
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement `MainMenuController.cs` and its associated UGUI view. The Main Menu should provide options like "New Game", "Load Game" (if save data exists), "Options", and "Quit".

# Details:
1.  **Create `MainMenuView.cs` (UGUI specific):**
    *   File Location: `Assets/_Project/Scripts/UI/MainMenu/MainMenuView.cs`
    *   Namespace: `PetalsOfHope.UI.MainMenu`
        ```csharp
        // In Assets/_Project/Scripts/UI/MainMenu/MainMenuView.cs
        namespace PetalsOfHope.UI.MainMenu
        {
            using UnityEngine;
            using UnityEngine.UI; // For Button

            public class MainMenuView : MonoBehaviour
            {
                [Header("Main Menu Buttons")]
                public Button newGameButton;
                public Button loadGameButton;
                public Button optionsButton;
                public Button quitButton;

                // Method to control interactability of Load Game button
                public void SetLoadGameButtonActive(bool isActive)
                {
                    if (loadGameButton != null)
                    {
                        loadGameButton.interactable = isActive;
                    }
                }
            }
        }
        ```
2.  **Create `MainMenuController.cs`:**
    *   File Location: `Assets/_Project/Scripts/UI/MainMenu/MainMenuController.cs`
    *   Namespace: `PetalsOfHope.UI.MainMenu`
        ```csharp
        // In Assets/_Project/Scripts/UI/MainMenu/MainMenuController.cs
        namespace PetalsOfHope.UI.MainMenu
        {
            using UnityEngine;
            using PetalsOfHope.Systems.SceneManagement; // For SceneLoader & SceneDataSO
            using PetalsOfHope.Systems.Persistence; // For SaveLoadManager (if static instance)

            [RequireComponent(typeof(MainMenuView))]
            public class MainMenuController : MonoBehaviour
            {
                private MainMenuView _view;
                // [SerializeField] private SceneDataSO _firstLevelSceneData; // Assign SceneDataSO for the first level

                // Name of the first game scene to load for "New Game"
                [Header("Scene Configuration")]
                [Tooltip("Name of the first playable game scene.")]
                [SerializeField] private string _firstGameSceneName = "Level1"; // Or use SceneDataSO

                private void Awake()
                {
                    _view = GetComponent<MainMenuView>();
                }

                private void Start()
                {
                    // Add listeners to buttons
                    _view.newGameButton?.onClick.AddListener(OnNewGameClicked);
                    _view.loadGameButton?.onClick.AddListener(OnLoadGameClicked);
                    _view.optionsButton?.onClick.AddListener(OnOptionsClicked);
                    _view.quitButton?.onClick.AddListener(OnQuitClicked);

                    // Check if save data exists to enable/disable Load Game button
                    // This assumes SaveLoadManager has an Instance and HasSaveData method.
                    bool hasSaveData = SaveLoadManager.Instance != null && SaveLoadManager.Instance.HasSaveData();
                    _view.SetLoadGameButtonActive(hasSaveData);
                }

                private void OnEnable()
                {
                    // Refresh Load Game button state when menu becomes active (e.g., if returning from game)
                    bool hasSaveData = SaveLoadManager.Instance != null && SaveLoadManager.Instance.HasSaveData();
                    _view.SetLoadGameButtonActive(hasSaveData);
                }

                private void OnNewGameClicked()
                {
                    Debug.Log("New Game Clicked");
                    // Optional: Delete any existing save data for a true new game experience
                    // SaveLoadManager.Instance?.DeleteSave(); // Or specific save slot if multiple
                    
                    // Initialize progression for a new game (if not handled by SaveLoadManager on no data)
                    PetalsOfHope.Systems.Progression.GameProgressionManager.Instance?.RestoreState(null); // Pass null to trigger default init

                    if (!string.IsNullOrEmpty(_firstGameSceneName))
                    {
                        SceneLoader.Instance?.LoadScene(_firstGameSceneName);
                    }
                    else
                    {
                        Debug.LogError("First Game Scene Name not set in MainMenuController.", this);
                    }
                    // UIManager.Instance?.ClosePanel(gameObject); // Close main menu panel
                    // UIManager.Instance?.ShowHUD(); // Show HUD
                }

                private void OnLoadGameClicked()
                {
                    Debug.Log("Load Game Clicked");
                    // SaveLoadManager will load data. Scene to load might be part of save data,
                    // or always load into a specific hub/last played level.
                    // For now, assume LoadGame also goes to first level, but after loading save.
                    SaveLoadManager.Instance?.LoadGame(); // This should restore progression, inventory, player stats, etc.
                                                         // And potentially load the last saved scene (if SaveLoadManager handles that).
                                                         // If not, we need to decide which scene to load after data restore.
                    
                    // If SaveLoadManager doesn't handle scene loading as part of LoadGame():
                    // string sceneToLoadAfterLoad = GetLastSavedSceneName(); // Logic needed for this
                    // SceneLoader.Instance?.LoadScene(sceneToLoadAfterLoad); 
                    // For now, just load the first level as a placeholder if SLM doesn't handle scene.
                    if (!string.IsNullOrEmpty(_firstGameSceneName))
                    {
                        SceneLoader.Instance?.LoadScene(_firstGameSceneName); // Placeholder: load first level
                    }
                     // UIManager.Instance?.ClosePanel(gameObject);
                     // UIManager.Instance?.ShowHUD();
                }

                private void OnOptionsClicked()
                {
                    Debug.Log("Options Clicked");
                    UIManager.Instance?.ShowOptionsMenu(); // UIManager handles opening Options panel
                    // gameObject.SetActive(false); // Optionally hide main menu while options are open
                }

                private void OnQuitClicked()
                {
                    Debug.Log("Quit Clicked");
                    Application.Quit();
                    #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false; // Also stop play mode in editor
                    #endif
                }
            }
        }
        ```
3.  **Create Main Menu Panel Prefab (UGUI):**
    *   Location: `Assets/_Project/Prefabs/UI/MainMenuPanel.prefab`
    *   Create a Panel GameObject `MainMenuPanel`.
    *   Add child UI Buttons for "New Game", "Load Game", "Options", "Quit". Style them.
    *   Attach `MainMenuView.cs` to `MainMenuPanel`. Assign buttons to its fields.
    *   Attach `MainMenuController.cs` to `MainMenuPanel`. Assign `_firstGameSceneName` or `_firstLevelSceneData`.
4.  **Integrate with `UIManager`:**
    *   Assign `MainMenuPanel.prefab` to `UIManager._mainMenuPanel`.
    *   Ensure `UIManager` shows `MainMenuPanel` on game start (if it's the entry point).

# Acceptance Criteria:
- `MainMenuView.cs` and `MainMenuController.cs` are implemented.
- `MainMenuPanel.prefab` (UGUI) is created with buttons for New Game, Load Game, Options, Quit.
- "New Game" button:
    - Optionally deletes existing save data.
    - Initializes default game progression.
    - Loads the first game scene (e.g., "Level1") using `SceneLoader`.
- "Load Game" button:
    - Is interactable only if `SaveLoadManager.Instance.HasSaveData()` is true.
    - Calls `SaveLoadManager.Instance.LoadGame()`.
    - Loads the appropriate scene (e.g., last saved scene or first level for now).
- "Options" button opens the Options Menu (via `UIManager.Instance.ShowOptionsMenu()`).
- "Quit" button closes the application.
- Main Menu is correctly shown/hidden by `UIManager`.

# Test Strategy:
- Manual Testing:
    - Ensure `MainMenuPanel.prefab` is shown on game start (or accessible via `UIManager`).
    - **New Game:** Click, verify console logs, verify first level loads.
    - **Save/Load:**
        - Play game, save progress (e.g., collect a talisman, reach a checkpoint).
        - Return to Main Menu. "Load Game" button should be active. Click it.
        - Verify game loads with previous progress restored and player in correct scene/state.
        - If no save data, "Load Game" button should be inactive.
    - **Options:** Click, verify Options Menu opens (placeholder for now).
    - **Quit:** Click, verify application closes (in build) or Play Mode stops (in editor).

# Notes/Questions:
- The scene to load after `SaveLoadManager.Instance.LoadGame()` needs careful consideration. Does the save file store the last scene name? Or does loading always take you to a hub or the first level? The current example loads `_firstGameSceneName` as a placeholder.
- The `_firstLevelSceneData` (SceneDataSO) is an alternative to string `_firstGameSceneName` for better scene referencing.
- `OnEnable()` in `MainMenuController` refreshes the Load Game button state in case the player quits to menu after saving/deleting save.