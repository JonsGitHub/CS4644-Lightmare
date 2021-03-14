using UnityEngine;

/// <summary>
/// Main Menu Controller behavior script
/// </summary>
public class MainMenuController : MonoBehaviour
{
    /// <summary>
    /// On Test Scene Button clicked callback
    /// </summary>
    public void OnTestSceneClicked()
    {
        LevelManager.Load("TestScene1");
    }

    /// <summary>
    /// On Quit Button clicked callback
    /// </summary>
    public void OnQuitClicked()
    {
        LevelManager.Quit();
    }
}
