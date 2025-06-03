using System;

namespace PetalsOfHope.Core.Persistence.Interfaces
{
    /// <summary>
    /// Interface for data persistence services.
    /// Defines methods for saving, loading, and deleting data.
    /// </summary>
    public interface IDataService
    {
        /// <summary>
        /// Saves data to a persistent storage.
        /// </summary>
        /// <typeparam name="T">The type of data to save.</typeparam>
        /// <param name="key">A unique key to identify the data.</param>
        /// <param name="data">The data object to save.</param>
        /// <param name="encrypt">Whether to encrypt the data before saving.</param>
        /// <returns>True if saving was successful, false otherwise.</returns>
        bool Save<T>(string key, T data, bool encrypt = false);

        /// <summary>
        /// Loads data from persistent storage.
        /// </summary>
        /// <typeparam name="T">The type of data to load.</typeparam>
        /// <param name="key">The unique key identifying the data.</param>
        /// <param name="decrypt">Whether to decrypt the data after loading.</param>
        /// <returns>The loaded data object, or default(T) if not found or an error occurs.</returns>
        T Load<T>(string key, bool decrypt = false);

        /// <summary>
        /// Checks if data exists for the given key.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if data exists, false otherwise.</returns>
        bool HasKey(string key);

        /// <summary>
        /// Deletes specific data associated with a key.
        /// </summary>
        /// <param name="key">The key of the data to delete.</param>
        /// <returns>True if deletion was successful or key did not exist, false on error.</returns>
        bool Delete(string key);

        /// <summary>
        /// Deletes all data managed by this service.
        /// </summary>
        /// <returns>True if deletion was successful, false otherwise.</returns>
        bool DeleteAll();
    }
}
