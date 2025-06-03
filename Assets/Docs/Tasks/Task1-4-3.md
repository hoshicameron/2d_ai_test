# Task ID: 1.4.3
# Parent Task ID: 1.4
# Title: Implement PlayerPrefsDataService
# Status: pending
# Dependencies: 1.4.1 # IDataService interface
# Priority: high
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement `PlayerPrefsDataService.cs`, a concrete implementation of `IDataService` that uses `PlayerPrefs` for data storage, with JSON serialization for complex objects.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/Core/Persistence/Services/PlayerPrefsDataService.cs`
2.  **Namespace:** `PetalsOfHope.Core.Persistence.Services`
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/Core/Persistence/Services/PlayerPrefsDataService.cs
    namespace PetalsOfHope.Core.Persistence.Services
    {
        using UnityEngine;
        using PetalsOfHope.Core.Persistence.Interfaces;
        using System; // For Exception

        public class PlayerPrefsDataService : IDataService
        {
            public bool Save<T>(string key, T data, bool encrypt = false)
            {
                if (encrypt)
                {
                    Debug.LogWarning("PlayerPrefsDataService does not support encryption. Data will be saved unencrypted.");
                    // Optionally, could integrate a simple XOR or Base64 obfuscation here if desired
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
                    Debug.LogWarning("PlayerPrefsDataService does not support decryption. Data will be loaded as is.");
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
                        return default(T);
                    }
                }
                else
                {
                    Debug.Log($"No data found for key '{key}' in PlayerPrefs.");
                    return default(T);
                }
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
                    Debug.LogError($"Failed to delete all data from PlayerPrefs. Error: {e.Message}");
                    return false;
                }
            }

            public bool HasKey(string key)
            {
                return PlayerPrefs.HasKey(key);
            }
        }
    }
    ```

# Acceptance Criteria:
- `PlayerPrefsDataService.cs` implements the `IDataService` interface.
- `Save<T>` method serializes data to JSON and saves it to `PlayerPrefs`.
- `Load<T>` method retrieves data from `PlayerPrefs` and deserializes it from JSON.
- `Delete`, `DeleteAll`, and `HasKey` methods correctly interact with `PlayerPrefs`.
- Basic error handling (try-catch blocks with `Debug.LogError`) is implemented for save/load operations.
- Encryption/decryption parameters are acknowledged with warnings as `PlayerPrefs` doesn't natively support them securely.
- Script compiles without errors.

# Test Strategy:
- Unit Testing (or focused integration testing):
    - Create an instance of `PlayerPrefsDataService`.
    - Test `Save` with a simple serializable class/struct.
    - Test `Load` to retrieve the same data and verify its integrity.
    - Test `HasKey` before and after saving/deleting.
    - Test `Delete` and verify `HasKey` returns false.
    - Test `DeleteAll` and verify previously saved keys are gone.
    - Manually inspect PlayerPrefs (e.g., in Windows Registry or macOS .plist file) to confirm data storage, if necessary for deep debugging.

# Notes/Questions:
- `JsonUtility` has limitations (e.g., doesn't directly serialize dictionaries or properties without `[SerializeField]`). If more complex serialization is needed, consider alternatives like Newtonsoft Json.NET (requires importing the package), but `JsonUtility` is fine for many Unity use cases and keeps dependencies low. The plan implies standard JSON serialization, so `JsonUtility` is a good first choice.
- `PlayerPrefs.Save()` is called after `SetString`, `DeleteKey`, and `DeleteAll` to ensure data is flushed to disk, though Unity often handles this at application quit/pause. Explicit calls are safer for immediate persistence.
- True encryption for `PlayerPrefs` is non-trivial. The current implementation logs a warning.