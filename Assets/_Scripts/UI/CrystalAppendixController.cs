using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrystalAppendixController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _aboutField;
    [SerializeField] private TextMeshProUGUI _descriptionField;
    
    public List<GameObject> _buttons = new List<GameObject>();

    public struct CrystalDescription
    {
        public string About;
        public string Description;
    }

    private Dictionary<PlayerData.Crystal, CrystalDescription> _dictionary;

    private void Awake()
    {
        _dictionary = new Dictionary<PlayerData.Crystal, CrystalDescription>()
        {
            {
                PlayerData.Crystal.SlimeCrystal, new CrystalDescription()
                {
                    About = "Found: In the forest dream from slaying King Slime",
                    Description = "Message: [here]"
                }
            },
            {
                PlayerData.Crystal.Forest, new CrystalDescription()
                {
                    About = "Found: In the forest dream's ancient crypts",
                    Description = "Message: [here]"
                }
            },
            {
                PlayerData.Crystal.DeerCrystal, new CrystalDescription()
                {
                    About = "Found: In the forest dream from hunting the crystal deer",
                    Description = "Message: [here]"
                }
            },
            {
                PlayerData.Crystal.WolfCrystal, new CrystalDescription()
                {
                    About = "Found: In the forest dream from saving the missing girl",
                    Description = "Message: [here]"
                }
            }
        };
    }

    private void OnEnable()
    {
        // Check which crystals the player currently and enable accordingly
        for (int i = 0; i < _buttons.Count; ++i)
        {
            if (PlayerData.HasCrystal((PlayerData.Crystal)i))
            {
                _buttons[i].GetComponent<Button>().interactable = true;
                _buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = ((PlayerData.Crystal)i).ToString();
            }
            else
            {
                _buttons[i].GetComponent<Button>().interactable = false;
                _buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = "? ? ?";
            }
        }
    }

    public void OnSelectionButtonPressed(int index)
    {
        if (_dictionary.TryGetValue((PlayerData.Crystal)index, out CrystalDescription description))
        {
            _aboutField.text = description.About;
            _descriptionField.text = description.Description;
        }
    }
}
