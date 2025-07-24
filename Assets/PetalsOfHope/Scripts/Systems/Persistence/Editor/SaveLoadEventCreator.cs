using UnityEditor;
using UnityEngine;
using System.IO;
using PetalsOfHope.Core.Events;

namespace PetalsOfHope.Editor.Systems.Events.Persistence
{
    public static class SaveLoadEventCreator
    {
        private const string MENU_PATH = "Tools/Petals of Hope/Create Save-Load Events";
        private const string TARGET_FOLDER = "Assets/_Project/ScriptableObjects/Events/Persistence";

        [MenuItem(MENU_PATH, priority = 100)]
        public static void CreateSaveLoadEvents()
        {
            // Ensure the target directory exists
            if (!AssetDatabase.IsValidFolder(TARGET_FOLDER))
            {
                string parentFolder = Path.GetDirectoryName(TARGET_FOLDER);
                string folderName = Path.GetFileName(TARGET_FOLDER);
                
                if (!AssetDatabase.IsValidFolder(parentFolder))
                {
                    Directory.CreateDirectory(Path.Combine(Application.dataPath, "../", parentFolder));
                    AssetDatabase.Refresh();
                }
                
                AssetDatabase.CreateFolder(parentFolder, folderName);
            }

            // Create each event asset
            CreateEventAsset<OnBeforeSaveGameEventSO>("OnBeforeSaveGameEvent");
            CreateEventAsset<OnAfterSaveGameEventSO>("OnAfterSaveGameEvent");
            CreateEventAsset<OnAfterLoadGameEventSO>("OnAfterLoadGameEvent");
            CreateEventAsset<OnSaveFailedEventSO>("OnSaveFailedEvent");
            CreateEventAsset<OnLoadFailedEventSO>("OnLoadFailedEvent");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log($"Successfully created save/load event assets in {TARGET_FOLDER}");
        }

        [MenuItem(MENU_PATH, true)]
        private static bool ValidateCreateSaveLoadEvents()
        {
            // Always enable the menu item
            return true;
        }

        private static void CreateEventAsset<T>(string assetName) where T : ScriptableObject
        {
            string assetPath = $"{TARGET_FOLDER}/{assetName}.asset";
            
            // Check if asset already exists
            if (AssetDatabase.LoadAssetAtPath<T>(assetPath) != null)
            {
                Debug.Log($"{assetName} already exists at {assetPath}");
                return;
            }

            // Create the asset
            T asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, assetPath);
            
            // Set description if available
            if (asset is GameEventSO gameEvent)
            {
                string description = GetEventDescription(assetName);
                if (!string.IsNullOrEmpty(description))
                {
                    // Use reflection to set the description if it's a private field
                    var fieldInfo = typeof(GameEventSO).GetField("_description", 
                        System.Reflection.BindingFlags.NonPublic | 
                        System.Reflection.BindingFlags.Instance);
                    
                    fieldInfo?.SetValue(gameEvent, description);
                    EditorUtility.SetDirty(gameEvent);
                }
            }
            
            Debug.Log($"Created {assetName} at {assetPath}");
        }

        private static string GetEventDescription(string eventName)
        {
            return eventName switch
            {
                "OnBeforeSaveGameEvent" => "Raised before the game state is saved. Use this to prepare objects for saving.",
                "OnAfterSaveGameEvent" => "Raised after the game state has been successfully saved.",
                "OnAfterLoadGameEvent" => "Raised after the game state has been loaded. Use this to initialize objects with loaded data.",
                "OnSaveFailedEvent" => "Raised when saving the game fails.",
                "OnLoadFailedEvent" => "Raised when loading the game fails.",
                _ => string.Empty
            };
        }
    }
}
