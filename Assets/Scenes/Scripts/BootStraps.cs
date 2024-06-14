using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SD.EventSystem;
using SD.Characters;

namespace SD.SceneManagement
{
    /// <summary>
    /// Loads individual scenes asynchronously and compiles them into active gameplay
    /// </summary>
    public class BootStraps : MonoBehaviour
    {
        //Scenes to load in the background
        [SerializeField] private SceneCollection bootScenes;
        //Event to raise on startup
        [SerializeField] private GameEvent bootEvent;
        [SerializeField] private ScenePicker eventLogger;

        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private Image loadingProgressBar;

        [Header("Codices")]
        [SerializeField] private ItemCodex _itemCodex;
        [SerializeField] private LocationCodex _locationCodex;

        private List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
        private List<GameEvent> eventsToInvokeAfterLoading = new List<GameEvent>();

        private void Awake() => PreloadEventLogger();
        private void Start() => Boot();

        /// <summary>
        /// Load the event logger before anything else so all events are captured.
        /// </summary>
        private void PreloadEventLogger()
        {
            string eventLoggerName = Path.GetFileNameWithoutExtension(eventLogger.ScenePath);
            scenesToLoad.Add(SceneManager.LoadSceneAsync(eventLoggerName, LoadSceneMode.Additive));
        }

        private void Boot()
        {
            _itemCodex.Init();
            _locationCodex.Init();

            bootEvent?.Invoke();
            LoadSceneCollectionAdditive(bootScenes);
        }

        /// <summary>
        /// Sets the scene as active to host instantiated objects.
        /// </summary>
        public void SetActiveScene(ScenePicker scene)
        {
            if (scene == null)
            {
                Debug.LogWarning("Scene is null");
                return;
            }
            string sceneName = Path.GetFileNameWithoutExtension(scene.ScenePath);
            var activeScene = SceneManager.GetSceneByName(sceneName);
            if (activeScene == null)
            {
                Debug.LogWarning("Cannot find scene by name");
                return;
            }
            SceneManager.SetActiveScene(activeScene);
        }

        /// <summary>
        /// Loads a ScenePicker without a splash screen.
        /// </summary>
        /// <param name="scene">Scene Picker object to load.</param>
        public void LoadSceneAdditiveWithoutSplashScreen(ScenePicker scene)
        {
            GameEvent afterLoadingEvent = scene.SceneFinishedLoadingEvent;
            if (afterLoadingEvent)
                eventsToInvokeAfterLoading.Add(afterLoadingEvent);

            string sceneName = Path.GetFileNameWithoutExtension(scene.ScenePath);
            scenesToLoad.Add(SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive));
            StartCoroutine(LoadingWithoutSplashScreen());
        }

        /// <summary>
        /// Display splash screen while loading Scene Picker object.
        /// </summary>
        /// <param name="scene">Scene Picker object to load.</param>
        public void LoadSceneAdditiveWithSplashScreen(ScenePicker scene)
        {
            ShowLoadingScreen();

            GameEvent afterLoadingEvent = scene.SceneFinishedLoadingEvent;
            if (afterLoadingEvent)
                eventsToInvokeAfterLoading.Add(afterLoadingEvent);

            string sceneName = Path.GetFileNameWithoutExtension(scene.ScenePath);
            scenesToLoad.Add(SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive));
            StartCoroutine(LoadingWithSplashScreen());
        }

        /// <summary>
        /// Loads a Scene Collection and display splash screen.
        /// </summary>
        /// <param name="sceneCollection">Collection of ScenePickers to load.</param>
        public void LoadSceneCollectionAdditive(SceneCollection sceneCollection)
        {
            int i;

            ShowLoadingScreen();

            for (i = 0; i < sceneCollection.Scenes.Count; i++)
            {
                GameEvent afterLoadingEvent = sceneCollection.Scenes[i].SceneFinishedLoadingEvent;
                if (afterLoadingEvent)
                    eventsToInvokeAfterLoading.Add(afterLoadingEvent);

                string sceneName = Path.GetFileNameWithoutExtension(sceneCollection.Scenes[i].ScenePath);
                scenesToLoad.Add(SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive));
            }

            StartCoroutine(LoadingWithSplashScreen());
        }

        /// <summary>
        /// Deactivate a single active scene.
        /// </summary>
        /// <param name="scene">Scene Picker to deactivate.</param>
        public void UnloadScene(ScenePicker scene)
        {
            string sceneName = Path.GetFileNameWithoutExtension(scene.ScenePath);
            SceneManager.UnloadSceneAsync(sceneName);
        }

        /// <summary>
        /// Deactivate a collection of scenes.
        /// </summary>
        /// <param name="sceneCollection">SceneCollection to deactivate.</param>
        public void UnloadSceneCollection(SceneCollection sceneCollection)
        {
            int i;
            for (i = 0; i < sceneCollection.Scenes.Count; i++)
            {
                string sceneName = Path.GetFileNameWithoutExtension(sceneCollection.Scenes[i].ScenePath);
                SceneManager.UnloadSceneAsync(sceneName);
            }
        }

        private void ShowLoadingScreen()
        {
            loadingScreen.SetActive(true);
        }

        private void HideLoadingScreen()
        {
            loadingScreen.SetActive(false);
        }

        /// <summary>
        /// Load scenes in background.
        /// </summary>
        /// <returns>null</returns>
        private IEnumerator LoadingWithoutSplashScreen()
        {
            int i;
            int sceneCount = scenesToLoad.Count;

            for (i = 0; i < sceneCount; i++)
            {
                while (!scenesToLoad[i].isDone)
                {
                    yield return null;
                }
            }

            InvokeSceneLoadedEvents();
        }

        /// <summary>
        /// Load scenes in foreground, display splash screen. Close when finished.
        /// </summary>
        private IEnumerator LoadingWithSplashScreen()
        {
            int i;
            float totalProgress = 0;
            int sceneCount = scenesToLoad.Count;

            for (i = 0; i < sceneCount; i++)
            {
                while (!scenesToLoad[i].isDone)
                {
                    totalProgress += scenesToLoad[i].progress;
                    loadingProgressBar.fillAmount = totalProgress / sceneCount;
                    yield return null;
                }
            }
            HideLoadingScreen();
            InvokeSceneLoadedEvents();
        }

        /// <summary>
        /// Event callbacks after scene has been loaded.
        /// </summary>
        private void InvokeSceneLoadedEvents()
        {
            for (int i = 0; i < eventsToInvokeAfterLoading.Count; i++)
            {
                eventsToInvokeAfterLoading[i]?.Invoke();
            }
            eventsToInvokeAfterLoading.Clear();
        }
    }
}