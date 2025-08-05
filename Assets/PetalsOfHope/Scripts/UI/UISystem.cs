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
        [Header("Screens")]
        [SerializeField] private List<ScreenView> screens;

        
        [Header("Event Channels")]
        [SerializeField] private UIEventChannels uiEvents;

        private ScreenView _currentScreen;
        private readonly List<object> _controllers = new();

        private void Awake()
        {
            // Initialize all screens and hide them by default.
            foreach (var screen in screens)
            {
                screen.Initialize();
                screen.Hide();
            }

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

            foreach (var screen in screens)
            {
                screen.Terminate();
            }
            
            UnsubscribeFromScreenEvents();
        }

        private void CreateControllers()
        {
            var gameplayScreenView = screens.OfType<GameplayScreenView>().FirstOrDefault();
            if (gameplayScreenView != null)
            {
                var controller = new GameplayScreenController(gameplayScreenView, uiEvents);
                _controllers.Add(controller);
            }

            var mainScreenView = screens.OfType<MainScreenView>().FirstOrDefault();
            if (mainScreenView != null)
            {
                var controller = new MainScreenController(mainScreenView, uiEvents);
                _controllers.Add(controller);
            }

            var optionsScreenView = screens.OfType<OptionsScreenView>().FirstOrDefault();
            if (optionsScreenView != null)
            {
                var controller = new OptionsScreenController(optionsScreenView, uiEvents);
                _controllers.Add(controller);
            }

            var splashScreenView = screens.OfType<SplashScreenView>().FirstOrDefault();
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
            var screenToShow = screens.FirstOrDefault(s => s is T);
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
