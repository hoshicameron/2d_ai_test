using UnityEditor;
using PetalsOfHope.Core.Events;
using PetalsOfHope.Core.Enums;

namespace PetalsOfHope.Editor.Events
{
    [CustomEditor(typeof(PlayUISoundEvent))]
    public class PlayUISoundEventEditor : EnumEventSOEditor<PlayUISoundEvent, UISoundType>
    {
    }
}
