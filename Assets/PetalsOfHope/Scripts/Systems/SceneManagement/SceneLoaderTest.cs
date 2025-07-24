using UnityEngine;
using PetalsOfHope.Core.Events;
using PetalsOfHope.Data.Levels;
using PetalsOfHope.Interfaces;
using UnityEngine.Serialization;

namespace PetalsOfHope.Systems.SceneManagement
{
    /// <summary>
    /// A simple component for testing the SceneLoader system by raising events from the Inspector.
    /// </summary>
    public class SceneLoaderTest : MonoBehaviour
    {
        [Header("Event Raisers")]
        [Tooltip("The channel to request a specific scene load.")]
        [SerializeField] private LoadSceneRequestEventSO loadSceneRequestChannel;
        
        [Tooltip("The channel to request a reload of the current scene.")]
        [SerializeField] private GameEventSO reloadCurrentSceneChannel;

        [FormerlySerializedAs("_sceneToLoad")]
        [Header("Test Data")]
        [Tooltip("The SceneDataSO for the scene you want to load.")]
        [SerializeField] private SceneDataSO sceneToLoad;
        
        

        [ContextMenu("Test Load Specific Scene")]
        public void TestLoadScene()
        {
            if (loadSceneRequestChannel == null)
            {
                Debug.LogError("Load Scene Request Channel is not assigned.", this);
                return;
            }
            if (sceneToLoad == null)
            {
                Debug.LogError("Scene To Load is not assigned.", this);
                return;
            }
            
            Debug.Log($"Raising request to load scene: {sceneToLoad.GetSceneName()}");
            loadSceneRequestChannel.Raise(sceneToLoad);
        }

        [ContextMenu("Test Reload Current Scene")]
        public void TestReloadScene()
        {
            if (reloadCurrentSceneChannel == null)
            {
                Debug.LogError("Reload Current Scene Channel is not assigned.", this);
                return;
            }
            
            Debug.Log("Raising request to reload the current scene.");
            reloadCurrentSceneChannel.Raise();
        }
        
        /*
        HOW TO USE:
        1. Add this component to any GameObject in your test scene.
        2. Create and assign the "Load Scene Request Channel" and "Reload Current Scene Channel" ScriptableObject assets.
        3. Create and assign a "SceneDataSO" asset for the scene you wish to test loading (e.g., "Level2_SceneData").
        4. Enter Play Mode.
        5. In the Inspector for this component, click the three dots (...) in the top right corner.
        6. Select "Test Load Specific Scene" or "Test Reload Current Scene" from the context menu to trigger the events.
        */
    }
}
