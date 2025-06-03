# Task ID: 5.1.5
# Parent Task ID: 5.1
# Title: Implement OptionsMenuController and View (Basic)
# Status: pending
# Dependencies: 5.1.1 (UIManager), 5.2 (AudioManager for volume controls)
# Priority: medium
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement `OptionsMenuController.cs` and its UGUI view for basic game options, such as volume controls (Master, BGM, SFX).

# Details:
1.  **Create `OptionsMenuView.cs` (UGUI specific):**
    *   File Location: `Assets/_Project/Scripts/UI/OptionsMenu/OptionsMenuView.cs`
    *   Namespace: `PetalsOfHope.UI.OptionsMenu`
        ```csharp
        // In Assets/_Project/Scripts/UI/OptionsMenu/OptionsMenuView.cs
        namespace PetalsOfHope.UI.OptionsMenu
        {
            using UnityEngine;
            using UnityEngine.UI;

            public class OptionsMenuView : MonoBehaviour
            {
                [Header("Volume Controls")]
                public Slider masterVolumeSlider;
                public Text masterVolumeValueText; // Optional text display
                public Slider bgmVolumeSlider;
                public Text bgmVolumeValueText;
                public Slider sfxVolumeSlider;
                public Text sfxVolumeValueText;

                [Header("Other Options (Examples)")]
                // public Toggle fullscreenToggle;
                // public Dropdown resolutionDropdown;

                [Header("Action Buttons")]
                public Button applyButton; // If changes are not applied instantly
                public Button backButton; 
            }
        }
        ```
2.  **Create `OptionsMenuController.cs`:**
    *   File Location: `Assets/_Project/Scripts/UI/OptionsMenu/OptionsMenuController.cs`
    *   Namespace: `PetalsOfHope.UI.OptionsMenu`
        ```csharp
        // In Assets/_Project/Scripts/UI/OptionsMenu/OptionsMenuController.cs
        namespace PetalsOfHope.UI.OptionsMenu
        {
            using UnityEngine;
            using UnityEngine.Audio; // For AudioMixer
            // using PetalsOfHope.Audio; // For AudioManager (Task 5.2)

            [RequireComponent(typeof(OptionsMenuView))]
            public class OptionsMenuController : MonoBehaviour
            {
                private OptionsMenuView _view;
                // [SerializeField] private AudioMixer _mainAudioMixer; // Assign your game's main AudioMixer

                // Constants for PlayerPrefs keys and Mixer exposed parameters
                public const string MASTER_VOLUME_KEY = "MasterVolume";
                public const string BGM_VOLUME_KEY = "BGMVolume";
                public const string SFX_VOLUME_KEY = "SFXVolume";
                
                public const string MIXER_MASTER_VOL = "MasterVolumeParam"; // Exposed param name in AudioMixer
                public const string MIXER_BGM_VOL = "BGMVolumeParam";
                public const string MIXER_SFX_VOL = "SFXVolumeParam";


                private void Awake()
                {
                    _view = GetComponent<OptionsMenuView>();
                    // Find AudioManager or AudioMixer if not directly assigned.
                    // For now, assume AudioManager (Task 5.2) will handle PlayerPrefs and Mixer updates.
                    // This controller will primarily set up UI listeners and initial values.
                }

                private void Start()
                {
                    // Add listeners to UI elements
                    _view.masterVolumeSlider?.onValueChanged.AddListener(SetMasterVolume);
                    _view.bgmVolumeSlider?.onValueChanged.AddListener(SetBGMVolume);
                    _view.sfxVolumeSlider?.onValueChanged.AddListener(SetSFXVolume);
                    
                    _view.backButton?.onClick.AddListener(OnBackClicked);
                    _view.applyButton?.onClick.AddListener(OnApplyClicked); // If apply button is used

                    LoadSettings(); // Load and apply saved settings to UI
                }

                private void OnEnable()
                {
                    // Refresh UI with current settings when panel becomes active
                    LoadSettings();
                }

                private void LoadSettings()
                {
                    // Load volumes from PlayerPrefs and set sliders
                    // AudioManager (Task 5.2) will be responsible for the actual loading and applying to mixer.
                    // This controller just updates its UI elements to reflect current state.
                    // Example: float masterVol = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 0.8f);
                    // _view.masterVolumeSlider?.SetValueWithoutNotify(ConvertMixerVolToSlider(masterVol)); // Need conversion if mixer is log
                    
                    // For now, let's assume AudioManager has methods to get current normalized volumes (0-1)
                    if (PetalsOfHope.Audio.AudioManager.Instance != null) {
                        _view.masterVolumeSlider?.SetValueWithoutNotify(PetalsOfHope.Audio.AudioManager.Instance.GetVolume(MASTER_VOLUME_KEY));
                        _view.bgmVolumeSlider?.SetValueWithoutNotify(PetalsOfHope.Audio.AudioManager.Instance.GetVolume(BGM_VOLUME_KEY));
                        _view.sfxVolumeSlider?.SetValueWithoutNotify(PetalsOfHope.Audio.AudioManager.Instance.GetVolume(SFX_VOLUME_KEY));
                        UpdateVolumeText();
                    }
                }

                public void SetMasterVolume(float value)
                {
                    // AudioManager will handle PlayerPrefs.SetFloat and AudioMixer.SetFloat
                    PetalsOfHope.Audio.AudioManager.Instance?.SetVolume(MASTER_VOLUME_KEY, value);
                    UpdateVolumeText();
                }
                public void SetBGMVolume(float value)
                {
                    PetalsOfHope.Audio.AudioManager.Instance?.SetVolume(BGM_VOLUME_KEY, value);
                    UpdateVolumeText();
                }
                public void SetSFXVolume(float value)
                {
                    PetalsOfHope.Audio.AudioManager.Instance?.SetVolume(SFX_VOLUME_KEY, value);
                    UpdateVolumeText();
                }
                
                private void UpdateVolumeText() // Helper to update text displays
                {
                    if (_view.masterVolumeValueText != null && _view.masterVolumeSlider != null) 
                        _view.masterVolumeValueText.text = Mathf.RoundToInt(_view.masterVolumeSlider.value * 100).ToString();
                    if (_view.bgmVolumeValueText != null && _view.bgmVolumeSlider != null) 
                        _view.bgmVolumeValueText.text = Mathf.RoundToInt(_view.bgmVolumeSlider.value * 100).ToString();
                    if (_view.sfxVolumeValueText != null && _view.sfxVolumeSlider != null) 
                        _view.sfxVolumeValueText.text = Mathf.RoundToInt(_view.sfxVolumeSlider.value * 100).ToString();
                }

                private void OnApplyClicked()
                {
                    // SaveSettings(); // AudioManager handles saving in SetVolume usually
                    Debug.Log("Options Applied (usually saved on change)");
                    // Could close options menu here or rely on Back button.
                    // UIManager.Instance?.ClosePanel(gameObject);
                }

                private void OnBackClicked()
                {
                    // If options were opened from PauseMenu, UIManager might need to show PauseMenu again.
                    // Or simple close:
                    UIManager.Instance?.ClosePanel(gameObject);
                    // UIManager needs to know what was open before to restore (e.g. MainMenu or PauseMenu)
                    // For now, assume UIManager.ClosePanel also handles re-enabling previous panel if using a stack.
                }

                // Helper for converting linear slider (0-1) to dB for AudioMixer (-80 to 0)
                // public static float LinearToDecibel(float linear) { ... }
                // public static float DecibelToLinear(float dB) { ... }
                // AudioManager will handle this conversion.
            }
        }
        ```
