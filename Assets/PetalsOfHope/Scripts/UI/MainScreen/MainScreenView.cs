using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace PetalsOfHope.UI.MainScreen
{
    /// <summary>
    /// Holds references to all the UI elements for the Main Screen.
    /// </summary>
    public class MainScreenView : MonoBehaviour
    {
        [Header("Main Screen Buttons")]
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button loadGameButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button quitButton;

        public Button NewGameButton => newGameButton;
        public Button LoadGameButton => loadGameButton;
        public Button OptionsButton => optionsButton;
        public Button QuitButton => quitButton;

        public void SetLoadGameButtonActive(bool isActive)
        {
            if (loadGameButton != null)
            {
                loadGameButton.interactable = isActive;
            }
        }
    }
}
