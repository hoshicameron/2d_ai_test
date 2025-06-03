# Task ID: 5.2.1
# Parent Task ID: 5.2
# Title: Implement AudioManager
# Status: pending
# Dependencies: 1.1.1 (Project Setup for AudioMixer asset)
# Priority: critical
# Estimated Effort: L
# Assignee: Unassigned

# Description:
Implement `AudioManager.cs` as a Singleton or service. It will manage SFX and BGM playback, use pooled Audio Sources for SFX, and interface with Unity's `AudioMixer` for volume control.

# Details:
1.  **Create AudioMixer Asset:**
    *   In Project window: `Create > Audio > Audio Mixer`. Name it `MainAudioMixer`.
    *   Location: `Assets/_Project/Settings/Audio/MainAudioMixer.mixer`
    *   Open the Audio Mixer window (`Window > Audio > Audio Mixer`).
    *   Select `MainAudioMixer`.
    *   Create Groups:
        *   `Master` (usually exists by default).
        *   Right-click `Master` -> `Add child group` -> `BGM`.
        *   Right-click `Master` -> `Add child group` -> `SFX`.
    *   Expose Volume Parameters for each group:
        *   Select `Master` group. In Inspector, right-click "Volume" -> "Expose 'Volume (of Master)' to script". Rename exposed parameter to `MasterVolumeParam` (as used in Task 5.1.5).
        *   Repeat for `BGM` group (expose as `BGMVolumeParam`).
        *   Repeat for `SFX` group (expose as `SFXVolumeParam`).

