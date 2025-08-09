using PetalsOfHope.Core.Events.Channels;
using PetalsOfHope.UI.Screens;

namespace PetalsOfHope.UI.Controllers
{
    /// <summary>
    /// Controller for the Main screen. Handles button clicks and raises events.
    /// </summary>
    public class MainScreenController
    {
        private readonly MainScreenView _view;
        private readonly UIEventChannels _uiEvents;

        public MainScreenController(MainScreenView view, UIEventChannels uiEvents)
        {
            _view = view;
            _uiEvents = uiEvents;

            _view.NewGameButton.onClick.AddListener(OnNewGameClicked);
            _view.LoadGameButton.onClick.AddListener(OnLoadGameClicked);
            _view.OptionsButton.onClick.AddListener(OnOptionsClicked);
            _view.QuitButton.onClick.AddListener(OnQuitClicked);
        }

        public void Terminate()
        {
            _view.NewGameButton.onClick.RemoveListener(OnNewGameClicked);
            _view.LoadGameButton.onClick.RemoveListener(OnLoadGameClicked);
            _view.OptionsButton.onClick.RemoveListener(OnOptionsClicked);
            _view.QuitButton.onClick.RemoveListener(OnQuitClicked);
        }

        private void OnNewGameClicked() => _uiEvents.NewGameEvent.Raise();
        private void OnLoadGameClicked() => _uiEvents.LoadGameEvent.Raise();
        private void OnOptionsClicked() => _uiEvents.ShowOptionsScreenEvent.Raise();
        private void OnQuitClicked() => _uiEvents.QuitGameEvent.Raise();
    }
}
