using UnityEngine;

namespace PetalsOfHope.Core.Events.Channels
{
    /// <summary>
    /// A ScriptableObject that acts as a container for all Audio-related event channels.
    /// </summary>
    [CreateAssetMenu(menuName = "Petals of Hope/Events/Audio Event Channels")]
    public class AudioEventChannels : ScriptableObject
    {
        [Header("Audio Request Events")]
        public PlayMusicEvent PlayMusicEvent;
        public PlaySFXEvent PlaySFXEvent;
        public PlayUISoundEvent PlayUISoundEvent;

        [Header("Volume Control Events")]
        public FloatEventSO MasterVolumeChangedEvent;
        public FloatEventSO BgmVolumeChangedEvent;
        public FloatEventSO SfxVolumeChangedEvent;
    }
}