2.  **Implement `AudioManager.cs`:**
    *   File Location: `Assets/_Project/Scripts/Audio/AudioManager.cs`
    *   Namespace: `PetalsOfHope.Audio`
    *   Implementation:
        ```csharp
        // In Assets/_Project/Scripts/Audio/AudioManager.cs
        namespace PetalsOfHope.Audio
        {
            using UnityEngine;
            using UnityEngine.Audio; // For AudioMixer
            using System.Collections.Generic; // For pooling
            // using PetalsOfHope.Core.Events; // If listening to AudioEventSO directly

            public class AudioManager : MonoBehaviour // Singleton
            {
                public static AudioManager Instance { get; private set; }

                [Header("Audio Mixer & Groups")]
                [SerializeField] private AudioMixer _mainAudioMixer;
                [SerializeField] private AudioMixerGroup _masterGroup;
                [SerializeField] private AudioMixerGroup _bgmGroup;
                [SerializeField] private AudioMixerGroup _sfxGroup;

                [Header("Audio Sources")]
                [Tooltip("AudioSource for playing BGM. Should loop.")]
                [SerializeField] private AudioSource _bgmSource;
                // SFX sources will be pooled

                [Header("SFX Pooling (Basic)")]
                [SerializeField] private GameObject _sfxAudioSourcePrefab; // Prefab with just an AudioSource component
                [SerializeField] private int _sfxPoolSize = 10;
                private List<AudioSource> _sfxSourcePool = new List<AudioSource>();
                private int _sfxPoolNextIndex = 0;
                
                // PlayerPrefs keys from OptionsMenuController
                private const string MASTER_VOLUME_KEY = "MasterVolume";
                private const string BGM_VOLUME_KEY = "BGMVolume";
                private const string SFX_VOLUME_KEY = "SFXVolume";
                
                private const string MIXER_MASTER_VOL = "MasterVolumeParam";
                private const string MIXER_BGM_VOL = "BGMVolumeParam";
                private const string MIXER_SFX_VOL = "SFXVolumeParam";


                private void Awake()
                {
                    if (Instance == null)
                    {
                        Instance = this;
                        DontDestroyOnLoad(gameObject);
                        InitializeSfxPool();
                        LoadVolumesFromPrefs(); // Load volumes on awake
                    }
                    else
                    {
                        Destroy(gameObject);
                        return;
                    }

                    if (_bgmSource == null) Debug.LogError("BGM AudioSource not assigned to AudioManager!", this);
                    if (_mainAudioMixer == null) Debug.LogError("MainAudioMixer not assigned!", this);
                    if (_sfxAudioSourcePrefab == null) Debug.LogWarning("SFXAudioSourcePrefab not assigned. SFX pooling might not work.", this);

                }

                private void InitializeSfxPool()
                {
                    if (_sfxAudioSourcePrefab == null) return;
                    for (int i = 0; i < _sfxPoolSize; i++)
                    {
                        GameObject sfxSourceObj = Instantiate(_sfxAudioSourcePrefab, transform); // Parent to AudioManager
                        sfxSourceObj.name = $"SFXSource_{i}";
                        AudioSource source = sfxSourceObj.GetComponent<AudioSource>();
                        if (source != null)
                        {
                            source.outputAudioMixerGroup = _sfxGroup; // Assign to SFX mixer group
                            source.playOnAwake = false;
                            _sfxSourcePool.Add(source);
                        }
                        sfxSourceObj.SetActive(false); // Keep inactive until used
                    }
                }

                private AudioSource GetPooledSfxSource()
                {
                    if (_sfxSourcePool.Count == 0) return null; // Or create one dynamically if pool is empty

                    for (int i = 0; i < _sfxSourcePool.Count; i++)
                    {
                        _sfxPoolNextIndex = (_sfxPoolNextIndex + 1) % _sfxSourcePool.Count;
                        if (!_sfxSourcePool[_sfxPoolNextIndex].isPlaying)
                        {
                            _sfxSourcePool[_sfxPoolNextIndex].gameObject.SetActive(true);
                            return _sfxSourcePool[_sfxPoolNextIndex];
                        }
                    }
                    // If all sources are playing, either expand pool or return null/log warning
                    Debug.LogWarning("SFX Pool: All sources are busy. Consider increasing pool size.");
                    // Optionally create a temporary one:
                    // GameObject tempSfxObj = Instantiate(_sfxAudioSourcePrefab, transform);
                    // AudioSource tempSource = tempSfxObj.GetComponent<AudioSource>();
                    // tempSource.outputAudioMixerGroup = _sfxGroup;
                    // tempSource.playOnAwake = false;
                    // Destroy(tempSfxObj, clipDuration + 0.1f); // Self-destruct after playing
                    // return tempSource;
                    return null;
                }

                public void PlaySFX(AudioClip clip, float volume = 1f, float pitch = 1f)
                {
                    if (clip == null) return;
                    AudioSource source = GetPooledSfxSource();
                    if (source != null)
                    {
                        source.clip = clip;
                        source.volume = volume;
                        source.pitch = pitch;
                        source.Play();
                        // For non-looping SFX, can disable object after duration if not using smart pooling
                        // StartCoroutine(ReturnSourceToPool(source, clip.length));
                    }
                }

                public void PlaySFX(AudioEventSO audioEvent) // Task 5.2.2 will define AudioEventSO
                {
                    if (audioEvent == null) return;
                    // Logic to pick a clip, set volume/pitch from AudioEventSO
                    // For now, placeholder:
                    if (audioEvent.clips != null && audioEvent.clips.Length > 0)
                    {
                        AudioClip clipToPlay = audioEvent.clips[Random.Range(0, audioEvent.clips.Length)];
                        PlaySFX(clipToPlay, audioEvent.volume, audioEvent.pitch);
                        // Note: AudioEventSO might specify its own mixer group, overriding the default SFX group.
                        // AudioSource source = GetPooledSfxSource(); ... source.outputAudioMixerGroup = audioEvent.audioMixerGroup ?? _sfxGroup;
                    }
                }

                public void PlayBGM(AudioClip clip, bool loop = true, float volume = 0.5f)
                {
                    if (_bgmSource == null || clip == null) return;
                    _bgmSource.clip = clip;
                    _bgmSource.loop = loop;
                    _bgmSource.volume = volume; // BGM volume is also controlled by mixer group
                    _bgmSource.outputAudioMixerGroup = _bgmGroup;
                    _bgmSource.Play();
                }
                
                public void PlayBGM(AudioEventSO audioEvent)
                {
                     if (audioEvent == null || _bgmSource == null) return;
                     if (audioEvent.clips != null && audioEvent.clips.Length > 0)
                     {
                        // BGM usually has one clip, but AudioEventSO supports multiple for variation if desired.
                        AudioClip clipToPlay = audioEvent.clips[0]; 
                        PlayBGM(clipToPlay, true, audioEvent.volume);
                        _bgmSource.outputAudioMixerGroup = audioEvent.audioMixerGroup ?? _bgmGroup;
                     }
                }

                public void StopBGM()
                {
                    _bgmSource?.Stop();
                }

                // --- Volume Control Methods (Called by OptionsMenuController) ---
                public void SetVolume(string volumeTypeKey, float normalizedVolume) // normalizedVolume 0-1
                {
                    if (_mainAudioMixer == null) return;
                    float decibelVolume = NormalizedToDecibel(normalizedVolume);
                    
                    string mixerParam = "";
                    switch (volumeTypeKey) {
                        case MASTER_VOLUME_KEY: mixerParam = MIXER_MASTER_VOL; break;
                        case BGM_VOLUME_KEY: mixerParam = MIXER_BGM_VOL; break;
                        case SFX_VOLUME_KEY: mixerParam = MIXER_SFX_VOL; break;
                        default: Debug.LogWarning($"Unknown volume type key: {volumeTypeKey}"); return;
                    }

                    _mainAudioMixer.SetFloat(mixerParam, decibelVolume);
                    PlayerPrefs.SetFloat(volumeTypeKey, normalizedVolume); // Save normalized volume
                    PlayerPrefs.Save();
                }

                public float GetVolume(string volumeTypeKey) // Returns normalized volume 0-1
                {
                    return PlayerPrefs.GetFloat(volumeTypeKey, 0.8f); // Default to 0.8 (80%)
                }

                private void LoadVolumesFromPrefs()
                {
                    SetVolume(MASTER_VOLUME_KEY, GetVolume(MASTER_VOLUME_KEY));
                    SetVolume(BGM_VOLUME_KEY, GetVolume(BGM_VOLUME_KEY));
                    SetVolume(SFX_VOLUME_KEY, GetVolume(SFX_VOLUME_KEY));
                }
                
                public static float NormalizedToDecibel(float normalizedLevel)
                {
                    if (normalizedLevel <= 0.0001f) return -80f; // Min decibels for mute
                    return Mathf.Log10(normalizedLevel) * 20f;
                }
            }
        }
        ```
    *   `SFXAudioSourcePrefab` should be a prefab containing only an `AudioSource` component.

