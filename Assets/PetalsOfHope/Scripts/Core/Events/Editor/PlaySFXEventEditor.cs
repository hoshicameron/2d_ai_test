using UnityEditor;
using PetalsOfHope.Core.Events;
using PetalsOfHope.Core.Enums;

namespace PetalsOfHope.Editor.Events
{
    [CustomEditor(typeof(PlaySFXEvent))]
    public class PlaySFXEventEditor : EnumEventSOEditor<PlaySFXEvent, SFXType>
    {
    }
}
