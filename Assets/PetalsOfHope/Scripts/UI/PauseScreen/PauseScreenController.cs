using UnityEngine;
using PetalsOfHope.Core.Events;
using PetalsOfHope.Data.Levels;
using PetalsOfHope.Interfaces;
using UnityEngine.Serialization;

namespace PetalsOfHope.UI.PauseScreen
{
    /// <summary>
    /// Handles the logic for the Pause Screen UI.
    /// </summary>
    [RequireComponent(typeof(PauseScreenView))]
    public class PauseScreenController : MonoBehaviour
    {
        private PauseScreenView _view;

        [Header("Event Raisers")]
        [Tooltip("Event to request resuming the game.")]
        [SerializeField] private GameEventSO resumeGameRequest;
        [Tooltip("Event to request showing the options screen.")]
        [SerializeField] private GameEventSO showOptionsScreenRequest;
        [Tooltip("Event to request loading the main menu scene.")]
        [SerializeField] private LoadSceneRequestEventSO loadMainMenuRequest;

        [Header("Scene Data")]
        [Tooltip("The SceneDataSO for the main menu scene.")]
        [SerializeField] private SceneDataSO mainMenuSceneData;

        private void Awake()
        {
            _view = GetComponent<PauseScreenView>();
        }

        private void Start()
        {
            _view.ResumeButton?.onClick.AddListener(OnResumeClicked);
            _view.OptionsButton?.onClick.AddListener(OnOptionsClicked);
            _view.MainMenuButton?.onClick.AddListener(OnMainMenuClicked);
        }

        private void OnResumeClicked()
        {
            resumeGameRequest?.Raise();
        }

        private void OnOptionsClicked()
        {
            showOptionsScreenRequest?.Raise();
        }

        private void OnMainMenuClicked()
        {
            // Ensure time is unpaused before leaving the level.
            Time.timeScale = 1f;
            loadMainMenuRequest?.Raise(mainMenuSceneData);
        }
        
        /*
        HOW TO USE:
        1. Attach this component to the same GameObject as the PauseScreenView.
        2. Create and assign all required EventSO and SceneDataSO assets in the Inspector.
        3. The `_resumeGameRequest` should be listened to by the UISystem to unpause the game.
        4. The `_showOptionsScreenRequest` should be listened to by the UISystem to show the options screen.
        5. The `_loadMainMenuRequest` should be listened to by the SceneLoader to load the main menu.
        */
    }
}
