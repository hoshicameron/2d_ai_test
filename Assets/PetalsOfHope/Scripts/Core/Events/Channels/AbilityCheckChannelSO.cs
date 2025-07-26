using UnityEngine;

namespace PetalsOfHope.Core.Events.Channels
{
    /// <summary>
    /// A concrete implementation of a function channel for checking if an ability is unlocked.
    /// Inherits from the generic FuncChannelSO, specifying string as input and bool as output.
    /// </summary>
    [CreateAssetMenu(menuName = "Petals of Hope/Events/Channels/Ability Check Channel")]
    public class AbilityCheckChannelSO : FuncChannelSO<string, bool>
    {
        /*
        HOW TO USE:

        PROVIDER (e.g., GameProgressionSystem):
        1. Have a [SerializeField] for this channel asset.
        2. In Awake() or OnEnable(), assign a method to the Func.
           e.g., abilityCheckChannel.Function = HasTalisman;
        3. In OnDisable(), clear the Func to prevent memory leaks.
           e.g., abilityCheckChannel.Function = null;

        REQUESTER (e.g., PlayerController):
        1. Have a [SerializeField] for this same channel asset.
        2. To check for an ability, call the Func. Use the null-conditional operator (?.) for safety.
           e.g., bool canDoubleJump = abilityCheckChannel.Function?.Invoke("TALISMAN_DOUBLE_JUMP") ?? false;
           The '?? false' provides a default value if no provider has assigned the function yet.
        */
    }
}
