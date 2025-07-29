using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace PetalsOfHope.UI.OptionsScreen
{
    /// <summary>
    /// Holds references to all the UI elements for the Options Screen.
    /// </summary>
    public class OptionsScreenView : MonoBehaviour
    {
        [Header("Volume Controls")]
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider bgmVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;

        [Header("Action Buttons")]
        [SerializeField] private Button backButton;

        public Slider MasterVolumeSlider => masterVolumeSlider;
        public Slider BgmVolumeSlider => bgmVolumeSlider;
        public Slider SfxVolumeSlider => sfxVolumeSlider;
        public Button BackButton => backButton;
    }
}
