using UnityEngine;

/// <summary>
/// Scene Controller class managing loading and saving of a scene
/// </summary>
public abstract class SceneController : MonoBehaviour
{
    /// <summary>
    /// Awake called before Start of class
    /// </summary>
    protected void Awake()
    {
        tag = "SceneController";
    }

    /// <summary>
    /// Loads the scene data into the scene.
    /// </summary>
    /// <param name="data">The data to load</param>
    public abstract void Load(object data);

    /// <summary>
    /// Saves the current scene state.
    /// </summary>
    /// <returns>The current scene state's data</returns>
    public abstract SceneData Save();
}