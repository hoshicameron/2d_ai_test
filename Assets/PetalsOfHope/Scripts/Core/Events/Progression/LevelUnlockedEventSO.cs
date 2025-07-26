using UnityEngine;
using PetalsOfHope.Interfaces;

namespace PetalsOfHope.Core.Events
{
    /// <summary>
    /// Event raised when a new level is unlocked.
    /// The payload is the ISceneData interface, decoupling this event from the concrete SceneDataSO.
    /// </summary>
    [CreateAssetMenu(menuName = "Petals of Hope/Events/Progression/Level Unlocked Event")]
    public class LevelUnlockedEventSO : TypedEventSO<ISceneData>
    {
        // Intentionally empty. Inherits all logic from TypedEventSO<ISceneData>.
    }
}