3.  **Create Options Menu Panel Prefab (UGUI):**
    *   Location: `Assets/_Project/Prefabs/UI/OptionsMenuPanel.prefab`
    *   Panel `OptionsMenuPanel` with UI Sliders for Master, BGM, SFX volume. Add Text elements for volume values. Add "Back" button (and "Apply" if not applying on change).
    *   Attach `OptionsMenuView.cs` and `OptionsMenuController.cs`. Assign UI elements and AudioMixer (if controller handles it directly, otherwise AudioManager handles it).
4.  **Integrate with `UIManager`:**
    *   Assign `OptionsMenuPanel.prefab` to `UIManager._optionsMenuPanel`.
    *   `UIManager.ShowOptionsMenu()` will display it. `OptionsMenuController.OnBackClicked()` will close it.

# Acceptance Criteria:
- `OptionsMenuView.cs` and `OptionsMenuController.cs` are implemented.
- `OptionsMenuPanel.prefab` (UGUI) is created with volume sliders and a back button.
- Volume sliders control corresponding exposed parameters on an `AudioMixer` (via `AudioManager`).
- Volume settings are loaded from `PlayerPrefs` when the menu opens and set to sliders.
- Changes to sliders update `PlayerPrefs` and the `AudioMixer` (via `AudioManager`).
- "Back" button closes the Options Menu and returns to the previous menu (e.g., Main Menu or Pause Menu, handled by `UIManager`'s panel stack logic).

# Test Strategy:
- Manual Testing:
    - Open Options Menu from Main Menu and Pause Menu.
    - Adjust volume sliders: verify `AudioMixer` levels change (monitor in Unity Editor) and sound output volume changes.
    - Verify slider values are persisted (close and reopen game/menu, check if sliders retain values).
    - Test "Back" button functionality from different entry points.

# Notes/Questions:
- This task depends on `AudioManager` (Task 5.2) for actual volume control logic and PlayerPrefs interaction. `OptionsMenuController` mainly wires up UI to `AudioManager` methods.
- Conversion between linear slider values (0-1) and logarithmic decibel values for `AudioMixer.SetFloat()` is important. `AudioManager` should handle this. Example: `float db = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;`.
- The plan doesn't specify `AudioMixer` but it's standard Unity practice for volume groups.
- Other options (fullscreen, resolution) can be added later; volume is a common starting point.