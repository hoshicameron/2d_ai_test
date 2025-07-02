using System;
using PetalsOfHope.Interfaces;
using UnityEngine;

namespace PetalsOfHope.AI.Core
{
    public class AIInputSource : MonoBehaviour, ICharacterInputSource
    {
        public Action<Vector2> MoveEvent { get; set; }
        public Action JumpEvent { get; set; }
        public Action JumpCancelledEvent { get; set; }
        public Action DashEvent { get; set; }
        public Action AttackEvent { get; set; }
        public Action InteractEvent { get; set; }
        public void DisableAllInput()
        {
            
        }

        public void EnableGameplayInput()
        {
            
        }

        public void SetMoveInput(Vector2 vector2)
        {
            MoveEvent?.Invoke(vector2);
        }

        public void TriggerAttack()
        {
            AttackEvent?.Invoke();
        }

        public void TriggerJump()
        {
            JumpEvent?.Invoke();
        }
    }
}