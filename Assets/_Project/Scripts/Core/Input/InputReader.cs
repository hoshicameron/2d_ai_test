using UnityEngine;
using PetalsOfHope.Core.Events;
using UnityEngine.InputSystem;

namespace PetalsOfHope.Core.Input
{
    [CreateAssetMenu(menuName = "Petals of Hope/Input/Input Reader", fileName = "NewInputReaderSO")]
    public class InputReader : ScriptableObject, PlayerInputActions.IGameplayActions, PlayerInputActions.IUIActions
    {
        // Gameplay Events
        [Header("Gameplay Input Events")]
        public TypedEventSO<Vector2> MoveEvent;         // For continuous movement input
        public GameEventSO JumpEvent;                   // For jump pressed
        public GameEventSO JumpCancelledEvent;          // For jump released (variable jump height)
        public GameEventSO DashEvent;                   // For dash pressed
        public GameEventSO InteractEvent;               // For interact pressed

        // UI Events
        [Header("UI Input Events")]
        public TypedEventSO<Vector2> UINavigateEvent;   // For UI navigation
        public GameEventSO UISubmitEvent;               // For UI submit
        public GameEventSO UICancelEvent;               // For UI cancel

        private PlayerInputActions _playerInputActions;

        private void OnEnable()
        {
            if (_playerInputActions == null)
            {
                _playerInputActions = new PlayerInputActions();
                _playerInputActions.Gameplay.SetCallbacks(this);
                _playerInputActions.UI.SetCallbacks(this);
            }
        }


        private void OnDisable()
        {
            DisableAllInput();
        }


        public void EnableGameplayInput()
        {
            _playerInputActions.UI.Disable();
            _playerInputActions.Gameplay.Enable();
            Debug.Log("Gameplay Input Enabled");
        }


        public void EnableUIInput()
        {
            _playerInputActions.Gameplay.Disable();
            _playerInputActions.UI.Enable();
            Debug.Log("UI Input Enabled");
        }


        public void DisableAllInput()
        {
            _playerInputActions?.Gameplay.Disable();
            _playerInputActions?.UI.Disable();
            Debug.Log("All Input Disabled");
        }

        // IGameplayActions Implementation
        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent?.Raise(context.ReadValue<Vector2>());
        }


        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                JumpEvent?.Raise();
            else if (context.phase == InputActionPhase.Canceled)
                JumpCancelledEvent?.Raise();
        }


        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                DashEvent?.Raise();
        }


        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                InteractEvent?.Raise();
        }

        // IUIActions Implementation
        public void OnNavigate(InputAction.CallbackContext context)
        {
            UINavigateEvent?.Raise(context.ReadValue<Vector2>());
        }


        public void OnSubmit(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                UISubmitEvent?.Raise();
        }


        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                UICancelEvent?.Raise();
        }
    }
}
