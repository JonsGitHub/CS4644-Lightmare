using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenController : MonoBehaviour
{
    public GameObject Background;
    private CanvasGroup LoadingScreen;

    void Awake()
    {
        // Persist between scenes
        DontDestroyOnLoad(gameObject);

        LoadingScreen = GetComponent<CanvasGroup>();
        LoadingScreen.alpha = 0;
    }

    public void StartLoading(int sceneIndex)
    {
        Background.SetActive(true);

        StartCoroutine(LoadScene(sceneIndex));
    }

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

        yield return StartCoroutine(FadeLoadingScreen(0, 1));

        // End Loading and clean up
        EndLoading();

        LevelManager.PostLoadingStep();
    }

    public void UpdateLoading(float progress)
    {

    }

    IEnumerator FadeLoadingScreen(float target, float duration)
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

    public void EndLoading()
    {
        // Clean up Gameobject
        Destroy(gameObject);
    }
}
