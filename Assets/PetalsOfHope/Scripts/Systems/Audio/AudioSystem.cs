using System.Collections.Generic;
using PetalsOfHope.Core.Enums;
using PetalsOfHope.Core.Events.Channels;
using PetalsOfHope.Data.Audio;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace PetalsOfHope.Systems
{
    public class AudioSystem : MonoBehaviour
    {
        [Header("Event Channels")]
        [SerializeField] private AudioEventChannels audioEvents;

        [Header("Data")]
        [SerializeField] private SoundDatabase soundDatabase;

        [Header("Audio Mixer & Groups")]
        [SerializeField] private AudioMixer mainAudioMixer;
        [SerializeField] private AudioMixerGroup bgmGroup;
        [SerializeField] private AudioMixerGroup sfxGroup;

        
        [Header("Audio Sources")]
        [SerializeField] private AudioSource bgmSource;
        [SerializeField] private AudioSource sfxSourcePrefab;
        [SerializeField] private int sfxPoolSize = 10;

        private List<AudioSource> _sfxSourcePool;
        private int _sfxPoolNextIndex = 0;

        private const string MIXER_MASTER_VOL = "MasterVolumeParam";
        private const string MIXER_BGM_VOL = "BGMVolumeParam";
        private const string MIXER_SFX_VOL = "SFXVolumeParam";

        private void OnEnable()
        {
            InitializeSfxPool();
            SubscribeToEvents();
        }

        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }

        private void InitializeSfxPool()
        {
            _sfxSourcePool = new List<AudioSource>();
            if (sfxSourcePrefab == null) return;

            for (int i = 0; i < sfxPoolSize; i++)
            {
                var source = Instantiate(sfxSourcePrefab, transform);
                source.outputAudioMixerGroup = sfxGroup;
                source.playOnAwake = false;
                _sfxSourcePool.Add(source);
            }
        }

        private void SubscribeToEvents()
        {
            if (audioEvents == null) return;

            audioEvents.PlayMusicEvent.RegisterListener(OnPlayMusic);
            audioEvents.PlaySFXEvent.RegisterListener(OnPlaySFX);
            audioEvents.PlayUISoundEvent.RegisterListener(OnPlayUISound);
            audioEvents.MasterVolumeChangedEvent.RegisterListener(OnMasterVolumeChanged);
            audioEvents.BgmVolumeChangedEvent.RegisterListener(OnBgmVolumeChanged);
            audioEvents.SfxVolumeChangedEvent.RegisterListener(OnSfxVolumeChanged);
        }

        private void UnsubscribeFromEvents()
        {
            if (audioEvents == null) return;

            audioEvents.PlayMusicEvent.UnregisterListener(OnPlayMusic);
            audioEvents.PlaySFXEvent.UnregisterListener(OnPlaySFX);
            audioEvents.PlayUISoundEvent.UnregisterListener(OnPlayUISound);
            audioEvents.MasterVolumeChangedEvent.UnregisterListener(OnMasterVolumeChanged);
            audioEvents.BgmVolumeChangedEvent.UnregisterListener(OnBgmVolumeChanged);
            audioEvents.SfxVolumeChangedEvent.UnregisterListener(OnSfxVolumeChanged);
        }

        private void OnPlayMusic(MusicType type)
        {
            var clip = soundDatabase.GetMusicClip(type);
            if (clip == null) return;

            bgmSource.clip = clip;
            bgmSource.outputAudioMixerGroup = bgmGroup;
            bgmSource.Play();
        }

        private void OnPlaySFX(SFXType type)
        {
            var clip = soundDatabase.GetSFXClip(type);
            PlayOneShot(clip, sfxGroup);
        }

        private void OnPlayUISound(UISoundType type)
        {
            var clip = soundDatabase.GetUISoundClip(type);
            PlayOneShot(clip, sfxGroup);
        }

        private void PlayOneShot(AudioClip clip, AudioMixerGroup group)
        {
            if (clip == null) return;

            var source = GetPooledSfxSource();
            if (source != null)
            {
                source.outputAudioMixerGroup = group;
                source.PlayOneShot(clip);
            }
        }

        private AudioSource GetPooledSfxSource()
        {
            if (_sfxSourcePool.Count == 0) return null;

            for (int i = 0; i < _sfxSourcePool.Count; i++)
            {
                _sfxPoolNextIndex = (_sfxPoolNextIndex + 1) % _sfxSourcePool.Count;
                if (!_sfxSourcePool[_sfxPoolNextIndex].isPlaying)
                {
                    return _sfxSourcePool[_sfxPoolNextIndex];
                }
            }
            return null;
        }

        private void OnMasterVolumeChanged(float volume) => SetVolume(MIXER_MASTER_VOL, volume);
        private void OnBgmVolumeChanged(float volume) => SetVolume(MIXER_BGM_VOL, volume);
        private void OnSfxVolumeChanged(float volume) => SetVolume(MIXER_SFX_VOL, volume);

        private void SetVolume(string parameter, float normalizedVolume)
        {
            var decibelVolume = normalizedVolume > 0.0001f ? Mathf.Log10(normalizedVolume) * 20f : -80f;
            mainAudioMixer.SetFloat(parameter, decibelVolume);
        }
    }
}
