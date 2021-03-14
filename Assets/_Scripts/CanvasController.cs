using UnityEngine;
using TMPro;

/// <summary>
/// Canvas Controller containing methods relevant to main
/// GUI behaviour.
/// </summary>
public class CanvasController : MonoBehaviour
{
    /// <summary>
    /// Enum representing the current mouse state
    /// </summary>
    private enum MouseState
    {
        Locked, Unlocked
    }
    
    private MouseState CurrentState;
    private GameObject PauseMenu;
    private TextMeshProUGUI MessageBox;

    /// <summary>
    /// Awake called before Start of class
    /// </summary>
    private void Awake()
    {
        PauseMenu = transform.Find("Pause_Menu").gameObject;
        MessageBox = transform.Find("Message").GetComponent<TextMeshProUGUI>();
        MessageBox.gameObject.SetActive(false);
        Unpause();
    }

    /// <summary>
    /// Update called every frame
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentState == MouseState.Unlocked)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
    }

    /// <summary>
    /// Sets the message box to the passed string.
    /// </summary>
    /// <param name="text">The string to display</param>
    public void SetMessage(string text)
    {
        MessageBox.gameObject.SetActive(true);
        MessageBox.text = text;
    }

    /// <summary>
    /// Clears the message box.
    /// </summary>
    public void ClearMessage()
    {
        MessageBox.gameObject.SetActive(false);
    }

    /// <summary>
    /// On Continue Button clicked callback method.
    /// </summary>
    public void OnContinueClicked()
    {
        Unpause();
    }

    /// <summary>
    /// On Save and Exit Button clicked callback method.
    /// </summary>
    public void OnSaveAndExitClicked()
    {
        Time.timeScale = 1; // Un pause the game without relocking the mouse
        LevelManager.Load("MainMenu");
    }

    /// <summary>
    /// Helper method that resumes the normal operation of the game.
    /// </summary>
    private void Unpause()
    {
        PauseMenu.SetActive(false);

        CurrentState = MouseState.Locked;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        Time.timeScale = 1;
    }

    /// <summary>
    /// Helper method that pauses the normal operation of the game.
    /// </summary>
    private void Pause()
    {
        PauseMenu.SetActive(true);

        CurrentState = MouseState.Unlocked;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        Time.timeScale = 0;
    }
}
