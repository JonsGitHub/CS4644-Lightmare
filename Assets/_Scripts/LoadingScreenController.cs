using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Loading Screen Controller that contains the behavior required for 
/// a loading screen utilized as a transition between scenes.
/// </summary>
public class LoadingScreenController : MonoBehaviour
{
    public GameObject Background;

    private CanvasGroup LoadingScreen;

    /// <summary>
    /// Awake called before Start of class
    /// </summary>
    void Awake()
    {
        // Persist between scenes
        DontDestroyOnLoad(gameObject);

        LoadingScreen = GetComponent<CanvasGroup>();
        LoadingScreen.alpha = 0;
    }

    /// <summary>
    /// Starts async loading of the passed scene index.
    /// </summary>
    /// <param name="sceneIndex">The scene index to load</param>
    public void StartLoading(int sceneIndex)
    {
        Background.SetActive(true);

        StartCoroutine(LoadScene(sceneIndex));
    }

    /// <summary>
    /// Async loading of the scene based on the passed scene index
    /// and begins loading screen transition while loading.
    /// </summary>
    /// <param name="sceneIndex">The scene index to load</param>
    /// <returns></returns>
    private IEnumerator LoadScene(int sceneIndex)
    {
        // Open loading screen
        yield return StartCoroutine(FadeLoadingScreen(1, 1));

        // Start Async Loading of scene
        var operation = SceneManager.LoadSceneAsync(sceneIndex);
        while(!operation.isDone)
        {
            // Add updating progress bar here
            yield return null;
        }

        // Load Level Data - may async step?
        LevelManager.PostLoadingStep();

        yield return StartCoroutine(FadeLoadingScreen(0, 1));

        // End Loading and clean up
        EndLoading();
    }

    /// <summary>
    /// Updates the loading bar or imagery in the loading screen
    /// </summary>
    /// <param name="progress">The current loading progress</param>
    private void UpdateLoading(float progress)
    {
        // TODO: Implement
    }

    /// <summary>
    /// Adjusts the loading screen to fade to the target alpha value
    /// over the passed time duration.
    /// </summary>
    /// <param name="target">The target alpha value</param>
    /// <param name="duration">The time duration of the transition</param>
    /// <returns></returns>
    private IEnumerator FadeLoadingScreen(float target, float duration)
    {
        float startValue = LoadingScreen.alpha;
        float time = 0;

        while (time < duration)
        {
            LoadingScreen.alpha = Mathf.Lerp(startValue, target, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        LoadingScreen.alpha = target;
    }

    /// <summary>
    /// Cleans up the loading screen, to preserve game resources
    /// </summary>
    public void EndLoading()
    {
        // Clean up Gameobject
        Destroy(gameObject);
    }
}
