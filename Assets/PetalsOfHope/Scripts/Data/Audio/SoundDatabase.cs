using System;
using System.Collections.Generic;
using System.Linq;
using PetalsOfHope.Core.Enums;
using UnityEngine;

namespace PetalsOfHope.Data.Audio
{
    [CreateAssetMenu(fileName = "SoundDatabase", menuName = "Petals of Hope/Audio/Sound Database")]
    public class SoundDatabase : ScriptableObject
    {
        [Header("Music Tracks")]
        public List<MusicEntry> musicTracks;

        [Header("Sound Effects")]
        public List<SFXEntry> soundEffects;

        [Header("UI Sounds")]
        public List<UISoundEntry> uiSounds;

        private Dictionary<MusicType, AudioClip> _musicDictionary;
        private Dictionary<SFXType, AudioClip> _sfxDictionary;
        private Dictionary<UISoundType, AudioClip> _uiSoundDictionary;

        private void OnEnable()
        {
            _musicDictionary = musicTracks.ToDictionary(x => x.type, x => x.clip);
            _sfxDictionary = soundEffects.ToDictionary(x => x.type, x => x.clip);
            _uiSoundDictionary = uiSounds.ToDictionary(x => x.type, x => x.clip);
        }

        public AudioClip GetMusicClip(MusicType type) => _musicDictionary.GetValueOrDefault(type);
        public AudioClip GetSFXClip(SFXType type) => _sfxDictionary.GetValueOrDefault(type);
        public AudioClip GetUISoundClip(UISoundType type) => _uiSoundDictionary.GetValueOrDefault(type);
    }

    [Serializable]
    public struct MusicEntry
    {
        public MusicType type;
        public AudioClip clip;
    }

    [Serializable]
    public struct SFXEntry
    {
        public SFXType type;
        public AudioClip clip;
    }

    [Serializable]
    public struct UISoundEntry
    {
        public UISoundType type;
        public AudioClip clip;
    }
}
