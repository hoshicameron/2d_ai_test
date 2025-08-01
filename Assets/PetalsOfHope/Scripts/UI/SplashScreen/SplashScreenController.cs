using UnityEngine;
using PetalsOfHope.Core.Events;
using PetalsOfHope.Data.Levels;

namespace PetalsOfHope.UI.SplashScreen
{
    /// <summary>
    /// Manages the splash screen logic. Waits for an event indicating that all
    /// core systems are initialized, then requests a transition to the main menu.
    /// </summary>
    [RequireComponent(typeof(SplashScreenView))]
    public class SplashScreenController : MonoBehaviour
    {
        private SplashScreenView _view;
        [Header("Event Listeners")]
        [Tooltip("Event to listen for, indicating that core systems are ready.")]
        [SerializeField] private GameEventSO onSystemsInitialized;

        [Header("Event Raisers")]
        [Tooltip("Event to raise to request loading the main menu scene.")]
        [SerializeField] private LoadSceneRequestEventSO loadMainScreenRequest;
        
        [Header("Event Listeners")]
        [Tooltip("Event that broadcasts the loading progress (0.0 to 1.0).")]
        [SerializeField] private FloatEventSO onLoadingProgressUpdate;

        [Header("Scene Data")]
        [Tooltip("The SceneDataSO for the main menu, to be loaded after initialization.")]
        [SerializeField] private SceneDataSO mainScreenSceneData;

        private void Awake()
        {
            _view = GetComponent<SplashScreenView>();
        }

        private void OnEnable()
        {
            onSystemsInitialized?.RegisterListener(OnSystemsInitialized);
            onLoadingProgressUpdate?.RegisterListener(SetLoadingProgress);
        }

        private void OnDisable()
        {
            onSystemsInitialized?.UnregisterListener(OnSystemsInitialized);
            onLoadingProgressUpdate?.UnregisterListener(SetLoadingProgress);
        }

        private void SetLoadingProgress(float value)
        {
            _view.SetLoadingProgress(value);
        }

        private void OnSystemsInitialized()
        {
            // Once systems are ready, request to load the main menu.
            if (loadMainScreenRequest != null && mainScreenSceneData != null)
            {
                loadMainScreenRequest.Raise(mainScreenSceneData);
            }
            else
            {
                Debug.LogError("LoadMainScreenRequest or MainScreenSceneData is not set on the SplashScreenController.", this);
            }
        }
        
        /*
        HOW TO USE:
        1. Create a "Startup" scene. This should be the first scene in your build settings (index 0).
        2. In the Startup scene, create a Canvas with your splash screen UI (e.g., a logo, a loading bar).
        3. Attach a `SplashScreenView` to the root of the splash screen UI and link its `loadingBar` field.
        4. Add this `SplashScreenController` component to a GameObject in the scene.
        5. Create and assign all required EventSO and SceneDataSO assets in the Inspector.
        6. A separate "SystemsInitializer" script on a persistent GameManager object will be responsible for:
           a. Periodically raising the `onLoadingProgressUpdate` (FloatEventSO) to update the loading bar.
           b. Raising the `onSystemsInitialized` (GameEventSO) event once all core systems are ready, which triggers the scene transition.
        */
    }
}
