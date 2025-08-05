using PetalsOfHope.UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace PetalsOfHope.UI.Screens
{
    /// <summary>
    /// Holds references to UI elements on the splash screen.
    /// For example, a loading bar or text element.
    /// </summary>
    public class SplashScreenView : ScreenView
    {
        [Header("Splash Screen Elements")]
        [Tooltip("Optional loading bar to show initialization progress.")]
        [SerializeField] private Slider loadingBar;
        
        public void SetLoadingProgress(float progress)
        {
            if (loadingBar != null)
            {
                // Clamp progress value to be safe
                loadingBar.value = Mathf.Clamp01(progress);
            }
        }
    }
}
