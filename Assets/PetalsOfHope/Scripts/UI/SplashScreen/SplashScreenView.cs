using UnityEngine;
using UnityEngine.UI;
using PetalsOfHope.Core.Events;

namespace PetalsOfHope.UI.SplashScreen
{
    /// <summary>
    /// Holds references to UI elements on the splash screen.
    /// For example, a loading bar or text element.
    /// </summary>
    public class SplashScreenView : MonoBehaviour
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