# Acceptance Criteria:
- `AudioManager.cs` (as a Singleton MonoBehaviour) is implemented.
- `MainAudioMixer` asset is created with `Master`, `BGM`, `SFX` groups, and their volumes exposed as parameters (e.g., `MasterVolumeParam`).
- `AudioManager` has fields for `_mainAudioMixer` and group references.
- It manages a pool of `AudioSource`s for SFX playback (basic implementation).
- `PlaySFX(AudioClip/AudioEventSO)` method plays a sound effect using a pooled source.
- `PlayBGM(AudioClip/AudioEventSO)` method plays background music on a dedicated `_bgmSource`.
- `SetVolume(string volumeTypeKey, float normalizedVolume)` method adjusts the corresponding `AudioMixer` group volume and saves the normalized (0-1) value to `PlayerPrefs`.
- `GetVolume(string volumeTypeKey)` retrieves normalized volume from `PlayerPrefs`.
- `LoadVolumesFromPrefs()` is called on `Awake` to apply saved settings.

# Test Strategy:
- Manual Testing:
    - Create `AudioManager` GameObject in scene. Assign `MainAudioMixer`, BGM source, SFX prefab.
    - Test `PlaySFX(clip)` and `PlayBGM(clip)` from another script. Verify sounds play.
    - Test `PlaySFX(AudioEventSO)` and `PlayBGM(AudioEventSO)` (once `AudioEventSO` is defined in Task 5.2.2).
    - Integrate with `OptionsMenuController` (Task 5.1.5):
        - Adjust volume sliders in Options Menu.
        - Verify `AudioManager.SetVolume` is called and `AudioMixer` group volumes change in editor.
        - Verify sound output levels change.
        - Verify settings are saved to `PlayerPrefs` and loaded correctly on game restart.
    - Test SFX pooling: play multiple SFX simultaneously or in rapid succession. Check for errors or sound cutoff.

# Notes/Questions:
- The SFX pooling implementation is basic. More advanced pooling might involve returning sources to the pool when finished playing.
- `AudioEventSO` (Task 5.2.2) will provide more data-driven sound playback.
- `NormalizedToDecibel` conversion is crucial for setting mixer volumes correctly from linear sliders.
- `PlayerPrefs` is used for saving volume settings, as is common.