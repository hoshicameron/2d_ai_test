# Task ID: 4.2.1
# Parent Task ID: 4.2
# Title: Implement SceneLoader Service
# Status: completed
# Dependencies: None (core Unity functionality)
# Priority: high
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Implement `SceneLoader.cs`, a service class responsible for loading scenes using `UnityEngine.SceneManagement`. It should include methods to load scenes by name/index and implement a basic fade-to-black transition.

# Details:
1.  **File Location:** `Assets/_Project/Scripts/Systems/SceneManagement/SceneLoader.cs`
2.  **Namespace:** `PetalsOfHope.Systems.SceneManagement`
3.  **Implementation:**
    ```csharp
    // In Assets/_Project/Scripts/Systems/SceneManagement/SceneLoader.cs
    namespace PetalsOfHope.Systems.SceneManagement
    {
        using UnityEngine;
        using UnityEngine.SceneManagement;
        using System.Collections;

        // This could be a static class, a MonoBehaviour Singleton, or a ScriptableObject service.
        // For simplicity, let's make it a MonoBehaviour Singleton that persists.
        public class SceneLoader : MonoBehaviour
        {
            public static SceneLoader Instance { get; private set; }

            [Header("Transition Settings")]
            [Tooltip("CanvasGroup used for fade transitions. Create one on a UI Canvas.")]
            [SerializeField] private CanvasGroup _fadeCanvasGroup;
            [Tooltip("Duration of the fade in/out animation.")]
            [SerializeField] private float _fadeDuration = 0.5f;

            private bool _isLoading = false;

            private void Awake()
            {
                if (Instance == null)
                {
                    Instance = this;
                    DontDestroyOnLoad(gameObject);
                    if (_fadeCanvasGroup == null)
                    {
                        Debug.LogError("FadeCanvasGroup not assigned to SceneLoader! Transitions will not work.", this);
                    }
                    else
                    {
                        _fadeCanvasGroup.alpha = 0; // Ensure it's invisible at start
                        _fadeCanvasGroup.blocksRaycasts = false; // Don't block input when invisible
                    }
                }
                else
                {
                    Destroy(gameObject);
                }
            }

            public void LoadScene(string sceneName)
            {
                if (_isLoading) return;
                StartCoroutine(LoadSceneRoutine(sceneName));
            }

            public void LoadScene(int buildIndex)
            {
                if (_isLoading) return;
                StartCoroutine(LoadSceneRoutine(buildIndex));
            }

            public void LoadMainMenu() // Assuming "MainMenu" is the name of your main menu scene
            {
                LoadScene("MainMenu");
            }
            
            public void ReloadCurrentScene()
            {
                if (_isLoading) return;
                StartCoroutine(LoadSceneRoutine(SceneManager.GetActiveScene().name));
            }

            private IEnumerator LoadSceneRoutine(string sceneName)
            {
                _isLoading = true;

                // Fade Out
                yield return StartCoroutine(Fade(1f)); // Fade to black

                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
                while (!asyncLoad.isDone)
                {
                    yield return null; // Wait for scene to load
                }

                // Fade In
                yield return StartCoroutine(Fade(0f)); // Fade from black

                _isLoading = false;
            }

            private IEnumerator LoadSceneRoutine(int buildIndex)
            {
                _isLoading = true;
                yield return StartCoroutine(Fade(1f));
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(buildIndex);
                while (!asyncLoad.isDone)
                {
                    yield return null;
                }
                yield return StartCoroutine(Fade(0f));
                _isLoading = false;
            }

            private IEnumerator Fade(float targetAlpha)
            {
                if (_fadeCanvasGroup == null) {
                    Debug.LogWarning("FadeCanvasGroup is null. Skipping fade.");
                    yield break;
                }

                _fadeCanvasGroup.blocksRaycasts = true; // Block input during fade
                float startAlpha = _fadeCanvasGroup.alpha;
                float elapsedTime = 0f;

                while (elapsedTime < _fadeDuration)
                {
                    elapsedTime += Time.unscaledDeltaTime; // Use unscaledDeltaTime for fades independent of game pause
                    _fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / _fadeDuration);
                    yield return null;
                }

                _fadeCanvasGroup.alpha = targetAlpha;
                _fadeCanvasGroup.blocksRaycasts = (targetAlpha == 1f); // Only block if fully faded out
            }
        }
    }
    ```
4.  **Setup Fade Canvas:**
    *   In your initial scene (e.g., a bootstrapper scene or your first game scene), create a UI Canvas (`GameObject > UI > Canvas`).
    *   Set its `Render Mode` to `Screen Space - Overlay` and `Sort Order` to a high value (e.g., 100) to ensure it's on top.
    *   Add an Image child to the Canvas (`UI > Image`). Make it cover the whole screen (Anchor presets: stretch-stretch). Set its color to black.
    *   Add a `CanvasGroup` component to this Image GameObject (or its parent Canvas, if the Canvas only contains this fade image).
    *   Create an instance of `SceneLoader` in your scene (or have it as part of a GameManager prefab) and assign this `CanvasGroup` to the `_fadeCanvasGroup` field.

# Acceptance Criteria:
- `SceneLoader.cs` (as a Singleton MonoBehaviour) is implemented.
- It provides methods `LoadScene(string sceneName)`, `LoadScene(int buildIndex)`, `LoadMainMenu()`, `ReloadCurrentScene()`.
- Scene loading is performed asynchronously using `SceneManager.LoadSceneAsync()`.
- A fade-to-black (and fade-from-black) transition using a `CanvasGroup` is implemented and plays during scene loads.
- `_fadeCanvasGroup` is assignable in the Inspector.
- The `SceneLoader` GameObject persists across scene loads (`DontDestroyOnLoad`).

# Test Strategy:
- Manual Testing:
    - Create at least two simple scenes (e.g., "TestSceneA", "TestSceneB"). Add them to `File > Build Settings > Scenes In Build`.
    - Place `SceneLoader.prefab` (or GameObject with `SceneLoader.cs` and configured CanvasGroup for fade) in "TestSceneA".
    *   Create UI Buttons or test scripts in "TestSceneA" that call `SceneLoader.Instance.LoadScene("TestSceneB")` and `SceneLoader.Instance.LoadScene(buildIndexForA)` (to reload).
    - In Play Mode, trigger scene loads.
    - Verify that the fade-out animation plays, the new scene loads, and the fade-in animation plays.
    - Verify `SceneLoader` instance persists.
    - Verify `LoadMainMenu()` and `ReloadCurrentScene()` work if relevant scenes are set up.

# Notes/Questions:
- A dedicated UI Canvas with a black Image and `CanvasGroup` is needed for the fade effect. This `SceneLoader` should ideally be part of a persistent GameManager object/prefab.
- Using `Time.unscaledDeltaTime` for fade coroutine makes the fade animation smooth even if `Time.timeScale` is set to 0 (e.g., for a pause menu).
- The `_isLoading` flag prevents starting multiple scene loads concurrently.