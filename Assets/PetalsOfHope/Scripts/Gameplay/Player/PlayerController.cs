using PetalOfHope.Gameplay.Character;
using PetalsOfHope.Core.Events;
using PetalsOfHope.Core.StateMachine;
using UnityEngine;
using CoreAnimation = PetalsOfHope.Core.Animation.AnimationController;

namespace PetalsOfHope.Gameplay.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(StateMachine))]
    [RequireComponent(typeof(CoreAnimation))]
    public class PlayerController : CharacterControllerBase
    {
        [Header("Health Events")]
        [Tooltip("Listen to the player dies event.")]
        [SerializeField] private GameEventSO _playerDiedEventSO;
        [SerializeField] private GameEventSO _playerLandedEventSO;

        protected override void OnEnable()
        {
            base.OnEnable();
            _playerDiedEventSO.RegisterListener(HandleCharacterDeath);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _playerDiedEventSO.UnregisterListener(HandleCharacterDeath);
        }

        protected override void HandleCharacterLanded()
        {
            if (!wasGrounded && IsGrounded)
            {
                RemainingJumps = AbilitySheetData.jumpData.maxJumps;
                _playerLandedEventSO?.Raise();
            }
        }
    }
}
