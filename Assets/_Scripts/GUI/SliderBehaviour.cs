using UnityEngine;
using TMPro;

/// <summary>
/// Custom behaviour script updating custom label for UI slider
/// </summary>
public class SliderBehaviour : MonoBehaviour
{
    /// <summary>
    /// Sliders onvaluechanged callback method.
    /// </summary>
    /// <param name="value">The new value</param>
    public void OnValueChanged(float value)
    {
        transform.Find("Indicator").GetComponent<TextMeshProUGUI>().text = value.ToString();
    }
}
