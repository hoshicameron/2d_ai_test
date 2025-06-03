# Task ID: 5.1.6
# Parent Task ID: 5.1
# Title: Implement Basic Dialogue System UI
# Status: pending
# Dependencies: 5.1.1 (UIManager) # Event Bus for triggering dialogue
# Priority: medium
# Estimated Effort: L
# Assignee: Unassigned

# Description:
Implement a basic UI for displaying dialogue. This includes a panel with text area for dialogue lines, character name display (optional), and a way to advance dialogue (e.g., click/button press).

# Details:
1.  **Define Dialogue Data Structure (if not already available):**
    *   Could be a `ScriptableObject` like `DialogueSO.cs` or simple struct/class.
    *   `DialogueSO` might contain:
        *   `CharacterName` (string, optional)
        *   `List<string> dialogueLines`
        *   `CharacterPortrait` (Sprite, optional)
        *   `AudioClip` for voice line (optional)
        *   `EventSO` to raise on dialogue completion (optional)
    *   For now, assume a simple `ShowDialogueEventSO(string speaker, string line)` or `ShowDialogueSequenceEventSO(DialogueData)` event triggers this UI.

2.  **Create `DialogueUIView.cs` (UGUI specific):**
    *   File Location: `Assets/_Project/Scripts/UI/Dialogue/DialogueUIView.cs`
    *   Namespace: `PetalsOfHope.UI.Dialogue`
        ```csharp
        // In Assets/_Project/Scripts/UI/Dialogue/DialogueUIView.cs
        namespace PetalsOfHope.UI.Dialogue
        {
            using UnityEngine;
            using UnityEngine.UI; // Or TMPro.TextMeshProUGUI
            // using UnityEngine.InputSystem; // For advancing dialogue

            public class DialogueUIView : MonoBehaviour
            {
                [Header("Dialogue UI Elements")]
                public GameObject dialoguePanelRoot; // The main panel to show/hide
                public Text speakerNameText;    // Optional
                public Text dialogueLineText;
                public Image characterPortraitImage; // Optional
                public Button continueButton; // Or listen for generic submit input

                public void ShowPanel(bool show)
                {
                    dialoguePanelRoot?.SetActive(show);
                }

                public void SetDialogue(string speaker, string line, Sprite portrait = null)
                {
                    if (speakerNameText != null)
                    {
                        speakerNameText.text = speaker;
                        speakerNameText.gameObject.SetActive(!string.IsNullOrEmpty(speaker));
                    }
                    if (dialogueLineText != null)
                    {
                        dialogueLineText.text = line; // Could implement typewriter effect here
                    }
                    if (characterPortraitImage != null)
                    {
                        characterPortraitImage.sprite = portrait;
                        characterPortraitImage.gameObject.SetActive(portrait != null);
                    }
                }
            }
        }
        ```
