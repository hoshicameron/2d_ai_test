// In Assets/_Project/Editor/Events/EventAssetCreator.cs

using PetalsOfHope.Core.Events.Base;

namespace PetalsOfHope.Editor.Events
{
    using UnityEditor;
    using UnityEngine;
    using PetalsOfHope.Core.Events;
    using System.IO;

    public static class EventAssetCreator
    {
        private const string EventsPath = "Assets/_Project/ScriptableObjects/Events";

        [MenuItem("Petals of Hope/Create Event Assets")]
        public static void CreateAllEventAssets()
        {
            // Ensure the directory exists
            if (!Directory.Exists(EventsPath))
            {
                Directory.CreateDirectory(EventsPath);
                AssetDatabase.Refresh();
            }

            // Create parameterless events
            CreateGameEvent("PlayerJumpedEventSO", "Raised when the player jumps.");
            CreateGameEvent("PlayerLandedEventSO", "Raised when the player lands on the ground after a jump or fall.");
            CreateGameEvent("PlayerDiedEventSO", "Raised when the player's health reaches zero.");
            CreateGameEvent("EnemyDiedEventSO", "Raised when an enemy is defeated.");
            CreateGameEvent("PauseGameEventSO", "Raised when the game is paused.");
            CreateGameEvent("ResumeGameEventSO", "Raised when the game is resumed from pause.");
            CreateGameEvent("LevelStartedEventSO", "Raised when a new level starts.");
            CreateGameEvent("LevelCompletedEventSO", "Raised when the current level is completed.");

            // Create typed events
            CreateTypedEvent<IntEventSO>("PlayerHealthChangedEventSO", 
                "Raised when the player's health changes. Payload: current health value.");
                
            CreateTypedEvent<IntEventSO>("ScoreUpdatedEventSO", 
                "Raised when the player's score changes. Payload: new score value.");
                
            CreateTypedEvent<IntEventSO>("DamageDealtEventSO", 
                "Raised when damage is dealt. Payload: amount of damage.");
                
            CreateTypedEvent<Vector2EventSO>("PlayerMoveInputEventSO", 
                "Raised when the player provides move input. Payload: normalized move direction.");
                
            CreateTypedEvent<Vector2EventSO>("CameraShakeRequestedEventSO", 
                "Raised to request a camera shake effect. Payload: x = intensity, y = duration.");
                
            CreateTypedEvent<StringEventSO>("ShowNotificationEventSO", 
                "Raised to show a notification to the player. Payload: message to display.");
                
            CreateTypedEvent<StringEventSO>("SceneLoadRequestedEventSO", 
                "Raised to request a scene load. Payload: name of the scene to load.");
                
            CreateTypedEvent<GameObjectEventSO>("EnemySpawnedEventSO", 
                "Raised when a new enemy is spawned. Payload: the spawned enemy GameObject.");
                
            CreateTypedEvent<GameObjectEventSO>("ItemCollectedEventSO", 
                "Raised when the player collects an item. Payload: the collected item GameObject.");
                
            CreateTypedEvent<BoolEventSO>("ToggleSettingEventSO", 
                "Raised when a game setting is toggled. Payload: new state of the setting (true/false).");

            AssetDatabase.Refresh();
            Debug.Log("All event assets have been created successfully!");
        }

        private static void CreateGameEvent(string assetName, string description)
        {
            var asset = ScriptableObject.CreateInstance<GameEventSO>();
            SetDescription(asset, description);
            SaveAsset(asset, assetName);
        }

        private static void CreateTypedEvent<T>(string assetName, string description) where T : BaseEventSO
        {
            var asset = ScriptableObject.CreateInstance<T>();
            SetDescription(asset, description);
            SaveAsset(asset, assetName);
        }

        private static void SetDescription(BaseEventSO asset, string description)
        {
            // Use reflection to set the private _developerDescription field
            var field = typeof(BaseEventSO).GetField("_developerDescription", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field.SetValue(asset, description);
        }

        private static void SaveAsset(UnityEngine.Object asset, string assetName)
        {
            string path = Path.Combine(EventsPath, assetName + ".asset");
            AssetDatabase.CreateAsset(asset, path);
        }
    }
}
