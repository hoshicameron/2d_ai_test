using PetalsOfHope.Data;
using PetalsOfHope.UI.Screens;

namespace PetalsOfHope.UI.Controllers
{
    /// <summary>
    /// Controller for the Gameplay screen. Manages the HUD and Pause panel controllers.
    /// </summary>
    public class GameplayScreenController
    {
        private readonly GameplayScreenView _view;
        private readonly HUDController _hudController;
        private readonly PausePanelController _pausePanelController;
        private readonly UIEventChannels _uiEvents;

        public GameplayScreenController(GameplayScreenView view, UIEventChannels uiEvents)
        {
            _view = view;
            _uiEvents = uiEvents;

            _hudController = new HUDController(_view.HUDPanel, _uiEvents);
            _pausePanelController = new PausePanelController(_view.PausePanel, _uiEvents);

            _uiEvents.PauseGameEvent.RegisterListener(OnPauseGame);
            _uiEvents.ResumeGameEvent.RegisterListener(OnResumeGame);
        }

        public void Terminate()
        {
            _hudController.Terminate();
            _pausePanelController.Terminate();
            _uiEvents.PauseGameEvent.UnregisterListener(OnPauseGame);
            _uiEvents.ResumeGameEvent.UnregisterListener(OnResumeGame);
        }

        private void OnPauseGame() => _view.PausePanel.Show();
        private void OnResumeGame() => _view.PausePanel.Hide();
    }
}
