using PetalsOfHope.Core.Events.Channels;
using PetalsOfHope.UI.Screens;

namespace PetalsOfHope.UI.Controllers
{
    /// <summary>
    /// Controller for the Options screen. Handles slider changes and button clicks.
    /// </summary>
    public class OptionsScreenController
    {
        private readonly OptionsScreenView _view;
        private readonly UIEventChannels _uiEvents;

        public OptionsScreenController(OptionsScreenView view, UIEventChannels uiEvents)
        {
            _view = view;
            _uiEvents = uiEvents;

            _view.BackButton.onClick.AddListener(OnBackClicked);
            _view.MasterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            _view.BgmVolumeSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
            _view.SfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
        }

        public void Terminate()
        {
            _view.BackButton.onClick.RemoveListener(OnBackClicked);
            _view.MasterVolumeSlider.onValueChanged.RemoveListener(OnMasterVolumeChanged);
            _view.BgmVolumeSlider.onValueChanged.RemoveListener(OnBgmVolumeChanged);
            _view.SfxVolumeSlider.onValueChanged.RemoveListener(OnSfxVolumeChanged);
        }

        private void OnBackClicked() => _uiEvents.BackEvent.Raise();
        private void OnMasterVolumeChanged(float value) => _uiEvents.MasterVolumeChangedEvent.Raise(value);
        private void OnBgmVolumeChanged(float value) => _uiEvents.BgmVolumeChangedEvent.Raise(value);
        private void OnSfxVolumeChanged(float value) => _uiEvents.SfxVolumeChangedEvent.Raise(value);
    }
}
