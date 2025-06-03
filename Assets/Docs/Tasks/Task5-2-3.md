# Task ID: 5.2.3
# Parent Task ID: 5.2
# Title: Trigger Sounds from Gameplay and UI
# Status: pending
# Dependencies: 5.2.1, 5.2.2, various gameplay/UI components
# Priority: high
# Estimated Effort: L (spread across many small integrations)
# Assignee: Unassigned

# Description:
Integrate audio playback into gameplay components (`PlayerController`, `EnemyBase`, etc.) and UI elements by calling `AudioManager` methods or by raising audio events that `AudioManager` (or dedicated listeners) can respond to. Implement BGM transitions between scenes/states.

# Details:
1.  **Identify Key Sound Trigger Points:**
    *   **Player Actions:** Jump, Land, Double Jump, Dash, Wall Jump, Attack (if any), Hurt, Death, Footsteps (optional).
    *   **Enemy Actions:** Attack, Hurt, Death, Detection, Special Abilities.
    *   **UI Interactions:** Button clicks, Menu open/close, Item select/collect.
    *   **Environment:** Doors, breakable objects, ambient loops.
    *   **Game Events:** Level Start, Level Complete, Talisman Awarded.
2.  **Create `AudioEventSO` Assets for these Sounds:**
    *   For each identified sound, create a corresponding `AudioEventSO` asset (Task 5.2.2).
    *   Configure clips, volume, pitch, variations. Example: `PlayerJump_AudioEventSO`, `ButtonClick_AudioEventSO`.
3.  **Call `AudioManager` from Scripts:**
    *   In the relevant C# scripts, get a reference to `AudioManager.Instance`.
    *   Call `AudioManager.Instance.PlaySFX(correspondingAudioEventSO)` at the appropriate trigger point.
    *   Example in `PlayerController` or player states (e.g., `JumpingState.Enter()`):
        ```csharp
        // public AudioEventSO jumpSound; // Assign in Inspector for PlayerController/State
        // AudioManager.Instance.PlaySFX(jumpSound);
        ```
    *   Example in UI button script (`OnClick()`):
        ```csharp
        // public AudioEventSO buttonClickSound; // Assign in Inspector
        // AudioManager.Instance.PlaySFX(buttonClickSound);
        ```
4.  **BGM Management and Transitions:**
    *   `SceneLoader` (Task 4.2.1) or a `LevelManager` can be responsible for BGM.
    *   When a new scene loads (or level starts), it can call `AudioManager.Instance.PlayBGM(levelSpecificBgmAudioEventSO)`.
    *   `LevelSettingsSO` (Task 1.3.3) can hold a reference to the `AudioEventSO` for that level's BGM.
    *   For smooth transitions:
        *   `AudioManager` could have `FadeOutBGM(float duration)` and `FadeInBGM(AudioEventSO newBgm, float duration)` methods.
        *   `SceneLoader` would call `AudioManager.Instance.FadeOutBGM()` before starting async scene load, and `AudioManager.Instance.FadeInBGM()` after new scene is loaded.

5.  **Alternative: Event-Driven Audio:**
    *   Instead of direct `AudioManager` calls, gameplay events (e.g., `PlayerJumpedEventSO` from Task 1.2.8) could be listened to by dedicated `AudioEventListener` components or by `AudioManager` itself.
    *   `AudioEventListener`: A MonoBehaviour that listens to a `GameEventSO` / `TypedEventSO` and plays an `AudioEventSO` in response.
        ```csharp
        // Example AudioEventListener.cs
        // public GameEventSO triggerEvent;
        // public AudioEventSO soundToPlay;
        // void OnEnable() { triggerEvent.RegisterListener(PlaySound); }
        // void OnDisable() { triggerEvent.UnregisterListener(PlaySound); }
        // void PlaySound() { AudioManager.Instance.PlaySFX(soundToPlay); }
        ```
    *   This decouples audio playback from game logic scripts further.

# Acceptance Criteria:
- Key player actions (jump, land, damage, death) trigger appropriate SFX via `AudioManager`.
- Key enemy actions (attack, damage, death) trigger SFX.
- Basic UI interactions (button clicks) trigger SFX.
- Background music plays and can change when transitioning between scenes (or game states).
- (If implemented) BGM transitions are smooth (fade out/in).
- (If event-driven audio is used) `AudioEventListener` components correctly trigger sounds based on game events.
- Sounds use `AudioEventSO` assets for configuration.

# Test Strategy:
- Manual Playtesting:
    - Play through game levels and interact with various elements.
    - Listen carefully for all implemented sounds. Verify they trigger at the correct times.
    - Test BGM changes during scene transitions.
    - Test UI sound feedback.
    - Check console for any audio-related errors (e.g., missing clips, null references).

# Notes/Questions:
- A mix of direct `AudioManager` calls and event-driven audio can be used. Direct calls are simpler for tightly coupled sounds (e.g., player jump sound within jump logic). Event-driven is better for decoupled sounds (e.g., UI click sound based on a general "UIButtonClickedEvent").
- Footstep sounds often require more complex logic (checking ground type, animation events from walk/run cycles). This might be a separate, more detailed task if complex footsteps are needed.
- Implementing BGM fade transitions in `AudioManager` would involve coroutines similar to the `SceneLoader` fade.