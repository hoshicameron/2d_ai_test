// In Assets/_Project/Scripts/Core/Events/Base/BaseEventSO.cs
namespace PetalsOfHope.Core.Events.Base
{
    using UnityEngine;

    /// <summary>
    /// Base class for all ScriptableObject-based events.
    /// Provides a description field for editor documentation.
    /// </summary>
    public abstract class BaseEventSO : ScriptableObject
    {
        [TextArea(3, 10)]
        [SerializeField]
        private string _developerDescription = "";
    }
}
