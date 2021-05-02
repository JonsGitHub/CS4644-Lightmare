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
                    About = "In the forest dream from slaying King Slime",
                    Description = "As guardians, we cannot exist together. We create a cosmic fissure that allows Misfortune to corrupt the dreams of those we protect."
                }
            },
            {
                PlayerData.Crystal.Forest, new CrystalDescription()
                {
                    About = "In the forest dream's ancient crypts",
                    Description = "Misfortune. The spawn of darkness. The origin of Nightmares. A bane to the existence of dreamers and Light alike."
                }
            },
            {
                PlayerData.Crystal.DeerCrystal, new CrystalDescription()
                {
                    About = "In the forest dream from hunting the crystal deer",
                    Description = "There is no true understanding to Misfortune's purpose. We are ingrained with the knowledge of his damage and his belief in the shadows."
                }
            },
            {
                PlayerData.Crystal.WolfCrystal, new CrystalDescription()
                {
                    About = "In the forest dream from saving the missing girl",
                    Description = "Banished to a realm devoid of hope, he plots and schemes his return to infect other worlds with his suffering."
                }
            },
            {
                PlayerData.Crystal.SciFiCrystal, new CrystalDescription()
                {
                    About = "In the space dream from helping fix the ship",
                    Description = "Misfortune yearns for a bleak future. The indignant pain he has faced since origin times has been built up."
                }
            },
            {
                PlayerData.Crystal.OceanCrystal, new CrystalDescription()
                {
                    About = "In the broken dream from self reflection",
                    Description = "While Light are meant to keep Misfortune from entering the dreamscape, he still finds a way. The festering of evil is bubbling over, and soon a shadowy tomb is all the will be left of our world."
                }
            },
            {
                PlayerData.Crystal.ColosseumCrystal, new CrystalDescription()
                {
                    About = "In the colosseum dream from defeating Malady",
                    Description = "Where Misfortune comes from still remains a mystery. How Misfortune comes certainly isn't."
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
                _buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = ConvertToString((PlayerData.Crystal)i);
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

    private string ConvertToString(PlayerData.Crystal crystal)
    {
        switch(crystal)
        {
            case PlayerData.Crystal.ColosseumCrystal:
                return "Colosseum";
            case PlayerData.Crystal.DeerCrystal:
                return "Deer";
            case PlayerData.Crystal.Forest:
                return "Forest";
            case PlayerData.Crystal.OceanCrystal:
                return "Ocean";
            case PlayerData.Crystal.SciFiCrystal:
                return "Space";
            case PlayerData.Crystal.SlimeCrystal:
                return "Slime";
            case PlayerData.Crystal.WolfCrystal:
                return "Wolf";
        }
        return "";
    }
}
