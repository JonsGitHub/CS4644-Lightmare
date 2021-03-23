//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;

///// <summary>
///// Canvas Controller containing methods relevant to main
///// GUI behaviour.
///// </summary>
//public class CanvasController : MonoBehaviour
//{
//    /// <summary>
//    /// Enum representing the current mouse state
//    /// </summary>
//    private enum MouseState
//    {
//        Locked, Unlocked
//    }

//    private MouseState CurrentState;
//    private GameObject PauseMenu;
//    private GameObject SettingsMenu;
//    private TextMeshProUGUI MessageBox;
//    //private DialogueController DialogueController;

//    private List<Ui3D> Uis = new List<Ui3D>();

//    /// <summary>
//    /// Awake called before Start of class
//    /// </summary>
//    private void Awake()
//    {

//        PauseMenu = transform.Find("Pause_Menu").gameObject;
//        SettingsMenu = transform.Find("Settings_Menu").gameObject;
//        SettingsMenu.SetActive(false);

//        MessageBox = transform.Find("Message").GetComponent<TextMeshProUGUI>();
//        MessageBox.gameObject.SetActive(false);

//        DialogueController = transform.Find("Dialogue_Box").GetComponent<DialogueController>();
//        DialogueController.gameObject.SetActive(false);

//        Unpause();
//    }

//    /// <summary>
//    /// Update called every frame
//    /// </summary>
//    private void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Escape))
//        {
//            if (CurrentState == MouseState.Unlocked)
//            {
//                Unpause();
//            }
//            else
//            {
//                Pause();
//            }
//        }
//    }

//    /// <summary>
//    /// Last update called every frame
//    /// </summary>
//    private void LateUpdate()
//    {
//        foreach (var ui in Uis)
//        {
//            ui.UpdateUI();
//        }
//    }

//    public void StartConversation(Conversation conversation) => DialogueController?.StartConversation(conversation);

//    /// <summary>
//    /// Sets the message box to the passed string.
//    /// </summary>
//    /// <param name="text">The string to display</param>
//    public void SetMessage(string text)
//    {
//        MessageBox.gameObject.SetActive(true);
//        MessageBox.text = text;
//    }

//    /// <summary>
//    /// Clears the message box.
//    /// </summary>
//    public void ClearMessage()
//    {
//        MessageBox.gameObject.SetActive(false);
//    }

//    /// <summary>
//    /// Adds a Ui3D to be rendered on the canvas
//    /// </summary>
//    /// <param name="ui">The Ui3D to be added</param>
//    public void AddLabel(Ui3D ui)
//    {
//        ui.AttachTo(transform);
//        Uis.Add(ui);
//    }

//    /// <summary>
//    /// Removes a Ui3D from the canvas and destroys the instance.
//    /// </summary>
//    /// <param name="ui">The Ui3D to be removed</param>
//    public void RemoveLabel(Ui3D ui)
//    {
//        Destroy(ui);
//        Uis.Remove(ui);
//    }

//    #region Callbacks

//    /// <summary>
//    /// On Continue Button clicked callback method.
//    /// </summary>
//    public void OnContinueClicked()
//    {
//        Unpause();
//    }

//    /// <summary>
//    /// On Save and Exit Button clicked callback method.
//    /// </summary>
//    public void OnSaveAndExitClicked()
//    {
//        Time.timeScale = 1; // Un pause the game without relocking the mouse
//        LevelManager.Load("MainMenu");
//    }

//    /// <summary>
//    /// On Settings Button clicked callback method.
//    /// </summary>
//    public void OnSettingsClicked()
//    {
//        PauseMenu.SetActive(false);
//        SettingsMenu.SetActive(true);
//    }

//    #endregion Callbacks

//    #region Private Methods

//    /// <summary>
//    /// Helper method that resumes the normal operation of the game.
//    /// </summary>
//    private void Unpause()
//    {
//        PauseMenu.SetActive(false);

//        CurrentState = MouseState.Locked;
//        Cursor.lockState = CursorLockMode.Locked;
//        Cursor.visible = false;
        
//        Time.timeScale = 1;
//    }

//    /// <summary>
//    /// Helper method that pauses the normal operation of the game.
//    /// </summary>
//    private void Pause()
//    {
//        PauseMenu.SetActive(true);

//        CurrentState = MouseState.Unlocked;
//        Cursor.lockState = CursorLockMode.None;
//        Cursor.visible = true;
        
//        Time.timeScale = 0;
//    }

//    #endregion
//}
