using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PetalsOfHope.Core.Input.Test
{
    public class InputTestManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InputReader _playerInputReader = default;
        
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI inputModeText = default;
        [SerializeField] private TextMeshProUGUI moveText = default;
        [SerializeField] private TextMeshProUGUI jumpText = default;
        [SerializeField] private TextMeshProUGUI dashText = default;
        [SerializeField] private TextMeshProUGUI interactText = default;
        [SerializeField] private TextMeshProUGUI uiNavigateText = default;
        [SerializeField] private TextMeshProUGUI uiSubmitText = default;
        [SerializeField] private TextMeshProUGUI uiCancelText = default;

        private void OnEnable()
        {
            if (_playerInputReader == null)
            {
                Debug.LogError("InputReader SO not assigned to InputTestManager!");
                return;
            }

            // Subscribe to all input events
            _playerInputReader.MoveEvent.RegisterListener(OnMove);
            _playerInputReader.JumpEvent.RegisterListener(OnJump);
            _playerInputReader.JumpCancelledEvent.RegisterListener(OnJumpCancelled);
            _playerInputReader.DashEvent.RegisterListener(OnDash);
            _playerInputReader.InteractEvent.RegisterListener(OnInteract);
            _playerInputReader.UINavigateEvent.RegisterListener(OnUINavigate);
            _playerInputReader.UISubmitEvent.RegisterListener(OnUISubmit);
            _playerInputReader.UICancelEvent.RegisterListener(OnUICancel);
            
            // Start with gameplay input
            _playerInputReader.EnableGameplayInput();
            UpdateInputModeText("Gameplay");
        }

        private void OnDisable()
        {
            if (_playerInputReader == null) return;
            
            // Unsubscribe from all events
            _playerInputReader.MoveEvent.UnregisterListener(OnMove);
            _playerInputReader.JumpEvent.UnregisterListener(OnJump);
            _playerInputReader.JumpCancelledEvent.UnregisterListener(OnJumpCancelled);
            _playerInputReader.DashEvent.UnregisterListener(OnDash);
            _playerInputReader.InteractEvent.UnregisterListener(OnInteract);
            _playerInputReader.UINavigateEvent.UnregisterListener(OnUINavigate);
            _playerInputReader.UISubmitEvent.UnregisterListener(OnUISubmit);
            _playerInputReader.UICancelEvent.UnregisterListener(OnUICancel);
        }

        // Button click handlers
        public void SetGameplayInput()
        {
            _playerInputReader?.EnableGameplayInput();
            UpdateInputModeText("Gameplay");
        }

        public void SetUIInput()
        {
            _playerInputReader?.EnableUIInput();
            UpdateInputModeText("UI");
        }

        public void DisableAllInput()
        {
            _playerInputReader?.DisableAllInput();
            UpdateInputModeText("None");
        }

        // Input event handlers
        private void OnMove(Vector2 moveInput)
        {
            moveText.text = $"Move: {moveInput}";
            Debug.Log($"Move: {moveInput}");
        }

        private void OnJump()
        {
            jumpText.text = "Jump: Pressed";
            Debug.Log("Jump Pressed");
        }

        private void OnJumpCancelled()
        {
            jumpText.text = "Jump: Released";
            Debug.Log("Jump Released");
        }

        private void OnDash()
        {
            dashText.text = "Dash: Pressed";
            Debug.Log("Dash Pressed");
            // Reset after a short delay for visual feedback
            Invoke(nameof(ResetDashText), 0.5f);
        }

        private void OnInteract()
        {
            interactText.text = "Interact: Pressed";
            Debug.Log("Interact Pressed");
            // Reset after a short delay for visual feedback
            Invoke(nameof(ResetInteractText), 0.5f);
        }

        private void OnUINavigate(Vector2 navigateInput)
        {
            uiNavigateText.text = $"UI Navigate: {navigateInput}";
            Debug.Log($"UI Navigate: {navigateInput}");
        }

        private void OnUISubmit()
        {
            uiSubmitText.text = "UI Submit: Pressed";
            Debug.Log("UI Submit Pressed");
            // Reset after a short delay for visual feedback
            Invoke(nameof(ResetUISubmitText), 0.5f);
        }


        private void OnUICancel()
        {
            uiCancelText.text = "UI Cancel: Pressed";
            Debug.Log("UI Cancel Pressed");
            // Reset after a short delay for visual feedback
            Invoke(nameof(ResetUICancelText), 0.5f);
        }

        // Helper methods to reset UI text
        private void ResetDashText() => dashText.text = "Dash: ";
        private void ResetInteractText() => interactText.text = "Interact: ";
        private void ResetUISubmitText() => uiSubmitText.text = "UI Submit: ";
        private void ResetUICancelText() => uiCancelText.text = "UI Cancel: ";
        private void UpdateInputModeText(string mode) 
        {
            if (inputModeText != null)
                inputModeText.text = $"Current Input Mode: {mode}";
        }
    }
}
