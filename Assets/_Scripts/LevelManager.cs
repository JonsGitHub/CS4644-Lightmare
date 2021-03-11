using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelManager
{
    public static void Load(string sceneName)
    {
        int index = SceneUtility.GetBuildIndexByScenePath(sceneName);
        if (index >= 0)
        {
            var loadingScreen = Resources.Load<LoadingScreenController>("Prefabs/LoadingScreen");

            // Add to the current scene
            loadingScreen = Object.Instantiate(loadingScreen);

            // Start Load sceen and scene loading
            loadingScreen.StartLoading(index);
        }
        else
        {
            Debug.LogError("Scene: " + sceneName + " does not exist.");
        }
    }

    public static void Quit()
    {
        // Check for saving data here

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
