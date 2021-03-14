using UnityEngine;

/// <summary>
/// Scene Controller class managing loading and saving of a scene
/// </summary>
public class SceneController : MonoBehaviour, ISceneController
{
    /// <summary>
    /// Awake called before Start of class
    /// </summary>
    protected void Awake()
    {
        tag = "SceneController";
    }

    /// <inheritdoc/>
    public virtual void Load(object data)
    {
        throw new System.NotImplementedException();
    }

    /// <inheritdoc/>
    public virtual SceneData Save()
    {
        throw new System.NotImplementedException();
    }
}

/// <summary>
/// Interface representing required methods for a scene controller
/// </summary>
public interface ISceneController
{
    /// <summary>
    /// Saves the current scene state.
    /// </summary>
    /// <returns>The current scene state's data</returns>
    SceneData Save();

    /// <summary>
    /// Loads the scene data into the scene.
    /// </summary>
    /// <param name="data">The data to load</param>
    void Load(object data);
}
