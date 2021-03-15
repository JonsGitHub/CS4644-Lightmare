using TMPro;

/// <summary>
/// A 3d panel UI.
/// </summary>
public class Panel3D : Ui3D
{
    private string text;
    private TextMeshProUGUI textBox;

    /// <summary>
    /// Gets or sets the text of the panel
    /// </summary>
    public string Text
    {
        get => text;
        set
        {
            text = value;
            if (textBox)
            {
                textBox.text = text;
            }
        }
    }

    /// <summary>
    /// Awake called before Start of class
    /// </summary>
    private new void Awake()
    {
        base.Awake();
        textBox = GetComponentInChildren<TextMeshProUGUI>();
    }
}
