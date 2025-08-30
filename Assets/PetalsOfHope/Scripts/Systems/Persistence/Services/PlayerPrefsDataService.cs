using System;
using PetalsOfHope.Contracts;
using UnityEngine;

namespace PetalsOfHope.Systems.Persistence.Services
{
    /// <summary>
    /// Implementation of IDataService that uses Unity's PlayerPrefs for data persistence.
    /// Best for small amounts of simple data that doesn't need to be easily accessible outside the game.
    /// </summary>
    public class PlayerPrefsDataService : IDataService
    {
        public bool Save<T>(string key, T data, bool encrypt = false)
        {
            if (encrypt)
            {
                Debug.LogWarning($"{nameof(PlayerPrefsDataService)} does not support encryption. Data will be saved unencrypted.");
            }

            try
            {
                string jsonData = JsonUtility.ToJson(data);
                PlayerPrefs.SetString(key, jsonData);
                PlayerPrefs.Save(); // Ensure data is written to disk
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save data with key '{key}' to PlayerPrefs. Error: {e.Message}");
                return false;
            }
        }

        public T Load<T>(string key, bool decrypt = false)
        {
            if (decrypt)
            {
                Debug.LogWarning($"{nameof(PlayerPrefsDataService)} does not support decryption. Data will be loaded as is.");
            }

            if (PlayerPrefs.HasKey(key))
            {
                try
                {
                    string jsonData = PlayerPrefs.GetString(key);
                    return JsonUtility.FromJson<T>(jsonData);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to load/deserialize data with key '{key}' from PlayerPrefs. Error: {e.Message}");
                    return default;
                }
            }
            
            Debug.Log($"No data found for key '{key}' in PlayerPrefs.");
            return default;
        }

        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public bool Delete(string key)
        {
            try
            {
                if (PlayerPrefs.HasKey(key))
                {
                    PlayerPrefs.DeleteKey(key);
                    PlayerPrefs.Save(); // Ensure change is written
                }
                return true; // Successful even if key didn't exist
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to delete data with key '{key}' from PlayerPrefs. Error: {e.Message}");
                return false;
            }
        }

        public bool DeleteAll()
        {
            try
            {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save(); // Ensure change is written
                Debug.Log("All PlayerPrefs data deleted.");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to delete all PlayerPrefs data. Error: {e.Message}");
                return false;
            }
        }
    }
}
