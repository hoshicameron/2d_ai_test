using PetalsOfHope.Core.Events;
using UnityEngine;

namespace PetalsOfHope.Data
{
    /// <summary>
    /// A ScriptableObject that acts as a container for all UI-related event channels.
    /// This helps to declutter the constructors of UI controllers by grouping all
    /// event dependencies into a single object.
    /// </summary>
    [CreateAssetMenu(menuName = "Petals of Hope/Data/UI Event Channels")]
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
        public GameEventSO ShowOptionsScreenEvent;
        public GameEventSO ShowMainMenuScreenEvent;
        public GameEventSO ShowGameplayScreenEvent;

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
