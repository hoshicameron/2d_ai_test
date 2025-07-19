using UnityEngine;
using System;

namespace PetalsOfHope.Core.Events.Channels
{
    [CreateAssetMenu(menuName = "Petals of Hope/Events/Channels/Ability Check Channel")]
    public class AbilityCheckChannelSO : ScriptableObject
    {
        /// <summary>
        /// The function that systems can subscribe to.
        /// It takes a string (e.g., a talisman ID) and returns a bool (e.g., is it unlocked?).
        /// </summary>
        public Func<string, bool> IsUnlocked;
        
        /*
        HOW TO USE:

        PROVIDER (e.g., GameProgressionSystem):
        1. Have a [SerializeField] for this channel asset.
        2. In Awake() or OnEnable(), assign a method to the Func.
           e.g., abilityCheckChannel.IsUnlocked = HasTalisman;
        3. In OnDisable(), clear the Func to prevent memory leaks.
           e.g., abilityCheckChannel.IsUnlocked = null;

        REQUESTER (e.g., PlayerController):
        1. Have a [SerializeField] for this same channel asset.
        2. To check for an ability, call the Func. Use the null-conditional operator (?.) for safety.
           e.g., bool canDoubleJump = abilityCheckChannel.IsUnlocked?.Invoke("TALISMAN_DOUBLE_JUMP") ?? false;
           The '?? false' provides a default value if no provider has assigned the function yet.
        */
    }
}
