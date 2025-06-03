# Task ID: 5.1.2
# Parent Task ID: 5.1
# Title: Implement HUDController and View
# Status: pending
# Dependencies: 5.1.1, 2.5 (PlayerHealth), 4.3.2 (InventorySystem), 1.2 (EventBus)
# Priority: high
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement `HUDController.cs` and its associated UGUI view. The HUD should display essential game information like player health and talisman count, updating dynamically from game events or by polling relevant systems.

# Details:
1.  **Create `HUDView.cs` (UGUI specific, handles UI element references):**
    *   File Location: `Assets/_Project/Scripts/UI/HUD/HUDView.cs`
    *   Namespace: `PetalsOfHope.UI.HUD`
        ```csharp
        // In Assets/_Project/Scripts/UI/HUD/HUDView.cs
        namespace PetalsOfHope.UI.HUD
        {
            using UnityEngine;
            using UnityEngine.UI; // Or TMPro.TextMeshProUGUI

            public class HUDView : MonoBehaviour
            {
                [Header("HUD Elements")]
                public Slider playerHealthSlider; // Or Image for fill amount
                public Text playerHealthText;     // Optional: text display of health
                public Text talismanCountText;    // Displays number of collected talismans
                // Add other HUD elements as needed (e.g., score, timer, current ability icons)

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
        ```
2.  **Create `HUDController.cs` (Handles logic and event listening):**
    *   File Location: `Assets/_Project/Scripts/UI/HUD/HUDController.cs`
    *   Namespace: `PetalsOfHope.UI.HUD`
        ```csharp
        // In Assets/_Project/Scripts/UI/HUD/HUDController.cs
        namespace PetalsOfHope.UI.HUD
        {
            using UnityEngine;
            using PetalsOfHope.Core.Events;       // For EventSO listeners
            using PetalsOfHope.Data.Collectibles; // For TalismanDataSO
            using PetalsOfHope.Systems.Inventory; // For InventorySystem and TalismanDataSOEvent
            // using PetalsOfHope.Gameplay.Player; // For PlayerHealth (if direct polling)

            [RequireComponent(typeof(HUDView))]
            public class HUDController : MonoBehaviour
            {
                private HUDView _view;
                // Assuming PlayerHealthChangedEventSO is IntEventSO payload: current health
                // And MaxHealth is fetched from PlayerStatsSO or PlayerHealth component directly.
                [Header("Event Listeners")]
                [SerializeField] private IntEventSO _playerHealthChangedEventSO; 
                [SerializeField] private TalismanDataSOEvent _talismanAwardedEventSO; // To update count when one is added
                // Optional: An event for when inventory is fully loaded/changed if count needs batch update

                // Direct references (alternative to events, or for initial setup)
                private PetalsOfHope.Gameplay.Player.PlayerHealth _playerHealth; 
                private InventorySystem _inventorySystem;

                private void Awake()
                {
                    _view = GetComponent<HUDView>();
                }

                private void Start()
                {
                    // Find player health and inventory system for initial values or polling if events are not comprehensive
                    GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                    if (playerObj != null) _playerHealth = playerObj.GetComponent<PetalsOfHope.Gameplay.Player.PlayerHealth>();
                    
                    _inventorySystem = InventorySystem.Instance;

                    // Initial UI update
                    if (_playerHealth != null) 
                        _view.UpdatePlayerHealth(_playerHealth.CurrentHealth, _playerHealth.MaxHealth);
                    if (_inventorySystem != null)
                        _view.UpdateTalismanCount(_inventorySystem.GetCollectedTalismans().Count);
                }

                private void OnEnable()
                {
                    _playerHealthChangedEventSO?.RegisterListener(OnPlayerHealthChanged);
                    _talismanAwardedEventSO?.RegisterListener(OnTalismanAwarded);
                }

                private void OnDisable()
                {
                    _playerHealthChangedEventSO?.UnregisterListener(OnPlayerHealthChanged);
                    _talismanAwardedEventSO?.UnregisterListener(OnTalismanAwarded);
                }

                private void OnPlayerHealthChanged(int currentHealth)
                {
                    // We need max health too. PlayerHealthChangedEventSO might need to be TypedEventSO<HealthData>
                    // Or, PlayerHealth component can be queried for MaxHealth.
                    if (_playerHealth != null) { // Assuming _playerHealth reference is valid
                         _view.UpdatePlayerHealth(currentHealth, _playerHealth.MaxHealth);
                    } else {
                        // Fallback if _playerHealth ref is null, requires a fixed or default max health
                        // Or the event payload must include max health.
                        // For now, let's assume _playerHealth reference is obtained in Start.
                        Debug.LogWarning("PlayerHealth reference missing in HUDController, cannot update health with max value.");
                        // _view.UpdatePlayerHealth(currentHealth, 100); // Example with default max
                    }
                }

                private void OnTalismanAwarded(TalismanDataSO talisman)
                {
                    // When a talisman is awarded, InventorySystem's count changes.
                    // Re-query InventorySystem for the new total count.
                    if (_inventorySystem != null)
                    {
                        _view.UpdateTalismanCount(_inventorySystem.GetCollectedTalismans().Count);
                    }
                }
            }
        }
        ```
