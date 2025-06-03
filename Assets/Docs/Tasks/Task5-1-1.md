# Task ID: 5.1.1
# Parent Task ID: 5.1
# Title: Decide on UI Technology and Basic UIManager Setup
# Status: pending
# Dependencies: 1.1.1 # Project Setup
# Priority: critical
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Officially decide on the UI technology (UI Toolkit or UGUI) for the project. Implement the basic structure of `UIManager.cs` as a Singleton or service to manage UI panels/screens and handle overall UI state.

# Details:
1.  **UI Technology Decision:**
    *   The plan states: "Decide on UI Technology: **UI Toolkit** (recommended for complex UIs) or **UGUI**."
    *   **Final Decision:** [To be filled by team - Assuming UGUI for current task generation unless specified otherwise]. For this task, proceeding with UGUI conceptualization.
2.  **Implement `UIManager.cs` (Basic Structure):**
    *   File Location: `Assets/_Project/Scripts/UI/UIManager.cs`
    *   Namespace: `PetalsOfHope.UI`
    *   Implementation (UGUI based conceptual structure):
        ```csharp
        // In Assets/_Project/Scripts/UI/UIManager.cs
        namespace PetalsOfHope.UI
        {
            using UnityEngine;
            using System.Collections.Generic;
            // using UnityEngine.InputSystem; // If UIManager handles global UI input like Pause
            using PetalsOfHope.Core.Input; // For InputReader if handling Pause
            using PetalsOfHope.Core.Events; // For GameEventSO (e.g., PauseGameEvent)

            public class UIManager : MonoBehaviour // Singleton
            {
                public static UIManager Instance { get; private set; }

                [Header("UI Panels (Assign in Inspector)")]
                [SerializeField] private GameObject _hudPanel;
                [SerializeField] private GameObject _mainMenuPanel;
                [SerializeField] private GameObject _pauseMenuPanel;
                [SerializeField] private GameObject _optionsMenuPanel;
                [SerializeField] private GameObject _dialoguePanel;
                // Add more panels as needed

                [Header("Input & Events")]
                [SerializeField] private InputReader _inputReader; // For pause input
                [SerializeField] private GameEventSO _pauseGameEvent; // Event to request pause
                [SerializeField] private GameEventSO _resumeGameEvent; // Event to request resume
                // [SerializeField] private PlayerInputActions _playerInputActions; // Direct input handling if needed

                private Stack<GameObject> _panelStack = new Stack<GameObject>(); // For managing panel history (e.g. back buttons)
                private bool _isGamePaused = false;

                private void Awake()
                {
                    if (Instance == null)
                    {
                        Instance = this;
                        DontDestroyOnLoad(gameObject);
                        // _playerInputActions = new PlayerInputActions(); // If handling input directly
                    }
                    else
                    {
                        Destroy(gameObject);
                        return;
                    }
                    // Initially, hide all managed panels except perhaps MainMenu if it's the entry point
                    // Or HUD if game starts directly. This depends on game flow.
                    _mainMenuPanel?.SetActive(true); // Example: Show MainMenu on start
                    _hudPanel?.SetActive(false); 
                    _pauseMenuPanel?.SetActive(false);
                    _optionsMenuPanel?.SetActive(false);
                    _dialoguePanel?.SetActive(false);
                }
                
                private void OnEnable()
                {
                    // If UIManager handles pause input directly via Input System actions
                    // _inputReader.PauseEvent.RegisterListener(TogglePauseMenu); // Assuming PauseEvent in InputReader
                }

                private void OnDisable()
                {
                    // _inputReader.PauseEvent.UnregisterListener(TogglePauseMenu);
                }

                private void Update()
                {
                    // Example: Handling pause key directly (alternative to InputReader event)
                    // if (Keyboard.current.escapeKey.wasPressedThisFrame) // Or gamepad pause button
                    // {
                    //    TogglePauseMenu();
                    // }
                }
                
                public void TogglePauseMenu()
                {
                    if (_isGamePaused)
                    {
                        ClosePanel(_pauseMenuPanel); // Close pause menu
                        // If options was open from pause, ensure it's also closed or handled by panel stack
                        if (_optionsMenuPanel.activeSelf) ClosePanel(_optionsMenuPanel);
                        
                        Time.timeScale = 1f;
                        _isGamePaused = false;
                        _inputReader?.EnableGameplayInput();
                        _resumeGameEvent?.Raise();
                        // If HUD was hidden by pause, show it.
                        _hudPanel?.SetActive(true); 
                    }
                    else
                    {
                        // Don't open pause menu if another full-screen menu is already open (e.g. dialogue, cutscene)
                        // Or handle panel layering more robustly.
                        OpenPanel(_pauseMenuPanel);
                        Time.timeScale = 0f;
                        _isGamePaused = true;
                        _inputReader?.EnableUIInput();
                        _pauseGameEvent?.Raise();
                        // Hide HUD when paused
                        _hudPanel?.SetActive(false);
                    }
                }

                public void OpenPanel(GameObject panelToShow)
                {
                    if (panelToShow == null) return;

                    // Simple hide current / show new logic. For stack behavior, push current to stack first.
                    // Example: Close currently active top panel IF it's not an overlay like HUD
                    if (_panelStack.Count > 0 && _panelStack.Peek() != _hudPanel && _panelStack.Peek() != panelToShow)
                    {
                        // _panelStack.Peek().SetActive(false); // If only one main panel at a time
                    }
                    
                    panelToShow.SetActive(true);
                    if (!_panelStack.Contains(panelToShow)) _panelStack.Push(panelToShow); // Add to stack if not already there
                    
                    // Ensure correct input map is active if opening a menu
                    if (panelToShow == _pauseMenuPanel || panelToShow == _mainMenuPanel || panelToShow == _optionsMenuPanel)
                    {
                        _inputReader?.EnableUIInput();
                    }
                }

                public void ClosePanel(GameObject panelToClose)
                {
                    if (panelToClose == null) return;
                    panelToClose.SetActive(false);
                    if (_panelStack.Count > 0 && _panelStack.Peek() == panelToClose)
                    {
                        _panelStack.Pop();
                    }
                    // If stack is empty or only HUD remains, and game is not meant to be paused, switch to gameplay input
                    if (_panelStack.Count == 0 && !_isGamePaused) { // Or count == 1 if HUD is always in stack base
                         _inputReader?.EnableGameplayInput();
                    } else if (_panelStack.Count > 0 && !_isGamePaused) { // If other UI is open but not pause menu
                        // Might still want UI input
                        _inputReader?.EnableUIInput();
                    }
                }
                
                // Methods for specific panels
                public void ShowMainMenu() => OpenPanel(_mainMenuPanel);
                public void ShowHUD() => OpenPanel(_hudPanel); // HUD might be always visible or context-dependent
                public void ShowOptionsMenu() => OpenPanel(_optionsMenuPanel);
                public void ShowDialoguePanel() => OpenPanel(_dialoguePanel);
                
                // TODO: Add logic for listening to game events (PlayerHealthChanged, TalismanAwarded) to update relevant UI controllers.
            }
        }
        ```

