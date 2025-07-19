using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using PetalsOfHope.Core.Events;

namespace PetalsOfHope.Systems.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        [Header("Event Listeners")]
        [Tooltip("Listen for requests to load a specific scene.")]
        [SerializeField] private LoadSceneRequestEventSO loadSceneRequestChannel;
        
        [Tooltip("Listen for requests to reload the current scene.")]
        [SerializeField] private GameEventSO reloadCurrentSceneChannel;

        [Header("Event Raisers")]
        [Tooltip("Raised when a scene load is about to start (before fade out).")]
        [SerializeField] private GameEventSO sceneLoadStartedEvent;
        
        [Tooltip("Raised when a new scene has finished loading (after fade in).")]
        [SerializeField] private GameEventSO sceneLoadCompletedEvent;

        [Header("Transition Settings")]
        [Tooltip("CanvasGroup used for fade transitions.")]
        [SerializeField] private CanvasGroup fadeCanvasGroup;
        [Tooltip("Duration of the fade in/out animation.")]
        [SerializeField] private float fadeDuration = 0.5f;

        private bool _isLoading = false;

        private void Awake()
        {
            // Persist this object across scene loads
            DontDestroyOnLoad(gameObject);

            if (fadeCanvasGroup == null)
            {
                Debug.LogError("FadeCanvasGroup not assigned to SceneLoader! Transitions will not work.", this);
            }
            else
            {
                fadeCanvasGroup.alpha = 0;
                fadeCanvasGroup.blocksRaycasts = false;
            }
        }

        private void OnEnable()
        {
            if (loadSceneRequestChannel != null)
            {
                loadSceneRequestChannel.RegisterListener(OnLoadSceneRequested);
            }
            if (reloadCurrentSceneChannel != null)
            {
                reloadCurrentSceneChannel.RegisterListener(ReloadCurrentScene);
            }
        }

        private void OnDisable()
        {
            if (loadSceneRequestChannel != null)
            {
                loadSceneRequestChannel.UnregisterListener(OnLoadSceneRequested);
            }
            if (reloadCurrentSceneChannel != null)
            {
                reloadCurrentSceneChannel.UnregisterListener(ReloadCurrentScene);
            }
        }

        private void OnLoadSceneRequested(SceneDataSO sceneData)
        {
            if (sceneData != null && !_isLoading)
            {
                StartCoroutine(LoadSceneRoutine(sceneData.GetSceneName()));
            }
        }

        public void ReloadCurrentScene()
        {
            if (!_isLoading)
            {
                StartCoroutine(LoadSceneRoutine(SceneManager.GetActiveScene().name));
            }
        }

        private IEnumerator LoadSceneRoutine(string sceneName)
        {
            _isLoading = true;

            sceneLoadStartedEvent?.Raise();

            yield return StartCoroutine(Fade(1f));

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            while (asyncLoad is { isDone: false })
            {
                yield return null;
            }

            yield return StartCoroutine(Fade(0f));

            _isLoading = false;
            
            sceneLoadCompletedEvent?.Raise();
        }

        private IEnumerator Fade(float targetAlpha)
        {
            if (fadeCanvasGroup == null) yield break;

            fadeCanvasGroup.blocksRaycasts = true;
            var startAlpha = fadeCanvasGroup.alpha;
            var elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
                yield return null;
            }

            fadeCanvasGroup.alpha = targetAlpha;
            fadeCanvasGroup.blocksRaycasts = (targetAlpha >= 1f);
        }
        
        /*
        HOW TO USE:
        1. Place this component on a persistent GameObject in your initial scene (e.g., a "GameManager" object with DontDestroyOnLoad).
        2. Create a UI Canvas in the same scene. Set its Render Mode to "Screen Space - Overlay" and Sort Order to a high number (e.g., 100).
        3. Inside the Canvas, create a UI Image that covers the entire screen and set its color to black.
        4. Add a CanvasGroup component to this black Image.
        5. In the Inspector for this SceneLoader component, drag the CanvasGroup from the black image into the "Fade Canvas Group" field.
        6. Create and assign the "Load Scene Request Channel" and "Reload Current Scene Channel" ScriptableObject assets to the corresponding fields.
        7. The SceneLoader is now ready. It will automatically listen for events raised on these channels and handle scene loading.
        */
    }
}
