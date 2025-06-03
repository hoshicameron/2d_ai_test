# Task ID: 4.3.3
# Parent Task ID: 4.3
# Title: Implement Basic UI Notification for Talisman Award
# Status: pending
# Dependencies: 4.3.2, 5.1 (UIManager, full UI is Phase 5)
# Priority: medium
# Estimated Effort: S (for basic notification), M (if integrating with placeholder UIManager)
# Assignee: Unassigned

# Description:
Implement a basic UI notification when a talisman is awarded. This will involve listening to `TalismanAwardedEventSO`. The full UI implementation is in Phase 5, so this can be a placeholder (e.g., debug log, or a very simple on-screen text).

# Details:
1.  **Create a Placeholder Notification System (if `UIManager` from Phase 5 isn't started):**
    *   This could be a simple script that displays text on a UI Text element.
    *   File: `Assets/_Project/Scripts/UI/SimpleNotifier.cs` (temporary location)
    *   Namespace: `PetalsOfHope.UI`
        ```csharp
        // In Assets/_Project/Scripts/UI/SimpleNotifier.cs
        namespace PetalsOfHope.UI
        {
            using UnityEngine;
            using UnityEngine.UI; // For basic Text, or TMPro.TextMeshProUGUI
            using PetalsOfHope.Data.Collectibles;
            using PetalsOfHope.Systems.Inventory; // For TalismanDataSOEvent

            public class SimpleNotifier : MonoBehaviour
            {
                [SerializeField] private TalismanDataSOEvent _talismanAwardedEventSO; // Listen to this
                [SerializeField] private Text _notificationText; // Assign a UI Text element
                [SerializeField] private float _displayDuration = 3f;

                private Coroutine _activeNotificationCoroutine;

                private void OnEnable()
                {
                    if (_talismanAwardedEventSO != null) 
                        _talismanAwardedEventSO.RegisterListener(OnTalismanAwarded);
                    else 
                        Debug.LogError("TalismanAwardedEventSO not assigned to SimpleNotifier.", this);

                    if (_notificationText != null) 
                        _notificationText.gameObject.SetActive(false);
                    else
                        Debug.LogError("NotificationText not assigned to SimpleNotifier.", this);
                }

                private void OnDisable()
                {
                    if (_talismanAwardedEventSO != null) 
                        _talismanAwardedEventSO.UnregisterListener(OnTalismanAwarded);
                }

                private void OnTalismanAwarded(TalismanDataSO talisman)
                {
                    if (_notificationText == null)
                    {
                        Debug.Log($"[NOTIFICATION] Talisman Awarded: {talisman.displayName} - {talisman.description}");
                        return;
                    }

                    if (_activeNotificationCoroutine != null)
                    {
                        StopCoroutine(_activeNotificationCoroutine);
                    }
                    _activeNotificationCoroutine = StartCoroutine(ShowNotificationRoutine(talisman));
                }

                private System.Collections.IEnumerator ShowNotificationRoutine(TalismanDataSO talisman)
                {
                    _notificationText.text = $"Talisman Acquired!\n{talisman.displayName}";
                    _notificationText.gameObject.SetActive(true);
                    
                    yield return new WaitForSeconds(_displayDuration);
                    
                    _notificationText.gameObject.SetActive(false);
                    _activeNotificationCoroutine = null;
                }
            }
        }
        ```
2.  **Setup UI in Scene:**
    *   Create a Canvas in your test scene.
    *   Add a UI Text element (or TextMeshProUGUI) to the Canvas for notifications. Anchor it appropriately (e.g., top-center of screen).
    *   Create an empty GameObject (e.g., `_NotificationManager`). Attach `SimpleNotifier.cs` to it.
    *   Assign the `TalismanAwardedEventSO` (from Task 4.3.2) and the UI Text element to the `SimpleNotifier` script in the Inspector.
3.  **Integration with `UIManager` (Phase 5):**
    *   When `UIManager.cs` (Task 5.1) is implemented, it will take over this notification role.
    *   `UIManager` will listen to `TalismanAwardedEventSO` and display a more polished notification panel/popup.
    *   This `SimpleNotifier` is a temporary stand-in if Phase 5 is not yet reached.

# Acceptance Criteria:
- A system (either `SimpleNotifier` or a debug log fallback) listens to `TalismanAwardedEventSO`.
- When a talisman is awarded via `InventorySystem.AddTalisman()`, the listener reacts.
- A basic notification is displayed (e.g., text on screen for a few seconds, or a `Debug.Log` message detailing the awarded talisman).

# Test Strategy:
- Manual Testing:
    - In a test scene with the `SimpleNotifier` (or `UIManager` if available) set up.
    - Trigger `InventorySystem.Instance.AddTalisman()` for a test talisman.
    - Verify the on-screen notification text appears with the talisman's name and then disappears after `_displayDuration`.
    - If no UI, verify `Debug.Log` output.

# Notes/Questions:
- This task fulfills the "UI Notification" part of the Talisman Award System from the plan for Phase 4, with the understanding that a more robust UI implementation will come in Phase 5.
- The `SimpleNotifier` script uses standard Unity UI Text. If TextMeshPro is preferred, replace `UnityEngine.UI.Text` with `TMPro.TextMeshProUGUI`.