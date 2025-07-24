namespace PetalsOfHope.Interfaces
{
    /// <summary>
    /// Interface for objects that can save and load their state.
    /// </summary>
    public interface ISaveable
    {
        /// <summary>
        /// A unique identifier for this saveable object.
        /// This is used to identify the object when saving/loading.
        /// </summary>
        string UniqueID { get; }

        /// <summary>
        /// Captures the current state of the object for saving.
        /// </summary>
        /// <returns>An object containing the state data to be serialized.</returns>
        object CaptureState();

        /// <summary>
        /// Restores the state of the object from saved data.
        /// </summary>
        /// <param name="state">The state data to restore from.</param>
        void RestoreState(object state);
    }
}
