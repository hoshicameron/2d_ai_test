using UnityEngine;
using System.Collections.Generic;
using PetalsOfHope.Core.Input;
using PetalsOfHope.Core.Events;

namespace PetalsOfHope.UI
{
    /// <summary>
    /// Manages the visibility and state of all major UI screens.
    /// Operates on an event-driven basis, without a singleton instance.
    /// </summary>
    public class UISystem : MonoBehaviour
    {
        [Header("UI Screens (Assign in Inspector)")]
        [SerializeField] private GameObject hudScreen;
        [SerializeField] private GameObject mainScreen;
        [SerializeField] private GameObject pauseScreen;
        [SerializeField] private GameObject optionsScreen;

        [Header("Input & Events")]
        [SerializeField] private InputReader inputReader;
        [SerializeField] private GameEventSO pauseGameEvent;
        [SerializeField] private GameEventSO resumeGameEvent;
        
        [Header("Screen Control Events")]
        [SerializeField] private GameEventSO showMainScreenEvent;
        [SerializeField] private GameEventSO hideMainScreenEvent;
        [SerializeField] private GameEventSO showHUDScreenEvent;
        [SerializeField] private GameEventSO hideHUDScreenEvent;
        [SerializeField] private GameEventSO showOptionsScreenEvent;
        [SerializeField] private GameEventSO hideOptionsScreenEvent;

        private bool isGamePaused = false;

        private void Awake()
        {
            // This system should be on a persistent object, loaded from the Startup scene.
            DontDestroyOnLoad(gameObject);

            // Initial setup: show main screen, hide others.
            mainScreen?.SetActive(true);
            hudScreen?.SetActive(false);
            pauseScreen?.SetActive(false);
            optionsScreen?.SetActive(false);
        }

        private void OnEnable()
        {
            // The connection for pausing is made via UnityEvents in the Inspector.
            // 1. Create a GameEventSO asset (e.g., "PauseRequestEvent").
            // 2. In the InputReader, make the Pause action raise this event.
            // 3. On that event asset, add a persistent listener that calls this component's TogglePauseScreen() method.
            
            showMainScreenEvent?.RegisterListener(ShowMainScreen);
            hideMainScreenEvent?.RegisterListener(HideMainScreen);
            showHUDScreenEvent?.RegisterListener(ShowHUDScreen);
            hideHUDScreenEvent?.RegisterListener(HideHUDScreen);
            showOptionsScreenEvent?.RegisterListener(ShowOptionsScreen);
            hideOptionsScreenEvent?.RegisterListener(HideOptionsScreen);
        }

        private void OnDisable()
        {
            showMainScreenEvent?.UnregisterListener(ShowMainScreen);
            hideMainScreenEvent?.UnregisterListener(HideMainScreen);
            showHUDScreenEvent?.UnregisterListener(ShowHUDScreen);
            hideHUDScreenEvent?.UnregisterListener(HideHUDScreen);
            showOptionsScreenEvent?.UnregisterListener(ShowOptionsScreen);
            hideOptionsScreenEvent?.UnregisterListener(HideOptionsScreen);
        }

        // This method is public so it can be called by a GameEventSO.
        public void TogglePauseScreen()
        {
            if (isGamePaused)
            {
                pauseScreen?.SetActive(false);
                hudScreen?.SetActive(true);
                Time.timeScale = 1f;
                isGamePaused = false;
                inputReader?.EnableGameplayInput();
                resumeGameEvent?.Raise();
            }
            else
            {
                pauseScreen?.SetActive(true);
                hudScreen?.SetActive(false);
                Time.timeScale = 0f;
                isGamePaused = true;
                inputReader?.EnableUIInput();
                pauseGameEvent?.Raise();
            }
        }

        // Private methods to be called by events for showing/hiding screens
        private void ShowMainScreen() => mainScreen?.SetActive(true);
        private void HideMainScreen() => mainScreen?.SetActive(false);
        private void ShowHUDScreen() => hudScreen?.SetActive(true);
        private void HideHUDScreen() => hudScreen?.SetActive(false);
        private void ShowOptionsScreen() => optionsScreen?.SetActive(true);
        private void HideOptionsScreen() => optionsScreen?.SetActive(false);
        
        /*
        HOW TO USE:
        1. Place this component on a persistent GameObject in your Startup scene.
        2. Create and assign all the UI Screen prefabs (HUD, MainScreen, etc.) to the fields in the Inspector.
        3. Assign the InputReader and all required GameEventSO assets.
        4. Other systems can now show/hide screens by raising the appropriate event (e.g., raise `showOptionsScreenEvent` to show the options).
        5. The InputReader needs to be configured to raise a specific event for the "Pause" action, which in turn calls `UISystem.TogglePauseScreen`.
        */
    }
}
