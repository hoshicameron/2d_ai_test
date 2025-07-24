namespace PetalsOfHope.Interfaces
{
    /// <summary>
    /// Defines the contract for any ScriptableObject that represents a collectible item.
    /// </summary>
    public interface ICollectible
    {
        /// <summary>
        /// The unique identifier for this collectible. Used for saving and progression checks.
        /// </summary>
        string ID { get; }

        /// <summary>
        /// The name of the collectible displayed in the UI.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// The description of the collectible displayed in the UI.
        /// </summary>
        string Description { get; }
    }
}
