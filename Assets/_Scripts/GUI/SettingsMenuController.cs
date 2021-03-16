using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller controlling the behaviour of the settings menu.
/// </summary>
public class SettingsMenuController : MonoBehaviour
{
    private GameObject pauseMenu;

    /// <summary>
    /// Awake called before Start of class
    /// </summary>
    private void Awake()
    {
        pauseMenu = transform.parent.Find("Pause_Menu").gameObject;

        var background = transform.Find("Background");
        background.Find("Mouse_Sensitivity").GetComponentInChildren<Slider>().value = Settings.Instance.MouseSensitivity;
        background.Find("Scroll_Sensitivity").GetComponentInChildren<Slider>().value = Settings.Instance.ScrollSensitivity;

        background.Find("Invert_YAxis").GetComponentInChildren<Toggle>().isOn = Settings.Instance.InvertedYAxis;
        background.Find("Invert_XAxis").GetComponentInChildren<Toggle>().isOn = Settings.Instance.InvertedXAxis;
    }

    #region Callbacks

    /// <summary>
    /// On Back button clicked callback method.
    /// </summary>
    public void OnBackClicked()
    {
        pauseMenu.SetActive(true);
        gameObject.SetActive(false);

        Settings.Instance.Save();
    }

    /// <summary>
    /// Inverted X Axis onvaluechanged callback method.
    /// </summary>
    /// <param name="value">The new value</param>
    public void OnInvertXAxisValueChanged(bool value)
    {
        Settings.Instance.InvertedXAxis = value;
    }

    /// <summary>
    /// Inverted Y Axis onvaluechanged callback method.
    /// </summary>
    /// <param name="value">The new value</param>
    public void OnInvertYAxisValueChanged(bool value)
    {
        Settings.Instance.InvertedYAxis = value;
    }

    /// <summary>
    /// Mouse Sensitivity onvaluechanged callback method.
    /// </summary>
    /// <param name="value">The new value</param>
    public void OnMouseSensitivtyValueChanged(float value)
    {
        Settings.Instance.MouseSensitivity = (int)value;
    }

    /// <summary>
    /// Scroll Sensitivity onvaluechanged callback method.
    /// </summary>
    /// <param name="value">The new value</param>
    public void OnScrollSensitivtyValueChanged(float value)
    {
        Settings.Instance.ScrollSensitivity = (int)value;
    }

    #endregion
}
