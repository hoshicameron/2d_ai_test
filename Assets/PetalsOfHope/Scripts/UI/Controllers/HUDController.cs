using PetalsOfHope.Data;
using PetalsOfHope.UI.Panels;

namespace PetalsOfHope.UI.Controllers
{
    /// <summary>
    /// Controller for the HUD panel. Listens to game events and updates the view.
    /// </summary>
    public class HUDController
    {
        private readonly HUDPanelView _view;
        private readonly UIEventChannels _uiEvents;

        public HUDController(HUDPanelView view, UIEventChannels uiEvents)
        {
            _view = view;
            _uiEvents = uiEvents;

            _view.PauseButton.onClick.AddListener(OnPauseClicked);
            _uiEvents.PlayerHealthChangedEvent.RegisterListener(OnHealthChanged);
            _uiEvents.CoinCountChangedEvent.RegisterListener(OnCoinCountChanged);
        }

        public void Terminate()
        {
            _view.PauseButton.onClick.RemoveListener(OnPauseClicked);
            _uiEvents.PlayerHealthChangedEvent.UnregisterListener(OnHealthChanged);
            _uiEvents.CoinCountChangedEvent.UnregisterListener(OnCoinCountChanged);
        }

        private void OnPauseClicked() => _uiEvents.PauseGameEvent.Raise();

        private void OnHealthChanged(int newHealth)
        {
            // Assuming max health is needed and can be retrieved from somewhere, e.g., a player stats SO
            // For now, let's just pass a placeholder for max health
            _view.UpdatePlayerHealth(newHealth, 100);
        }

        private void OnCoinCountChanged(int newCount)
        {
            _view.UpdateCoinCount(newCount);
        }
    }
}