3.  **Create HUD Panel Prefab (UGUI):**
    *   Location: `Assets/_Project/Prefabs/UI/HUDPanel.prefab`
    *   Create a Canvas GameObject, or use an existing persistent UI Canvas.
    *   Add a Panel GameObject as a child, name it `HUDPanel`.
    *   Inside `HUDPanel`, create UI elements:
        *   `PlayerHealthSlider` (UI Slider).
        *   `PlayerHealthText` (UI Text or TextMeshProUGUI).
        *   `TalismanCountText` (UI Text or TextMeshProUGUI).
    *   Anchor these elements appropriately (e.g., health top-left, talismans top-right).
    *   Attach `HUDView.cs` to the `HUDPanel` GameObject. Drag the UI elements to its fields.
    *   Attach `HUDController.cs` to the `HUDPanel` GameObject. Assign necessary EventSO assets.
4.  **Integrate with `UIManager`:**
    *   Ensure the `HUDPanel.prefab` is assigned to `UIManager._hudPanel` field.
    *   `UIManager` will control its visibility (e.g., show during gameplay, hide during main menu/pause).

# Acceptance Criteria:
- `HUDView.cs` and `HUDController.cs` are implemented.
- `HUDPanel.prefab` (UGUI) is created with UI elements for player health and talisman count.
- `HUDController` listens to `_playerHealthChangedEventSO` and updates health display.
- `HUDController` listens to `_talismanAwardedEventSO` (or polls `InventorySystem`) and updates talisman count display.
- HUD is correctly shown/hidden by `UIManager`.
- HUD elements update dynamically and correctly during gameplay.

# Test Strategy:
- Manual Testing:
    - Ensure `HUDPanel.prefab` is part of your scene setup, managed by `UIManager`.
    - Start game: verify initial health and talisman count are displayed.
    - Player takes damage: verify health display (slider/text) updates.
    - Player collects a talisman: verify talisman count updates.
    - Pause game: verify HUD is hidden (if `UIManager` implements this). Resume: verify HUD reappears.

# Notes/Questions:
- `_playerHealthChangedEventSO` currently has `int currentHealth`. For a slider or text like "50/100", `maxHealth` is also needed. The event payload could be a struct `HealthData { int current, int max; }`, or `HUDController` can cache/fetch `maxHealth` from `PlayerHealth` component. The current implementation tries to fetch from `_playerHealth` reference.
- This task assumes UGUI. If UI Toolkit is used, `HUDView` would be replaced by UXML for structure, USS for styling, and `HUDController` would query/manipulate UI Toolkit elements.