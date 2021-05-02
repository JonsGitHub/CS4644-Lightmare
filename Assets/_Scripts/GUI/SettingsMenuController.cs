using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller controlling the behaviour of the settings menu.
/// </summary>
public class SettingsMenuController : MonoBehaviour
{
    [SerializeField] private GameObject _controlsPage;
    [SerializeField] private GameObject _graphicsPage;
    [SerializeField] private GameObject _audioPage;

    private Toggle _highGraphics;
    private Toggle _mediumGraphics;
    private Toggle _lowGraphics;

    private GameObject _highDetails;
    private GameObject _mediumDetails;
    private GameObject _lowDetails;

    /// <summary>
    /// Awake called before Start of class
    /// </summary>
    private void OnEnable()
    {
        _controlsPage.transform.Find("Mouse_Sensitivity").GetComponentInChildren<Slider>().value = Settings.Instance.MouseSensitivity;
        _controlsPage.transform.Find("Scroll_Sensitivity").GetComponentInChildren<Slider>().value = Settings.Instance.ScrollSensitivity;
        _controlsPage.transform.Find("Invert_YAxis").GetComponentInChildren<Toggle>().isOn = Settings.Instance.InvertedYAxis;
        _controlsPage.transform.Find("Invert_XAxis").GetComponentInChildren<Toggle>().isOn = Settings.Instance.InvertedXAxis;

        _audioPage.transform.Find("Master_Volume").GetComponentInChildren<Slider>().value = Settings.Instance.MasterVolume;
        _audioPage.transform.Find("Music_Volume").GetComponentInChildren<Slider>().value = Settings.Instance.MusicVolume;
        _audioPage.transform.Find("Dialogue_Volume").GetComponentInChildren<Slider>().value = Settings.Instance.DialogueVolume;
        _audioPage.transform.Find("SFX_Volume").GetComponentInChildren<Slider>().value = Settings.Instance.SFXVolume;

        var quality = _graphicsPage.transform.Find("Quality").transform;
        _highGraphics = quality.Find("HighToggle").GetComponent<Toggle>();
        _mediumGraphics = quality.Find("MediumToggle").GetComponent<Toggle>();
        _lowGraphics = quality.Find("LowToggle").GetComponent<Toggle>();
        _highDetails = _graphicsPage.transform.Find("High_Details").gameObject;
        _mediumDetails = _graphicsPage.transform.Find("Medium_Details").gameObject;
        _lowDetails = _graphicsPage.transform.Find("Low_Details").gameObject;

        switch (Settings.Instance.GraphicsQualityLevel)
        {
            case 2:
                _highGraphics.isOn = true;
                break;
            case 1:
                _mediumGraphics.isOn = true;
                break;
            case 0:
                _lowGraphics.isOn = true;
                break;
        }
    }

    #region Callbacks

    /// <summary>
    /// On Back button clicked callback method.
    /// </summary>
    public void OnBackClicked()
    {
        Settings.Instance.Save();

        // Update Audio Settings with new values
        

        // Update Graphics Settings with new values

    }

    public void SelectControls()
    {
        _controlsPage.SetActive(true);
        _graphicsPage.SetActive(false);
        _audioPage.SetActive(false);
    }

    public void SelectGraphics()
    {
        _controlsPage.SetActive(false);
        _graphicsPage.SetActive(true);
        _audioPage.SetActive(false);
    }

    public void SelectAudio()
    {
        _controlsPage.SetActive(false);
        _graphicsPage.SetActive(false);
        _audioPage.SetActive(true);
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

    public void OnMasterVolumeValueChanged(float value)
    {
        Settings.Instance.MasterVolume = (int)value;
    }

    public void OnMusicVolumeValueChanged(float value)
    {
        Settings.Instance.MusicVolume = (int)value;
    }

    public void OnDialogueVolumeValueChanged(float value)
    {
        Settings.Instance.DialogueVolume = (int)value;
    }

    public void OnSFXVolumeValueChanged(float value)
    {
        Settings.Instance.SFXVolume = (int)value;
    }

    public void OnHighToggleValueChanged(bool value)
    {
        if (value)
        {
            Settings.Instance.GraphicsQualityLevel = 2;
            QualitySettings.SetQualityLevel(2, true);
            _highDetails.SetActive(true);

            _mediumGraphics.isOn = false;
            _lowGraphics.isOn = false;
            _mediumDetails.SetActive(false);
            _lowDetails.SetActive(false);
        }
    }

    public void OnMediumToggleValueChanged(bool value)
    {
        if (value)
        {
            Settings.Instance.GraphicsQualityLevel = 1;
            QualitySettings.SetQualityLevel(1, true);
            _mediumDetails.SetActive(true);

            _highGraphics.isOn = false;
            _lowGraphics.isOn = false;
            _highDetails.SetActive(false);
            _lowDetails.SetActive(false);
        }
    }

    public void OnLowToggleValueChanged(bool value)
    {
        if (value)
        {
            Settings.Instance.GraphicsQualityLevel = 0;
            QualitySettings.SetQualityLevel(0, true);
            _lowDetails.SetActive(true);

            _highGraphics.isOn = false;
            _mediumGraphics.isOn = false;
            _highDetails.SetActive(false);
            _mediumDetails.SetActive(false);
        }
    }

    #endregion
}
