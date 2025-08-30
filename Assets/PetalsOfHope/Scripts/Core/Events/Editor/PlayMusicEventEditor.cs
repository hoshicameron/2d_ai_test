using UnityEditor;
using PetalsOfHope.Core.Events;
using PetalsOfHope.Contracts;

namespace PetalsOfHope.Editor.Events
{
    [CustomEditor(typeof(PlayMusicEvent))]
    public class PlayMusicEventEditor : EnumEventSOEditor<PlayMusicEvent, MusicType>
    {
    }
}
