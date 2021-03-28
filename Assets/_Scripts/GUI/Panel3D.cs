using UnityEngine;
using TMPro;

/// <summary>
/// A 3d panel UI.
/// </summary>
public class Panel3D : Ui3D
{
    private string _text;
    private TextMeshProUGUI _textBox;

    /// <summary>
    /// Gets or sets the text of the panel
    /// </summary>
    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            if (_textBox)
            {
                _textBox.text = _text;
            }
        }
    }

    public Color TextColor
    {
        get => _textBox.color;
        set => _textBox.color = value;
    }

    /// <summary>
    /// Awake called before Start of class
    /// </summary>
    private new void Awake()
    {
        base.Awake();
        _textBox = GetComponentInChildren<TextMeshProUGUI>();
    }
}
