using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI; // Or TMPro.TextMeshProUGUI

namespace PetalsOfHope.UI.HUD
{
    /// <summary>
    /// Holds references to all the UI elements that make up the HUD.
    /// This component is responsible for the direct manipulation of the UI elements.
    /// </summary>
    public class HUDView : MonoBehaviour
    {
        [Header("HUD Elements")]
        [Tooltip("Slider or Image fill to represent player health.")]
        [SerializeField] private Slider playerHealthSlider;
        [Tooltip("Optional text to display health values, e.g., '3 / 3'.")]
        [SerializeField] private Text playerHealthText;
        [Tooltip("Text to display the number of collected talismans.")]
        [SerializeField] private Text talismanCountText;

        public void UpdatePlayerHealth(int currentHealth, int maxHealth)
        {
            if (playerHealthSlider != null)
            {
                playerHealthSlider.maxValue = maxHealth;
                playerHealthSlider.value = currentHealth;
            }
            if (playerHealthText != null)
            {
                playerHealthText.text = $"{currentHealth} / {maxHealth}";
            }
        }

        public void UpdateTalismanCount(int count)
        {
            if (talismanCountText != null)
            {
                talismanCountText.text = $"Talismans: {count}";
            }
        }
    }
}
