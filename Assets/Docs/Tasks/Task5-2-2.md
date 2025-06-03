# Task ID: 5.2.2
# Parent Task ID: 5.2
# Title: Implement AudioEventSO
# Status: pending
# Dependencies: 1.1.2 (Folder Structure)
# Priority: high
# Estimated Effort: S
# Assignee: Unassigned

# Description:
Implement `AudioEventSO.cs`, a ScriptableObject that defines properties for a sound event. This includes an array of `AudioClip`s for variations, volume, pitch, and target `AudioMixerGroup`.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/Data/Audio/Events/AudioEventSO.cs` (Create `Audio/Events` under `Data`)
2.  **Namespace:** `PetalsOfHope.Data.Audio.Events`
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/Data/Audio/Events/AudioEventSO.cs
    namespace PetalsOfHope.Data.Audio.Events
    {
        using UnityEngine;
        using UnityEngine.Audio; // For AudioMixerGroup

        [CreateAssetMenu(menuName = "Petals of Hope/Audio/Audio Event", fileName = "NewAudioEventSO")]
        public class AudioEventSO : ScriptableObject
        {
            [Header("Sound Properties")]
            [Tooltip("Audio clips to choose from. One will be picked randomly if multiple are provided.")]
            public AudioClip[] clips;

            [Range(0f, 1f)]
            [Tooltip("Base volume for this sound event.")]
            public float volume = 1f;

            [Range(0.1f, 3f)]
            [Tooltip("Base pitch for this sound event.")]
            public float pitch = 1f;
            
            [Tooltip("Optional: Specific AudioMixerGroup for this event. If null, AudioManager's default (SFX/BGM) group is used.")]
            public AudioMixerGroup audioMixerGroup; // Optional override

            [Header("Randomization (Optional)")]
            [Range(0f, 1f)]
            [Tooltip("Random variation +/- for volume (e.g., 0.1 means volume can vary by +/- 10%).")]
            public float volumeVariation = 0f;
            
            [Range(0f, 1f)]
            [Tooltip("Random variation +/- for pitch (e.g., 0.1 means pitch can vary by +/- 10% of base pitch).")]
            public float pitchVariation = 0f;


            // Method to be called by AudioManager to play this event
            // AudioManager will handle getting a source and applying these properties.
            // This SO itself doesn't play, it just holds data.
            public void Play(AudioSource source) // AudioManager passes one of its pooled sources
            {
                if (clips == null || clips.Length == 0 || source == null)
                {
                    // Debug.LogWarning($"AudioEventSO {name}: No clips assigned or source is null.", this);
                    return;
                }

                AudioClip clipToPlay = clips[Random.Range(0, clips.Length)];
                if (clipToPlay == null) {
                    // Debug.LogWarning($"AudioEventSO {name}: Selected clip is null.", this);
                    return;
                }

                source.clip = clipToPlay;
                source.volume = volume * (1f + Random.Range(-volumeVariation, volumeVariation));
                source.pitch = pitch * (1f + Random.Range(-pitchVariation, pitchVariation));
                
                if (audioMixerGroup != null)
                {
                    source.outputAudioMixerGroup = audioMixerGroup;
                }
                // else AudioManager will use its default SFX/BGM group for the source.
                
                source.Play();
            }
            
            // Overload for AudioManager to directly call, if AudioManager handles source acquisition.
            // This is what PlaySFX(AudioEventSO) in AudioManager does.
            // The 'Play(AudioSource source)' method above is more for a pattern where the SO itself
            // is given a source and told to configure and play on it.
            // For AudioManager integration, it's simpler if AudioManager reads properties from this SO.
        }
    }
    ```

# Acceptance Criteria:
- `AudioEventSO.cs` ScriptableObject is created.
- It includes fields for:
    - `AudioClip[] clips`
    - `volume` (float, Range 0-1)
    - `pitch` (float, Range 0.1-3)
    - `audioMixerGroup` (AudioMixerGroup, optional)
    - Optional `volumeVariation` and `pitchVariation` fields.
- `[CreateAssetMenu]` attribute allows easy creation of audio event assets.
- (Conceptual) A `Play(AudioSource source)` method demonstrates how properties could be applied to an AudioSource (though `AudioManager` will likely read properties directly).
- Script compiles without errors.

# Test Strategy:
- Manual Verification:
    - Create an `AudioEventSO` asset in the Project window (e.g., `Assets/_Project/ScriptableObjects/AudioEvents/PlayerJumpAudioEvent.asset`).
    - Assign one or more `AudioClip`s, set volume, pitch, and optionally assign an `AudioMixerGroup`.
    - Test with `AudioManager.PlaySFX(audioEventSO)` or `AudioManager.PlayBGM(audioEventSO)`.
    - Verify a random clip plays (if multiple provided).
    - Verify volume and pitch settings (and variations if implemented) are applied.
    - Verify sound plays on the correct `AudioMixerGroup` if specified, otherwise AudioManager's default.

# Notes/Questions:
- The `Play(AudioSource source)` method within `AudioEventSO` is one pattern. Another is for `AudioManager` to simply read the public fields of the `AudioEventSO` and configure the `AudioSource` itself, which is what was implemented in `AudioManager.PlaySFX(AudioEventSO)`. This is cleaner. The `Play` method in `AudioEventSO` can be removed or kept as a utility.
- Randomization of volume/pitch adds nice variation to sounds.
- `AudioEventSO` makes managing sounds much more flexible and data-driven.