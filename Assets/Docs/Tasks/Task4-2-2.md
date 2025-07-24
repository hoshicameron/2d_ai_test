# Task ID: 4.2.2
# Parent Task ID: 4.2
# Title: Create SceneDataSO for Scene Metadata
# Status: completed
# Dependencies: 1.1.2 # Folder structure
# Priority: medium
# Estimated Effort: S
# Assignee: Unassigned

# Description:
Create `SceneDataSO.cs`, a ScriptableObject to hold metadata for scenes, such as their build index or path, and a display name. This allows for easier management and referencing of scenes.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/Systems/SceneManagement/SceneDataSO.cs`
2.  **Namespace:** `PetalsOfHope.Systems.SceneManagement`
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/Systems/SceneManagement/SceneDataSO.cs
    namespace PetalsOfHope.Systems.SceneManagement
    {
        using UnityEngine;
        #if UNITY_EDITOR
        using UnityEditor; // For SceneAsset
        #endif

        public enum SceneType { System, Menu, Level, Other }

        [CreateAssetMenu(menuName = "Petals of Hope/Scene Management/Scene Data", fileName = "NewSceneDataSO")]
        public class SceneDataSO : ScriptableObject
        {
            [Header("Scene Information")]
            [Tooltip("A friendly display name for the scene (e.g., for UI).")]
            public string displayName;
            
            [Tooltip("Type of scene (e.g., Menu, Level).")]
            public SceneType sceneType = SceneType.Level;

            // Option 1: Store scene name (requires exact match and scene in build settings)
            [Tooltip("The name of the scene file (e.g., 'Level01'). Must be in Build Settings.")]
            public string sceneName;

            // Option 2: Use SceneAsset for editor-time safety (converts to name/path at build or runtime)
            #if UNITY_EDITOR
            [Tooltip("Drag the scene asset here from the Project window. Ensures scene exists.")]
            [SerializeField] private SceneAsset _sceneAsset = null;
            #endif
            
            [Header("Loading Screen (Optional)")]
            [Tooltip("Custom loading screen background for this scene (if applicable).")]
            public Sprite loadingScreenBackground;
            [TextArea]
            [Tooltip("Tip or flavor text to display while loading this scene.")]
            public string loadingTip;


            // Method to get the scene name reliably, using SceneAsset if available in Editor
            public string GetSceneName()
            {
                #if UNITY_EDITOR
                if (_sceneAsset != null)
                {
                    return _sceneAsset.name; // In editor, SceneAsset.name gives the scene name
                }
                #endif
                // Fallback or runtime: use the explicitly set sceneName field
                if (!string.IsNullOrEmpty(sceneName))
                {
                    return sceneName;
                }
                Debug.LogWarning($"Scene name not properly configured for SceneDataSO: {name}", this);
                return string.Empty;
            }

            #if UNITY_EDITOR
            // Editor-only logic to keep sceneName field synchronized with _sceneAsset if used
            private void OnValidate()
            {
                if (_sceneAsset != null)
                {
                    // Check if the name of the asset (which is the scene filename without extension)
                    // is different from the stored sceneName.
                    string assetName = _sceneAsset.name;
                    if (sceneName != assetName)
                    {
                        sceneName = assetName;
                        EditorUtility.SetDirty(this); // Mark as dirty to save change
                    }
                }
                else if (!string.IsNullOrEmpty(sceneName))
                {
                    // If sceneName is set but _sceneAsset is not, try to find it
                    // This is more complex and might not be necessary if workflow enforces SceneAsset usage.
                    // string[] scenePaths = AssetDatabase.FindAssets($"t:SceneAsset {sceneName}");
                    // if (scenePaths.Length > 0) {
                    //     _sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(AssetDatabase.GUIDToAssetPath(scenePaths[0]));
                    // }
                }
            }
            #endif
        }
    }
    ```

# Acceptance Criteria:
- `SceneDataSO.cs` ScriptableObject is created.
- It includes fields for `displayName` (string), `sceneType` (enum), `sceneName` (string).
- (Editor-only) It includes a `SceneAsset` field (`_sceneAsset`) for easier and safer scene referencing in the editor.
- `GetSceneName()` method reliably returns the scene name to be loaded.
- (Editor-only) `OnValidate()` attempts to synchronize `sceneName` with `_sceneAsset.name`.
- Optional fields for loading screen customization (`loadingScreenBackground`, `loadingTip`) are present.
- Script compiles without errors.

# Test Strategy:
- Manual Verification:
    - Create a `SceneDataSO` asset in the Project window (e.g., `Assets/_Project/ScriptableObjects/SceneData/Level1_SceneDataSO.asset`).
    *   Assign a `SceneAsset` to its `_sceneAsset` field in the Inspector. Verify `sceneName` field updates.
    *   Set `displayName`, `sceneType`.
    - Create a test script that takes a `SceneDataSO` as a field, calls `GetSceneName()`, and then uses that name with `SceneLoader.Instance.LoadScene()`.
    - Verify the correct scene is loaded.

# Notes/Questions:
- Using `SceneAsset` in the editor is highly recommended as it prevents errors from typos in scene names and ensures the referenced scene exists. `sceneName` string is used as a fallback or for runtime.
- The `OnValidate` logic helps keep `sceneName` in sync with the `SceneAsset` when changes are made in the editor.
- `SceneType` enum can be useful for organizing scenes or applying different logic based on type (e.g., different loading procedures).