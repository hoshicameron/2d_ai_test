using UnityEditor;
using PetalsOfHope.Core.Events;
using PetalsOfHope.Contracts;

namespace PetalsOfHope.Editor.Events
{
    [CustomEditor(typeof(PlaySFXEvent))]
    public class PlaySFXEventEditor : EnumEventSOEditor<PlaySFXEvent, SFXType>
    {
    }
}
