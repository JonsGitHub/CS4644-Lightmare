using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameCanvasManager : MonoBehaviour
{
    public InputReader inputReader;

    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject SettingsMenu;
    [SerializeField] private TextMeshProUGUI MessageBox;
    [SerializeField] private DialogueController DialogueBox;

    private List<Ui3D> Uis = new List<Ui3D>();

    private void OnEnable()
    {
        inputReader.pauseEvent += Pause;
        inputReader.menuUnpauseEvent += Unpause;
    }

    private void OnDisable()
    {
        inputReader.pauseEvent -= Pause;
        inputReader.menuUnpauseEvent -= Unpause;
    }

    /// <summary>
    /// Last update called every frame
    /// </summary>
    private void LateUpdate()
    {
        foreach (var ui in Uis)
        {
            ui.UpdateUI();
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
    /// Adds a Ui3D to be rendered on the canvas
    /// </summary>
    /// <param name="ui">The Ui3D to be added</param>
    public void AddLabel(Ui3D ui)
    {
        ui.AttachTo(transform);
        Uis.Add(ui);
    }

    /// <summary>
    /// Removes a Ui3D from the canvas and destroys the instance.
    /// </summary>
    /// <param name="ui">The Ui3D to be removed</param>
    public void RemoveLabel(Ui3D ui)
    {
        Destroy(ui);
        Uis.Remove(ui);
    }

    #region Callbacks

    /// <summary>
    /// On Continue Button clicked callback method.
    /// </summary>
    public void OnContinueClicked()
    {
        inputReader.ManualUnpause();
    }

    /// <summary>
    /// On Save and Exit Button clicked callback method.
    /// </summary>
    public void OnSaveAndExitClicked()
    {
        //Time.timeScale = 1; // Un pause the game without relocking the mouse
        //Unpause();
        //LevelManager.Load("MainMenu");
    }

    /// <summary>
    /// On Settings Button clicked callback method.
    /// </summary>
    public void OnSettingsClicked()
    {
        PauseMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }

    #endregion Callbacks

    #region Private Methods

    /// <summary>
    /// Helper method that resumes the normal operation of the game.
    /// </summary>
    private void Unpause()
    {
        inputReader.EnableGameplayInput();

        PauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    /// <summary>
    /// Helper method that pauses the normal operation of the game.
    /// </summary>
    private void Pause()
    {
        inputReader.EnableMenuInput();

        PauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    #endregion
}
