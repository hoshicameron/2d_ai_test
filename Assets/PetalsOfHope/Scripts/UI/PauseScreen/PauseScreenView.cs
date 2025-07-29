using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace PetalsOfHope.UI.PauseScreen
{
    /// <summary>
    /// Holds references to all the UI elements for the Pause Screen.
    /// </summary>
    public class PauseScreenView : MonoBehaviour
    {
        [Header("Pause Screen Buttons")]
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button mainMenuButton;

        public Button ResumeButton => resumeButton;
        public Button OptionsButton => optionsButton;
        public Button MainMenuButton => mainMenuButton;
    }
}
