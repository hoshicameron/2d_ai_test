using System.Collections.Generic;
using System.Linq;
using PetalsOfHope.Data;
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

        private ScreenView _currentScreen;
        private readonly List<ScreenView> _instantiatedScreens = new();
        private readonly List<object> _controllers = new();

        private void Awake()
        {
            InstantiateAndInitializeScreens();
            CreateControllers();
            SubscribeToScreenEvents();
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
        /// Shows a screen of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the screen to show.</typeparam>
        public void ShowScreen<T>() where T : ScreenView
        {
            var screenToShow = _instantiatedScreens.FirstOrDefault(s => s is T);
            if (screenToShow == null)
            {
                Debug.LogError($"{nameof(UISystem)}: Screen of type {typeof(T).Name} not found.");
                return;
            }

            if (_currentScreen != null)
            {
                _currentScreen.Hide();
            }

            _currentScreen = screenToShow;
            _currentScreen.Show();
        }

        private void SubscribeToScreenEvents()
        {
            if (uiEvents == null) return;

            uiEvents.ShowGameplayScreenEvent.RegisterListener(ShowGameplayScreen);
            uiEvents.ShowOptionsScreenEvent.RegisterListener(ShowOptionsScreen);
            uiEvents.ShowMainMenuScreenEvent.RegisterListener(ShowMainMenuScreen);
        }

        private void UnsubscribeFromScreenEvents()
        {
            if (uiEvents == null) return;

            uiEvents.ShowGameplayScreenEvent.UnregisterListener(ShowGameplayScreen);
            uiEvents.ShowOptionsScreenEvent.UnregisterListener(ShowOptionsScreen);
            uiEvents.ShowMainMenuScreenEvent.UnregisterListener(ShowMainMenuScreen);
        }

        private void ShowGameplayScreen() => ShowScreen<GameplayScreenView>();
        private void ShowOptionsScreen() => ShowScreen<OptionsScreenView>();
        private void ShowMainMenuScreen() => ShowScreen<MainScreenView>();
    }
}
