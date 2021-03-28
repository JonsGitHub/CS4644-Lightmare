using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar3D : Ui3D
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _fillImage;
    [SerializeField] private Gradient _gradient;
    [SerializeField] private TextMeshProUGUI _textBox;
    private string _text;

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

    public float Health
    {
        get => _slider.value;
        set
        {
            _slider.value = value;
            _fillImage.color = _gradient.Evaluate(value / (float)MaxHealth);
        }
    }

    public float MaxHealth
    {
        get => _slider.maxValue;
        set => _slider.maxValue = value;
    }
}
