namespace PetalsOfHope.Contracts
{
    /// <summary>
    /// Defines the contract for any ScriptableObject that holds data about a scene.
    /// This allows the Core systems (like the SceneLoader) to load scenes
    /// without needing a direct reference to the concrete SceneDataSO class in the Data assembly.
    /// </summary>
    public interface ISceneData
    {
        /// <summary>
        /// Gets the name of the scene to be loaded.
        /// </summary>
        /// <returns>The build name of the scene.</returns>
        string GetSceneName();
    }
}
