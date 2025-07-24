using UnityEngine;
using PetalsOfHope.Interfaces;

namespace PetalsOfHope.Core.Events
{
    [CreateAssetMenu(menuName = "Petals of Hope/Events/Load Scene Request Event")]
    public class LoadSceneRequestEventSO : TypedEventSO<ISceneData>
    {
        // This event uses the ISceneData interface as its payload.
        // This decouples the Core event system from the concrete SceneDataSO class in the Data assembly.
        
        /*
        HOW TO USE:
        1. Create an instance of this ScriptableObject asset.
        2. Name it "LoadSceneRequestChannel".
        3. In a requester script (e.g., a UI button), get a reference to this channel.
        4. In the requester, also get a reference to the specific SceneDataSO you want to load.
        5. Call the Raise method, passing the SceneDataSO. It will be implicitly cast to ISceneData.
           e.g., loadSceneRequestChannel.Raise(level1SceneData);
        */
    }
}
