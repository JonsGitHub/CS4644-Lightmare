using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;
using TMPro;

public class UIInteractionItemFiller : MonoBehaviour
{
    [SerializeField] LocalizeStringEvent _interactionName = default;
    [SerializeField] TextMeshProUGUI _interactionStringName = default;
    [SerializeField] GameObject _keySelected = default;
    [SerializeField] Image _interactionIcon = default;

    public void FillInteractionPanel(InteractionSO interactionItem, string nameOverride = "")
    {
        SetSelected(false);
        if (nameOverride.Length > 0)
        {
            // TODO: Create localized string reference table for name overrides
            _interactionStringName.text = nameOverride;
        }
        else
        {
            _interactionName.StringReference = interactionItem.InteractionName;
        }
        _interactionIcon.sprite = interactionItem.InteractionIcon;
    }

    public void SetSelected(bool state)
    {
        _keySelected.SetActive(state);
    }
}