# Acceptance Criteria:
- UI Technology (UGUI or UI Toolkit) is officially chosen and documented.
- `UIManager.cs` (as a Singleton MonoBehaviour) is created.
- It has serialized fields for common UI panels (HUD, MainMenu, PauseMenu, OptionsMenu, DialoguePanel).
- It includes basic methods like `OpenPanel(GameObject panel)` and `ClosePanel(GameObject panel)`.
- It includes `TogglePauseMenu()` logic that:
    - Shows/hides the pause menu.
    - Sets `Time.timeScale` to 0 when paused, 1 when resumed.
    - Switches input maps using `InputReader` (`EnableUIInput`/`EnableGameplayInput`).
    - Raises `PauseGameEventSO`/`ResumeGameEventSO`.
- `UIManager` GameObject persists across scenes.
- Initial panel visibility is set correctly in `Awake()`.

# Test Strategy:
- Manual Testing:
    - Create a test scene with `UIManager` and placeholder GameObjects for each panel type (e.g., empty GameObjects or simple UGUI Panels). Assign them to `UIManager` fields.
    - Assign `InputReader` and create/assign `PauseGameEventSO`, `ResumeGameEventSO`.
    - In Play Mode, test `TogglePauseMenu()` (e.g., by pressing Escape if direct input is hooked up, or calling it from a test script).
    - Verify `Time.timeScale` changes.
    - Verify `InputReader` action maps are switched (use input debug logs from `InputReader` or `InputTestManager`).
    - Verify pause/resume events are raised (use EventListeners).
    - Test `OpenPanel` and `ClosePanel` with mock panels.

# Notes/Questions:
- The `UIManager` needs access to `InputReader` if it's responsible for handling the pause input. `InputReader` needs a "Pause" action and corresponding event. If `InputReader` doesn't have a Pause event, UIManager might listen to `Keyboard.current.escapeKey` directly.
- Panel management logic (`_panelStack`) is basic; more sophisticated stacking or queueing might be needed for complex UI flows.
- The current `OpenPanel` / `ClosePanel` is simple. A true stack system would involve hiding the panel below the current one if it's not an overlay.
- **Reconfirm UI Technology Choice:** The chosen technology will heavily influence subtasks 5.1.2 - 5.1.6. I've assumed UGUI for now.