using UnityEngine;
using UnityEngine.UI;
using PetalsOfHope.Interfaces;
using PetalsOfHope.Core.Events;
using System.Collections;

namespace PetalsOfHope.UI
{
    public class SimpleNotifier : MonoBehaviour
    {
        [SerializeField] private CollectibleEventSO onCollectibleAwarded;
        [SerializeField] private Text notificationText;
        [SerializeField] private float displayDuration = 3f;

        private Coroutine _activeNotificationCoroutine;

        private void OnEnable()
        {
            if (onCollectibleAwarded != null) 
                onCollectibleAwarded.RegisterListener(OnCollectibleAwarded);
            else 
                Debug.LogError("CollectibleEventSO not assigned to SimpleNotifier.", this);

            if (notificationText != null) 
                notificationText.gameObject.SetActive(false);
            else
                Debug.LogError("NotificationText not assigned to SimpleNotifier.", this);
        }

        private void OnDisable()
        {
            if (onCollectibleAwarded != null) 
                onCollectibleAwarded.UnregisterListener(OnCollectibleAwarded);
        }

        private void OnCollectibleAwarded(ICollectible collectible)
        {
            if (notificationText == null)
            {
                Debug.Log($"[NOTIFICATION] Collectible Acquired: {collectible.DisplayName} - {collectible.Description}");
                return;
            }

            if (_activeNotificationCoroutine != null)
            {
                StopCoroutine(_activeNotificationCoroutine);
            }
            _activeNotificationCoroutine = StartCoroutine(ShowNotificationRoutine(collectible));
        }

        private System.Collections.IEnumerator ShowNotificationRoutine(ICollectible collectible)
        {
            notificationText.text = $"Acquired!\n{collectible.DisplayName}";
            notificationText.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(displayDuration);
            
            notificationText.gameObject.SetActive(false);
            _activeNotificationCoroutine = null;
        }
        
        /*
        HOW TO USE:
        1. Place this component on a GameObject in your scene (e.g., a "UIManager").
        2. Create a UI Text element on a Canvas in your scene.
        3. Assign the UI Text element to the "Notification Text" field of this component.
        4. Assign the "TalismanAwardedEvent" ScriptableObject asset to the "On Talisman Awarded" field.
        5. This component will now listen for the event and display a notification when a talisman is collected.
        */
    }
}
