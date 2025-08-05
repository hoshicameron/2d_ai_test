using PetalsOfHope.Data;
using PetalsOfHope.UI.Panels;

namespace PetalsOfHope.UI.Controllers
{
    /// <summary>
    /// Controller for the Pause panel. Handles button clicks and raises events.
    /// </summary>
    public class PausePanelController
    {
        private readonly PausePanelView _view;
        private readonly UIEventChannels _uiEvents;

        public PausePanelController(PausePanelView view, UIEventChannels uiEvents)
        {
            _view = view;
            _uiEvents = uiEvents;

            _view.ResumeButton.onClick.AddListener(OnResumeClicked);
            _view.OptionsButton.onClick.AddListener(OnOptionsClicked);
            _view.MainMenuButton.onClick.AddListener(OnMainMenuClicked);
        }

        public void Terminate()
        {
            _view.ResumeButton.onClick.RemoveListener(OnResumeClicked);
            _view.OptionsButton.onClick.RemoveListener(OnOptionsClicked);
            _view.MainMenuButton.onClick.RemoveListener(OnMainMenuClicked);
        }

        private void OnResumeClicked() => _uiEvents.ResumeGameEvent.Raise();
        private void OnOptionsClicked() => _uiEvents.ShowOptionsScreenEvent.Raise();
        private void OnMainMenuClicked() => _uiEvents.ShowMainMenuScreenEvent.Raise();
    }
}
