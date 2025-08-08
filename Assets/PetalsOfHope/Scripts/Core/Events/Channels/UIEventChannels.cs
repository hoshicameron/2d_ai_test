using UnityEngine;

namespace PetalsOfHope.Core.Events.Channels
{
    /// <summary>
    /// A ScriptableObject that acts as a container for all UI-related event channels.
    /// This helps to declutter the constructors of UI controllers by grouping all
    /// event dependencies into a single object.
    /// </summary>
    [CreateAssetMenu(menuName = "Petals of Hope/Events/UI Event Channels")]
    public class UIEventChannels : ScriptableObject
    {
        [Header("Game State Events")]
        public GameEventSO PauseGameEvent;
        public GameEventSO ResumeGameEvent;
        public GameEventSO NewGameEvent;
        public GameEventSO LoadGameEvent;
        public GameEventSO QuitGameEvent;
        public GameEventSO BackEvent;

        [Header("Screen Navigation Events")]
        public GameEventSO ShowGameplayScreenEvent;
        public GameEventSO ShowOptionsScreenEvent;
        public GameEventSO ShowMainMenuScreenEvent;

        [Header("Player-Related Events")]
        public IntEventSO PlayerHealthChangedEvent;
        public IntEventSO CoinCountChangedEvent;

        [Header("Settings Events")]
        public FloatEventSO MasterVolumeChangedEvent;
        public FloatEventSO BgmVolumeChangedEvent;
        public FloatEventSO SfxVolumeChangedEvent;

        [Header("Loading Events")]
        public FloatEventSO LoadingProgressEvent;
    }
}
