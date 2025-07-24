using UnityEngine;
using PetalsOfHope.Interfaces;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PetalsOfHope.Data.Levels
{
    public enum SceneType { System, Menu, Level, Other }

    [CreateAssetMenu(menuName = "Petals of Hope/Scene Management/Scene Data", fileName = "NewSceneDataSO")]
    public class SceneDataSO : ScriptableObject, ISceneData
    {
        [Header("Scene Information")]
        [Tooltip("A friendly display name for the scene (e.g., for UI).")]
        public string DisplayName;
        
        [Tooltip("Type of scene (e.g., Menu, Level).")]
        public SceneType SceneType = SceneType.Level;

        [Tooltip("The name of the scene file (e.g., 'Level01'). Must be in Build Settings.")]
        public string sceneName;

#if UNITY_EDITOR
        [Tooltip("Drag the scene asset here from the Project window. Ensures scene exists.")]
        [SerializeField] private SceneAsset sceneAsset = null;
#endif
        
        [Header("Loading Screen (Optional)")]
        [Tooltip("Custom loading screen background for this scene (if applicable).")]
        public Sprite LoadingScreenBackground;
        [TextArea]
        [Tooltip("Tip or flavor text to display while loading this scene.")]
        public string LoadingTip;

        public string GetSceneName()
        {
#if UNITY_EDITOR
            if (sceneAsset != null)
            {
                return sceneAsset.name;
            }
#endif
            if (!string.IsNullOrEmpty(sceneName))
            {
                return sceneName;
            }
            Debug.LogWarning($"Scene name not properly configured for SceneDataSO: {name}", this);
            return string.Empty;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (sceneAsset != null)
            {
                string assetName = sceneAsset.name;
                if (sceneName != assetName)
                {
                    sceneName = assetName;
                    EditorUtility.SetDirty(this);
                }
            }
        }
#endif
    }
}
