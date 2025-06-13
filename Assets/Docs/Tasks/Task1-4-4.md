# Task ID: 1.4.4
# Parent Task ID: 1.4
# Title: Implement JsonDataService
# Status: completed
# Dependencies: 1.4.1 # IDataService interface
# Priority: high
# Estimated Effort: L
# Assignee: Unassigned

# Description:
Implement `JsonDataService.cs`, a concrete implementation of `IDataService` that uses file-based JSON storage in `Application.persistentDataPath`.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/Core/Persistence/Services/JsonDataService.cs`
2.  **Namespace:** `PetalsOfHope.Core.Persistence.Services`
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/Core/Persistence/Services/JsonDataService.cs
    namespace PetalsOfHope.Core.Persistence.Services
    {
        using UnityEngine;
        using PetalsOfHope.Core.Persistence.Interfaces;
        using System;
        using System.IO; // For File operations

        public class JsonDataService : IDataService
        {
            private string GetPathForKey(string key)
            {
                // Sanitize key to be a valid filename if necessary, or use key directly if it's simple
                // For simplicity, assuming key can be part of filename.
                // Add a .json extension.
                return Path.Combine(Application.persistentDataPath, $"{key}.json");
            }

            // Basic encryption/decryption (obfuscation) - XOR example
            // For real security, use a robust encryption library. This is just a placeholder.
            private string EncryptDecrypt(string data, string key)
            {
                if (string.IsNullOrEmpty(key) || key.Length < 8) // Simple key check
                {
                     Debug.LogWarning("Encryption key is too short or null. Using data as is.");
                     return data; // Or throw error
                }

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sb.Append((char)(data[i] ^ key[i % key.Length]));
                }
                return sb.ToString();
            }


            public bool Save<T>(string key, T data, bool encrypt = false)
            {
                string filePath = GetPathForKey(key);
                try
                {
                    string jsonData = JsonUtility.ToJson(data, true); // true for pretty print (optional)
                    if (encrypt)
                    {
                        // Replace "YourSecureEncryptionKey" with a real, securely managed key
                        jsonData = EncryptDecrypt(jsonData, "YourSecureEncryptionKey");
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
                if (File.Exists(filePath))
                {
                    try
                    {
                        string jsonData = File.ReadAllText(filePath);
                        if (decrypt)
                        {
                            // Replace "YourSecureEncryptionKey" with a real, securely managed key
                            jsonData = EncryptDecrypt(jsonData, "YourSecureEncryptionKey");
                        }
                        return JsonUtility.FromJson<T>(jsonData);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Failed to load/deserialize data from file '{filePath}'. Error: {e.Message}");
                        // Consider if file should be deleted or renamed if corrupt
                        return default(T);
                    }
                }
                else
                {
                    Debug.Log($"No data file found at '{filePath}'.");
                    return default(T);
                }
            }

            public bool Delete(string key)
            {
                string filePath = GetPathForKey(key);
                try
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    return true; // Successful even if file didn't exist
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
                    // Careful: This deletes all .json files in persistentDataPath.
                    // If other systems use .json files here, this needs to be more specific.
                    // For now, assuming only this service creates .json files with this pattern.
                    DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath);
                    FileInfo[] files = directoryInfo.GetFiles("*.json"); // Or a more specific pattern
                    foreach (FileInfo file in files)
                    {
                        file.Delete();
                    }
                    Debug.Log("All .json data files deleted from persistentDataPath.");
                    return true;
                {
                catch (Exception e)
                {
                    Debug.LogError($"Failed to delete all .json data files. Error: {e.Message}");
                    return false;
                }
            }


            public bool HasKey(string key)
            {
                string filePath = GetPathForKey(key);
                return File.Exists(filePath);
            }
        }
    }
    ```

# Acceptance Criteria:
- `JsonDataService.cs` implements the `IDataService` interface.
- `Save<T>` method serializes data to JSON and saves it to a file in `Application.persistentDataPath`.
- `Load<T>` method retrieves data from a file and deserializes it from JSON.
- `Delete`, `DeleteAll`, and `HasKey` methods correctly interact with files in `Application.persistentDataPath`.
- Basic error handling (try-catch blocks with `Debug.LogError`) is implemented.
- A placeholder encryption/decryption mechanism (e.g., simple XOR) is implemented if `encrypt`/`decrypt` flags are true.
- Script compiles without errors.

# Test Strategy:
- Unit Testing (or focused integration testing):
    - Create an instance of `JsonDataService`.
    - Test `Save` with a serializable class/struct. Verify file creation in `Application.persistentDataPath`.
    - Test `Load` to retrieve data from the file and verify its integrity.
    - Test `HasKey` before and after saving/deleting files.
    - Test `Delete` and verify file removal.
    - Test `DeleteAll` and verify all relevant .json files are removed.
    - Test with encryption/decryption flags on and off. Check file content (if not encrypted or simply obfuscated).

# Notes/Questions:
- `Application.persistentDataPath` is the standard location for user-specific persistent data.
- The `EncryptDecrypt` method provided is a very basic XOR obfuscation and **not secure**. For production, a robust encryption library (like BouncyCastle or .NET's `System.Security.Cryptography`) should be used if strong encryption is required. The key "YourSecureEncryptionKey" must be changed and managed securely. This is just to fulfill the interface contract.
- `DeleteAll()` needs careful consideration. If other parts of the application also store `.json` files in `persistentDataPath`, this method might delete too much. A more robust approach might involve a manifest file or a specific subdirectory managed by this service. For now, it deletes `*.json` as a simple starting point.
- Filename sanitization for `key` might be necessary if keys can contain characters invalid for filenames. The current `GetPathForKey` assumes simple keys.