using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelManager
{
    private static string CurrentSceneFilePath => Application.persistentDataPath + "/" + SceneManager.GetActiveScene().name.Replace(' ', '_') + ".dat";

    public static void Load(string sceneName)
    {
        Save(); // Save the current scene before loading new scene

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

    public static void Save()
    {
        var controller = GameObject.FindGameObjectWithTag("SceneController")?.GetComponent<SceneController>();
        if (controller)
        {
            SurrogateSelector surrogateSelector = new SurrogateSelector();
            var vector3SS = new Vector3SerializationSurrogate();
            var quaternionSS = new QuaternionSerializationSurrogate();
            surrogateSelector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3SS);
            surrogateSelector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaternionSS);

            var formatter = new BinaryFormatter()
            {
                SurrogateSelector = surrogateSelector
            };

            var file = File.OpenWrite(CurrentSceneFilePath);
            var data = controller.Save();
            formatter.Serialize(file, data);
            file.Close();
        }
    }

    // Currently called at the end of the loading screen @line 44 - should be self contained to Level Manager (fix)
    public static void PostLoadingStep()
    {
        var controller = GameObject.FindGameObjectWithTag("SceneController")?.GetComponent<SceneController>();
        if (controller && File.Exists(CurrentSceneFilePath))
        {
            SurrogateSelector surrogateSelector = new SurrogateSelector();
            var vector3SS = new Vector3SerializationSurrogate();
            var quaternionSS = new QuaternionSerializationSurrogate();
            surrogateSelector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3SS);
            surrogateSelector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaternionSS);

            var formatter = new BinaryFormatter()
            {
                SurrogateSelector = surrogateSelector
            };

            Debug.Log("Loading Data at: " + CurrentSceneFilePath);
            var file = File.OpenRead(CurrentSceneFilePath);
            controller.Load(formatter.Deserialize(file));
            file.Close();
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
