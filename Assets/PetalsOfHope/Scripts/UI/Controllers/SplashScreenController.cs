using PetalsOfHope.Data;
using PetalsOfHope.UI.Screens;

namespace PetalsOfHope.UI.Controllers
{
    /// <summary>
    /// Controller for the Splash screen. Handles loading progress and transitions.
    /// </summary>
    public class SplashScreenController
    {
        private readonly SplashScreenView _view;
        private readonly UIEventChannels _uiEvents;

        public SplashScreenController(SplashScreenView view, UIEventChannels uiEvents)
        {
            _view = view;
            _uiEvents = uiEvents;

            _uiEvents.LoadingProgressEvent.RegisterListener(OnLoadingProgress);
        }

        public void Terminate()
        {
            _uiEvents.LoadingProgressEvent.UnregisterListener(OnLoadingProgress);
        }

        private void OnLoadingProgress(float progress)
        {
            _view.SetLoadingProgress(progress);
        }
    }
}
