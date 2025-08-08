using PetalsOfHope.UI.Base;
using PetalsOfHope.UI.Panels;
using UnityEngine;

namespace PetalsOfHope.UI.Screens
{
    /// <summary>
    /// Represents the main gameplay screen, which contains the HUD and Pause panels.
    /// </summary>
    public class GameplayScreenView : ScreenView
    {
        [Header("Panels")]
        [SerializeField] private HUDPanelView hudPanel;
        [SerializeField] private PausePanelView pausePanel;

        public HUDPanelView HUDPanel => hudPanel;
        public PausePanelView PausePanel => pausePanel;

        public override void Initialize()
        {
            base.Initialize();
            // By default, the pause panel should be hidden.
            pausePanel.Hide();
        }
    }
}