3.  **Create `DialogueUIController.cs`:**
    *   File Location: `Assets/_Project/Scripts/UI/Dialogue/DialogueUIController.cs`
    *   Namespace: `PetalsOfHope.UI.Dialogue`
        ```csharp
        // In Assets/_Project/Scripts/UI/Dialogue/DialogueUIController.cs
        namespace PetalsOfHope.UI.Dialogue
        {
            using UnityEngine;
            using System.Collections.Generic;
            using PetalsOfHope.Core.Events; // For listening to dialogue trigger events
            // Assuming a simple event to show a list of lines:
            // public class DialogueSequenceData { public string Speaker; public List<string> Lines; public Sprite Portrait; }
            // public class DialogueSequenceEventSO : TypedEventSO<DialogueSequenceData> {}

            [RequireComponent(typeof(DialogueUIView))]
            public class DialogueUIController : MonoBehaviour
            {
                private DialogueUIView _view;
                private Queue<string> _dialogueLinesQueue = new Queue<string>();
                private string _currentSpeaker;
                private Sprite _currentPortrait;
                private bool _isDialogueActive = false;

                // Example event to trigger dialogue display
                // [SerializeField] private DialogueSequenceEventSO _showDialogueSequenceEvent;
                // Or a simpler event for single lines:
                // [SerializeField] private TypedEventSO<string> _showSingleLineDialogueEvent; 

                // For now, let's assume a public method to start dialogue
                // public void StartDialogue(DialogueSequenceData data) { ... }

                private void Awake()
                {
                    _view = GetComponent<DialogueUIView>();
                    _view.ShowPanel(false); // Start hidden
                }

                private void OnEnable()
                {
                    // _showDialogueSequenceEvent?.RegisterListener(StartDialogueSequence);
                    _view.continueButton?.onClick.AddListener(OnContinueClicked);
                    // Alternatively, listen to global UI Submit event from InputReader if UIManager passes it.
                }

                private void OnDisable()
                {
                    // _showDialogueSequenceEvent?.UnregisterListener(StartDialogueSequence);
                     _view.continueButton?.onClick.RemoveListener(OnContinueClicked);
                }
                
                // Public method to be called by external systems (e.g., interaction triggers, cutscene manager)
                public void StartDialogue(string speaker, List<string> lines, Sprite portrait = null) {
                    if (lines == null || lines.Count == 0) return;

                    _currentSpeaker = speaker;
                    _currentPortrait = portrait;
                    _dialogueLinesQueue = new Queue<string>(lines);
                    
                    _isDialogueActive = true;
                    _view.ShowPanel(true);
                    UIManager.Instance?.InputReader?.EnableUIInput(); // Ensure UI input for dialogue
                    Time.timeScale = 0f; // Optional: Pause game during dialogue
                    
                    DisplayNextLine();
                }

                private void OnContinueClicked()
                {
                    if (!_isDialogueActive) return;
                    DisplayNextLine();
                }

                private void DisplayNextLine()
                {
                    if (_dialogueLinesQueue.Count > 0)
                    {
                        string line = _dialogueLinesQueue.Dequeue();
                        _view.SetDialogue(_currentSpeaker, line, _currentPortrait);
                    }
                    else
                    {
                        EndDialogue();
                    }
                }

                private void EndDialogue()
                {
                    _isDialogueActive = false;
                    _view.ShowPanel(false);
                    UIManager.Instance?.InputReader?.EnableGameplayInput(); // Or previous input state
                    Time.timeScale = 1f; // Resume game
                    // Raise DialogueEndedEventSO if other systems need to know
                }
            }
        }
        ```
4.  **Create Dialogue Panel Prefab (UGUI):**
    *   Location: `Assets/_Project/Prefabs/UI/DialoguePanel.prefab`
    *   Panel `DialoguePanel` with UI Text elements for Speaker Name, Dialogue Line. Optional Image for portrait, Button for "Continue".
    *   Attach `DialogueUIView.cs` and `DialogueUIController.cs`. Assign UI elements.
    *   (Optional) Assign event SOs for triggering dialogue if using event-driven approach.
5.  **Integrate with `UIManager`:**
    *   Assign `DialoguePanel.prefab` to `UIManager._dialoguePanel`.
    *   `UIManager.ShowDialoguePanel()` could be called, or `DialogueUIController.StartDialogue()` directly.

# Acceptance Criteria:
- `DialogueUIView.cs` and `DialogueUIController.cs` are implemented.
- `DialoguePanel.prefab` (UGUI) is created with text areas, optional portrait/speaker name, and continue button/mechanism.
- `DialogueUIController.StartDialogue(speaker, lines, portrait)` method can be called to display a sequence of dialogue lines.
- Clicking "Continue" (or pressing submit) advances to the next line or closes the dialogue panel if it's the last line.
- Dialogue panel is shown/hidden correctly.
- (Optional) Game pauses (`Time.timeScale = 0`) during dialogue and resumes after.
- (Optional) Input map switches to UI during dialogue.

# Test Strategy:
- Manual Testing:
    - Add `DialoguePanel.prefab` to a test scene, managed by `UIManager` or with `DialogueUIController` accessible.
    - Create a test script that calls `DialogueUIController.StartDialogue()` with sample speaker name and lines.
    - Verify dialogue panel appears, displays first line correctly.
    - Click "Continue": verify next line appears. Repeat until dialogue ends and panel closes.
    - Test with/without speaker name and portrait.
    - Verify game pause/resume and input map switching if implemented.

# Notes/Questions:
- The actual triggering of dialogue (e.g., from player interacting with an NPC, or a cutscene manager) is not part of this UI task but will call `DialogueUIController.StartDialogue()`.
- A typewriter effect for `dialogueLineText` is a common polish item that can be added later to `DialogueUIView`.
- This is a *basic* dialogue UI. More advanced features (choice C, branching dialogue, rich text) are out of scope for this initial pass.
- Need to define how dialogue sequences are triggered (e.g., via a `TypedEventSO<DialogueSequenceData>`). The example `StartDialogue` method is a direct call for now.