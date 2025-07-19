using UnityEngine;


namespace PetalsOfHope.Core.Events
{
    [CreateAssetMenu(menuName = "Petals of Hope/Events/Load Scene Request Event")]
    public class LoadSceneRequestEventSO : TypedEventSO<SceneDataSO>
    {
        // No additional implementation needed, inherits all functionality from TypedEventSO<SceneDataSO>
        
        /*
        HOW TO USE:
        1. Create an instance of this ScriptableObject in your project assets (e.g., via Right-Click -> Create -> Petals of Hope -> Events -> Load Scene Request Event).
        2. Name it something like "LoadSceneRequestChannel".
        3. In any script that needs to request a scene load (e.g., a UI button, a level-end trigger), add a SerializedField of this type:
           [SerializeField] private LoadSceneRequestEventSO loadSceneRequestChannel;
        4. Assign the "LoadSceneRequestChannel" asset to this field in the Inspector.
        5. To trigger a scene load, call the Raise method with the SceneDataSO of the scene you want to load:
           loadSceneRequestChannel.Raise(targetSceneData);
        */
    }
}
