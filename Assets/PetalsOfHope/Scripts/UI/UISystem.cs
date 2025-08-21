using System.Collections.Generic;
using System.Linq;
using PetalsOfHope.Core.Events.Channels;
using PetalsOfHope.UI.Base;
using PetalsOfHope.UI.Controllers;
using PetalsOfHope.UI.Screens;
using UnityEngine;

namespace PetalsOfHope.UI
{
    /// <summary>
    /// Manages the UI screens in the game, ensuring that only one screen is active at a time.
    /// Acts as the bootstrapper for the UI system, creating controllers and injecting dependencies.
    /// </summary>
    public class UISystem : MonoBehaviour
    {
        [Header("Screen Prefabs")]
        [SerializeField] private List<ScreenView> screenPrefabs;
        
        [Header("Event Channels")]
        [SerializeField] private UIEventChannels uiEvents;
        

        private readonly Stack<ScreenView> _navigationStack = new();
        private readonly List<ScreenView> _instantiatedScreens = new();
        private readonly List<object> _controllers = new();

        private void Awake()
        {
            InstantiateAndInitializeScreens();
            CreateControllers();
            SubscribeToScreenEvents();
        }

        private void Start()
        {
            // Show the main menu as the initial screen
            ShowScreen<MainScreenView>();
        }

        private void OnDestroy()
        {
            // Terminate all controllers
            foreach (var controller in _controllers)
            {
                switch (controller)
                {
                    case GameplayScreenController c1:
                        c1.Terminate();
                        break;
                    case MainScreenController c2:
                        c2.Terminate();
                        break;
                    case OptionsScreenController c3:
                        c3.Terminate();
                        break;
                    case SplashScreenController c4:
                        c4.Terminate();
                        break;
                }
            }

            // Terminate and destroy screens
            foreach (var screen in _instantiatedScreens)
            {
                screen.Terminate();
            }
            
            UnsubscribeFromScreenEvents();
        }
        
        private void InstantiateAndInitializeScreens()
        {
            foreach (var screenPrefab in screenPrefabs)
            {
                var screenInstance = Instantiate(screenPrefab, transform);
                _instantiatedScreens.Add(screenInstance);
                screenInstance.Initialize();
                screenInstance.Hide();
            }
        }

        private void CreateControllers()
        {
            var gameplayScreenView = _instantiatedScreens.OfType<GameplayScreenView>().FirstOrDefault();
            if (gameplayScreenView != null)
            {
                var controller = new GameplayScreenController(gameplayScreenView, uiEvents);
                _controllers.Add(controller);
            }

            var mainScreenView = _instantiatedScreens.OfType<MainScreenView>().FirstOrDefault();
            if (mainScreenView != null)
            {
                var controller = new MainScreenController(mainScreenView, uiEvents);
                _controllers.Add(controller);
            }

            var optionsScreenView = _instantiatedScreens.OfType<OptionsScreenView>().FirstOrDefault();
            if (optionsScreenView != null)
            {
                var controller = new OptionsScreenController(optionsScreenView, uiEvents);
                _controllers.Add(controller);
            }

            var splashScreenView = _instantiatedScreens.OfType<SplashScreenView>().FirstOrDefault();
            if (splashScreenView != null)
            {
                var controller = new SplashScreenController(splashScreenView, uiEvents);
                _controllers.Add(controller);
            }
        }

        /// <summary>
        /// Shows a screen of the specified type and adds it to the navigation stack.
        /// </summary>
        /// <typeparam name="T">The type of the screen to show.</typeparam>
        public void ShowScreen<T>() where T : ScreenView
        {
            if (_navigationStack.Count > 0)
            {
                var currentScreen = _navigationStack.Peek();
                currentScreen.Hide();
            }

            var screenToShow = _instantiatedScreens.FirstOrDefault(s => s is T);
            if (screenToShow == null)
            {
                Debug.LogError($"{nameof(UISystem)}: Screen of type {typeof(T).Name} not found.");
                return;
            }

            _navigationStack.Push(screenToShow);
            screenToShow.Show();
        }

        /// <summary>
        /// Hides the current screen and shows the previous one in the navigation stack.
        /// </summary>
        public void GoToPreviousScreen()
        {
            if (_navigationStack.Count <= 1)
            {
                Debug.LogWarning($"{nameof(UISystem)}: No previous screen in the stack to go back to.");
                return;
            }

            var currentScreen = _navigationStack.Pop();
            currentScreen.Hide();

            var previousScreen = _navigationStack.Peek();
            previousScreen.Show();
        }

        private void SubscribeToScreenEvents()
        {
            if (uiEvents == null) return;

            // Screen navigation
            uiEvents.ShowGameplayScreenEvent.RegisterListener(ShowGameplayScreen);
            uiEvents.ShowOptionsScreenEvent.RegisterListener(ShowOptionsScreen);
            uiEvents.ShowMainMenuScreenEvent.RegisterListener(ShowMainMenuScreen);
            uiEvents.BackEvent.RegisterListener(GoToPreviousScreen);
            
            // Volume event forwarding
            uiEvents.MasterVolumeChangedEvent.RegisterListener(OnMasterVolumeChanged_Forwarder);
            uiEvents.BgmVolumeChangedEvent.RegisterListener(OnBgmVolumeChanged_Forwarder);
            uiEvents.SfxVolumeChangedEvent.RegisterListener(OnSfxVolumeChanged_Forwarder);
        }

        private void UnsubscribeFromScreenEvents()
        {
            if (uiEvents == null) return;

            // Screen navigation
            uiEvents.ShowGameplayScreenEvent.UnregisterListener(ShowGameplayScreen);
            uiEvents.ShowOptionsScreenEvent.UnregisterListener(ShowOptionsScreen);
            uiEvents.ShowMainMenuScreenEvent.UnregisterListener(ShowMainMenuScreen);
            uiEvents.BackEvent.UnregisterListener(GoToPreviousScreen);
            
            // Volume event forwarding
            uiEvents.MasterVolumeChangedEvent.UnregisterListener(OnMasterVolumeChanged_Forwarder);
            uiEvents.BgmVolumeChangedEvent.UnregisterListener(OnBgmVolumeChanged_Forwarder);
            uiEvents.SfxVolumeChangedEvent.UnregisterListener(OnSfxVolumeChanged_Forwarder);
        }

        // --- Event Forwarders ---
        private void OnMasterVolumeChanged_Forwarder(float value) => uiEvents.MasterVolumeChangedEvent.Raise(value);
        private void OnBgmVolumeChanged_Forwarder(float value) => uiEvents.BgmVolumeChangedEvent.Raise(value);
        private void OnSfxVolumeChanged_Forwarder(float value) => uiEvents.SfxVolumeChangedEvent.Raise(value);

        private void ShowGameplayScreen() => ShowScreen<GameplayScreenView>();
        private void ShowOptionsScreen() => ShowScreen<OptionsScreenView>();
        private void ShowMainMenuScreen() => ShowScreen<MainScreenView>();
    }
}
