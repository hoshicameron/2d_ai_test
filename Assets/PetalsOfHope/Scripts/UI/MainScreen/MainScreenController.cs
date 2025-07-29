using UnityEngine;
using PetalsOfHope.Core.Events;
using PetalsOfHope.Data.Levels;
using PetalsOfHope.Interfaces;
using UnityEngine.Serialization;

namespace PetalsOfHope.UI.MainScreen
{
    /// <summary>
    /// Handles the logic for the Main Screen UI.
    /// </summary>
    [RequireComponent(typeof(MainScreenView))]
    public class MainScreenController : MonoBehaviour
    {
        private MainScreenView view;

        [Header("Event Raisers")]
        [Tooltip("Event to request loading the first level for a new game.")]
        [SerializeField] private LoadSceneRequestEventSO loadNewGameRequest;
        [Tooltip("Event to request loading a saved game.")]
        [SerializeField] private GameEventSO loadSavedGameRequest;
        [Tooltip("Event to request showing the options screen.")]
        [SerializeField] private GameEventSO showOptionsScreenRequest;

        [Header("Scene Data")]
        [Tooltip("The SceneDataSO for the first level of the game.")]
        [SerializeField] private SceneDataSO firstLevelSceneData;

        // This controller needs a way to check if save data exists.
        // This will be handled by a function provider channel from the SaveLoadSystem.
        // For now, we'll assume it's always available.

        private void Awake()
        {
            view = GetComponent<MainScreenView>();
        }

        private void Start()
        {
            view.NewGameButton?.onClick.AddListener(OnNewGameClicked);
            view.LoadGameButton?.onClick.AddListener(OnLoadGameClicked);
            view.OptionsButton?.onClick.AddListener(OnOptionsClicked);
            view.QuitButton?.onClick.AddListener(OnQuitClicked);

            // Logic to check for save data and enable/disable the load button
            // will be added once the SaveLoadSystem provides a function channel for it.
            view.SetLoadGameButtonActive(true); // Placeholder
        }

        private void OnNewGameClicked()
        {
            loadNewGameRequest?.Raise(firstLevelSceneData);
        }

        private void OnLoadGameClicked()
        {
            loadSavedGameRequest?.Raise();
        }

        private void OnOptionsClicked()
        {
            showOptionsScreenRequest?.Raise();
        }

        private void OnQuitClicked()
        {
            Application.Quit();
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
        
        /*
        HOW TO USE:
        1. Attach this component to the same GameObject as the MainScreenView.
        2. Create and assign all required EventSO and SceneDataSO assets in the Inspector.
        3. The `_loadNewGameRequest` should be listened to by the SceneLoader to load the first level.
        4. The `_loadSavedGameRequest` should be listened to by the SaveLoadSystem to load the game state.
        5. The `_showOptionsScreenRequest` should be listened to by the UISystem to show the options screen.
        */
    }
}
