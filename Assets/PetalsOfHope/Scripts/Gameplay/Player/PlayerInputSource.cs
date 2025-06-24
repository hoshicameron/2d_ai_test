using System;
using PetalsOfHope.Core.Input;
using PetalsOfHope.Interfaces;
using UnityEngine;

namespace PetalsOfHope.Gameplay.Player
{
    public class PlayerInputSource : MonoBehaviour, ICharacterInputSource
    {
        [SerializeField] private InputReader _inputReader;

        public Action<Vector2> MoveEvent { get; set;}
        public Action JumpEvent { get; set;}
        public Action JumpCancelledEvent { get; set;}
        public Action DashEvent { get; set;}
        public Action AttackEvent { get; set;}
        public Action InteractEvent { get; set;}
        
        public void DisableAllInput() => _inputReader.DisableAllInput();
        public void EnableGameplayInput() => _inputReader.EnableGameplayInput();

        private void OnEnable()
        {
            if (_inputReader != null)
            {
                _inputReader.MoveEvent.RegisterListener(HandleMove);
                _inputReader.JumpEvent.RegisterListener(HandleJumpPressed);
                _inputReader.JumpCancelledEvent.RegisterListener(HandleJumpReleased);
                _inputReader.DashEvent.RegisterListener(HandleDash);
                _inputReader.AttackEvent.RegisterListener(HandleAttackPressed);
                _inputReader.InteractEvent.RegisterListener(HandleInteract);
            }
        }

        private void OnDisable()
        {
            if (_inputReader != null)
            {
                _inputReader.MoveEvent.UnregisterListener(HandleMove);
                _inputReader.JumpEvent.UnregisterListener(HandleJumpPressed);
                _inputReader.JumpCancelledEvent.UnregisterListener(HandleJumpReleased);
                _inputReader.DashEvent.UnregisterListener(HandleDash);
            }
        }

        private void HandleMove(Vector2 move) => MoveEvent?.Invoke(move);
        private void HandleJumpPressed() => JumpEvent?.Invoke();
        private void HandleJumpReleased() => JumpCancelledEvent?.Invoke();
        private void HandleDash() => DashEvent?.Invoke();
        private void HandleInteract() => InteractEvent?.Invoke();
        private void HandleAttackPressed() => AttackEvent?.Invoke();
        
    }
}