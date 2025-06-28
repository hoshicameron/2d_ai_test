using UnityEngine;
using PetalsOfHope.Core.Persistence.Interfaces;
using System;
using System.IO;
using System.Text;

namespace PetalsOfHope.Core.Persistence.Services
{
    /// <summary>
    /// Implementation of IDataService that saves data to JSON files in the persistent data path.
    /// Better for larger or more complex data that might need to be accessed outside the game.
    /// </summary>
    public class JsonDataService : IDataService
    {
        private string GetPathForKey(string key)
        {
            // Sanitize key to be a valid filename
            string safeKey = string.Join("", key.Split(Path.GetInvalidFileNameChars()));
            return Path.Combine(Application.persistentDataPath, $"{safeKey}.json");
        }

        // Simple XOR encryption/decryption - for demonstration only
        // In production, use a proper encryption library
        private string EncryptDecrypt(string data, string key)
        {
            if (string.IsNullOrEmpty(key) || key.Length < 8)
            {
                Debug.LogWarning("Encryption key is too short or null. Using data as is.");
                return data;
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append((char)(data[i] ^ key[i % key.Length]));
            }
            return sb.ToString();
        }

        public bool Save<T>(string key, T data, bool encrypt = false)
        {
            string filePath = GetPathForKey(key);
            string directory = Path.GetDirectoryName(filePath);
            
            try
            {
                // Ensure the directory exists
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string jsonData = JsonUtility.ToJson(data, true); // Pretty print for readability
                
                if (encrypt)
                {
                    // In a real application, use a secure key management solution
                    const string encryptionKey = "YourSecureEncryptionKey123!";
                    jsonData = EncryptDecrypt(jsonData, encryptionKey);
                }

                File.WriteAllText(filePath, jsonData);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save data to file '{filePath}'. Error: {e.Message}");
                return false;
            }
        }

        public T Load<T>(string key, bool decrypt = false)
        {
            string filePath = GetPathForKey(key);
            
            if (!File.Exists(filePath))
            {
                Debug.Log($"No data file found at '{filePath}'.");
                return default;
            }

            try
            {
                string jsonData = File.ReadAllText(filePath);
                
                if (decrypt)
                {
                    // Must use the same key used for encryption
                    const string encryptionKey = "YourSecureEncryptionKey123!";
                    jsonData = EncryptDecrypt(jsonData, encryptionKey);
                }

                return JsonUtility.FromJson<T>(jsonData);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load/deserialize data from file '{filePath}'. Error: {e.Message}");
                return default;
            }
        }

        public bool HasKey(string key)
        {
            string filePath = GetPathForKey(key);
            return File.Exists(filePath);
        }

        public bool Delete(string key)
        {
            string filePath = GetPathForKey(key);
            
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                return true; // Return true if file didn't exist (treated as success)
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to delete file '{filePath}'. Error: {e.Message}");
                return false;
            }
        }

        public bool DeleteAll()
        {
            try
            {
                string[] jsonFiles = Directory.GetFiles(Application.persistentDataPath, "*.json");
                bool allDeleted = true;
                
                foreach (string file in jsonFiles)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Failed to delete file '{file}'. Error: {e.Message}");
                        allDeleted = false;
                    }
                }
                
                if (allDeleted)
                {
                    Debug.Log($"Successfully deleted {jsonFiles.Length} JSON files.");
                }
                
                return allDeleted;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to delete JSON files. Error: {e.Message}");
                return false;
            }
        }
    }
}
