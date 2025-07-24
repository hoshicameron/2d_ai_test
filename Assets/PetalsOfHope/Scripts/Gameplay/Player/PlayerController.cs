using PetalOfHope.Gameplay.Character;
using PetalsOfHope.Core.Events;
using UnityEngine;
using CoreAnimation = PetalsOfHope.Gameplay.Animation.AnimationController;

namespace PetalsOfHope.Gameplay.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(StateMachine.StateMachine))]
    [RequireComponent(typeof(CoreAnimation))]
    public class PlayerController : CharacterControllerBase
    {
        [Header("Health Events")]
        [Tooltip("Listen to the player dies event.")]
        [SerializeField] private GameEventSO playerDiedEventSo;
        [SerializeField] private GameEventSO playerLandedEventSo;
        
        [SerializeField] private GameEventSO onPlayerRespawnEventSo;

        protected override void OnEnable()
        {
            base.OnEnable();
            playerDiedEventSo.RegisterListener(HandleCharacterDeath);
            onPlayerRespawnEventSo.RegisterListener(HandlePlayerRespawn);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            playerDiedEventSo.UnregisterListener(HandleCharacterDeath);
            onPlayerRespawnEventSo.UnregisterListener(HandlePlayerRespawn);
        }
        
        private void HandlePlayerRespawn()
        {
            StateMachine.ChangeState(RespawnState);
        }

        protected override void HandleCharacterLanded()
        {
            if (!wasGrounded && IsGrounded)
            {
                RemainingJumps = _isDoubleJumpUnlocked ? AbilitySheetData.jumpData.maxJumps : 1;
                playerLandedEventSo?.Raise();
            }
        }
    }
}
