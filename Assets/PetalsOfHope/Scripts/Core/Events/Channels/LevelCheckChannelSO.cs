using UnityEngine;

namespace PetalsOfHope.Core.Events.Channels
{
    /// <summary>
    /// A concrete implementation of a function channel for checking if a level is unlocked.
    /// Inherits from the generic FuncChannelSO, specifying string as input and bool as output.
    /// </summary>
    [CreateAssetMenu(menuName = "Petals of Hope/Events/Channels/Level Check Channel", fileName = "NewLevelCheckChannelSO")]
    public class LevelCheckChannelSO : FuncChannelSO<string, bool>
    {
        // This class is intentionally left empty.
        // It inherits all its functionality from the generic base class.
    }
}
