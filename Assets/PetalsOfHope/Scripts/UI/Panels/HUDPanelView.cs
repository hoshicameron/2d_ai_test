using PetalsOfHope.UI.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PetalsOfHope.UI.Panels
{
    /// <summary>
    /// Holds references to all the UI elements that make up the HUD.
    /// This component is responsible for the direct manipulation of the UI elements.
    /// </summary>
    public class HUDPanelView : PanelView
    {
        [Header("HUD Elements")]
        [Tooltip("Slider or Image fill to represent player health.")]
        [SerializeField] private Slider playerHealthSlider;
        [Tooltip("The Image component of the slider's Fill Area.")]
        [SerializeField] private Image playerHealthFillImage;
        [Tooltip("Gradient to determine the health bar color based on health percentage.")]
        [SerializeField] private Gradient healthGradient;
        [Tooltip("Optional text to display health values, e.g., '3 / 3'.")]
        [SerializeField] private TextMeshProUGUI playerHealthText;
        [Tooltip("Text to display the number of collected coins.")]
        [SerializeField] private TextMeshProUGUI coinCountText;
        [Tooltip("Button to pause the game.")]
        [SerializeField] private Button pauseButton;

        public Button PauseButton => pauseButton;

        public void UpdatePlayerHealth(int currentHealth, int maxHealth)
        {
            if (playerHealthSlider != null)
            {
                playerHealthSlider.maxValue = maxHealth;
                playerHealthSlider.value = currentHealth;

                if (playerHealthFillImage != null && healthGradient != null)
                {
                    // Calculate health percentage
                    float healthPercent = (float)currentHealth / maxHealth;
                    playerHealthFillImage.color = healthGradient.Evaluate(healthPercent);
                }
            }
            if (playerHealthText != null)
            {
                playerHealthText.text = $"{currentHealth} / {maxHealth}";
            }
        }

        public void UpdateCoinCount(int count)
        {
            if (coinCountText != null)
            {
                coinCountText.text = $"Coins: {count}";
            }
        }
    }
}
