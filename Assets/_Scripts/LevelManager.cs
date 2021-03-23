using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Static class containing all of the methodology for scene
/// management and scene switching
/// </summary>
public static class LevelManager
{
    /// <summary>
    /// The current scene file path
    /// </summary>
    private static string CurrentSceneFilePath => Application.persistentDataPath + "/" + SceneManager.GetActiveScene().name.Replace(' ', '_') + ".dat";

    /// <summary>
    /// Loads the scene based on the passed scene name.
    /// </summary>
    /// <param name="sceneName">The scene name to load</param>
    public static void Load(string sceneName)
    {
        Save(); // Save the current scene before loading new scene

        int index = SceneUtility.GetBuildIndexByScenePath(sceneName);
        if (index >= 0)
        {
            //var loadingScreen = Resources.Load<LoadingScreenController>("Prefabs/LoadingScreen");

            //// Add to the current scene
            //loadingScreen = Object.Instantiate(loadingScreen);

            //// Start Load sceen and scene loading
            //loadingScreen.StartLoading(index);
        }
        else
        {
            Debug.LogError("Scene: " + sceneName + " does not exist.");
        }
    }

    /// <summary>
    /// Saves the current scene.
    /// </summary>
    public static void Save()
    {
        var controller = GameObject.FindGameObjectWithTag("SceneController")?.GetComponent<SceneController>();
        if (controller)
        {
            var formatter = new UnityBinaryFormatter();
            var file = File.OpenWrite(CurrentSceneFilePath);
            var data = controller.Save();
            formatter.Serialize(file, data);
            file.Close();
        }
    }

    /// <summary>
    /// Loading Step checking for save data of the current and loading 
    /// when applicable.
    /// </summary>
    public static void PostLoadingStep()
    {
        // TODO: Currently called at the end of the loading screen @line 44 - should be self contained to Level Manager (fix)
        
        var controller = GameObject.FindGameObjectWithTag("SceneController")?.GetComponent<SceneController>();
        if (controller && File.Exists(CurrentSceneFilePath))
        {
            Debug.Log("Loading Data at: " + CurrentSceneFilePath);
            
            var formatter = new UnityBinaryFormatter();
            var file = File.OpenRead(CurrentSceneFilePath);
            controller.Load(formatter.Deserialize(file));
            file.Close();
        }
    }

    /// <summary>
    /// Exits the application.
    /// </summary>
    public static void Quit()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
