using UnityEngine;
using UnityEditor;
using PetalsOfHope.Core.Events;

#if UNITY_EDITOR
namespace PetalsOfHope.Core.Editor
{
    public static class CreateInputEvents
    {
        private const string MENU_PATH = "Assets/Create/Petals of Hope/Input Events/";
        private const string DEFAULT_PATH = "Assets/_Project/ScriptableObjects/Events/Input/";

        [MenuItem(MENU_PATH + "Gameplay Events", false, 10)]
        public static void CreateGameplayEvents()
        {
            // Create directory if it doesn't exist
            if (!AssetDatabase.IsValidFolder("Assets/_Project/ScriptableObjects/Events/Input"))
            {
                AssetDatabase.CreateFolder("Assets/_Project/ScriptableObjects/Events", "Input");
            }

            // Gameplay Events
            CreateEvent<GameEventSO>("PlayerJumpPressedEvent");
            CreateEvent<GameEventSO>("PlayerJumpCancelledEvent");
            CreateEvent<GameEventSO>("PlayerDashPressedEvent");
            CreateEvent<GameEventSO>("PlayerInteractPressedEvent");
            CreateEvent<TypedEventSO<Vector2>>("PlayerMoveInputEvent");

            // UI Events
            CreateEvent<GameEventSO>("UISubmitPressedEvent");
            CreateEvent<GameEventSO>("UICancelPressedEvent");
            CreateEvent<TypedEventSO<Vector2>>("UINavigateInputEvent");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreateEvent<T>(string name) where T : ScriptableObject
        {
            string path = $"{DEFAULT_PATH}{name}.asset";
            
            if (AssetDatabase.LoadAssetAtPath<T>(path) == null)
            {
                T asset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, path);
                Debug.Log($"Created {name} at {path}");
            }
            else
            {
                Debug.Log($"{name} already exists at {path}");
            }
        }
    }
}
#endif
