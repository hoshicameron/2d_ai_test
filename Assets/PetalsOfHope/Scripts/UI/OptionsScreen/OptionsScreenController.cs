using UnityEngine;
using PetalsOfHope.Core.Events;
using UnityEngine.Serialization;

namespace PetalsOfHope.UI.OptionsScreen
{
    /// <summary>
    /// Handles the logic for the Options Screen UI.
    /// </summary>
    [RequireComponent(typeof(OptionsScreenView))]
    public class OptionsScreenController : MonoBehaviour
    {
        private OptionsScreenView _view;

        [Header("Event Raisers")]
        [Tooltip("Event to request hiding the options screen.")]
        [SerializeField] private GameEventSO hideOptionsScreenRequest;

        // This controller will need to communicate with an AudioManager.
        // For now, the logic will be placeholders.

        private void Awake()
        {
            _view = GetComponent<OptionsScreenView>();
        }

        private void Start()
        {
            _view.MasterVolumeSlider?.onValueChanged.AddListener(SetMasterVolume);
            _view.BgmVolumeSlider?.onValueChanged.AddListener(SetBgmVolume);
            _view.SfxVolumeSlider?.onValueChanged.AddListener(SetSfxVolume);
            _view.BackButton?.onClick.AddListener(OnBackClicked);

            LoadSettings();
        }

        private void LoadSettings()
        {
            // Placeholder: Load settings from PlayerPrefs or an AudioManager
            _view.MasterVolumeSlider?.SetValueWithoutNotify(PlayerPrefs.GetFloat("MasterVolume", 1f));
            _view.BgmVolumeSlider?.SetValueWithoutNotify(PlayerPrefs.GetFloat("BgmVolume", 1f));
            _view.SfxVolumeSlider?.SetValueWithoutNotify(PlayerPrefs.GetFloat("SfxVolume", 1f));
        }

        private void SetMasterVolume(float value)
        {
            // Placeholder: This would typically call an AudioManager
            PlayerPrefs.SetFloat("MasterVolume", value);
        }

        private void SetBgmVolume(float value)
        {
            // Placeholder: This would typically call an AudioManager
            PlayerPrefs.SetFloat("BgmVolume", value);
        }

        private void SetSfxVolume(float value)
        {
            // Placeholder: This would typically call an AudioManager
            PlayerPrefs.SetFloat("SfxVolume", value);
        }

        private void OnBackClicked()
        {
            hideOptionsScreenRequest?.Raise();
        }
        
        /*
        HOW TO USE:
        1. Attach this component to the same GameObject as the OptionsScreenView.
        2. Create and assign the `_hideOptionsScreenRequest` GameEventSO in the Inspector.
        3. This event should be listened to by the UISystem to hide the options screen.
        4. The volume control logic is currently a placeholder using PlayerPrefs. This should be
           integrated with a dedicated AudioManager that controls an AudioMixer.
        */
    }
}
