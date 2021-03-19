using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    private TextMeshProUGUI speakerBox;
    private TextMeshProUGUI speechBox;
    private Conversation current;
    private PlayerController player;
    
    private PlayerController PlayerController
    {
        get
        {
            player = player ?? GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            return player;
        }
    }

    private void Awake()
    {
        speakerBox = transform.Find("Speaker").GetComponent<TextMeshProUGUI>();
        speechBox = transform.Find("Speech").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (current.HasNext())
            {
                DisplayDialogue(current.Next());
            }
            else
            {
                Close();
            }
        }
    }

    public void StartConversation(Conversation conversation)
    {
        Open();

        current = conversation;
        DisplayDialogue(current.Next());
    }

    private void DisplayDialogue(Dialogue dialogue)
    {
        speakerBox.text = dialogue.Speaker;
        speechBox.text = dialogue.Speech;
    }

    private void Open()
    {
        PlayerController.SetInputStatus(PlayerController.InputStatus.Blocked);
        gameObject.SetActive(true);
    }

    private void Close()
    {
        PlayerController.SetInputStatus(PlayerController.InputStatus.Default);
        gameObject.SetActive(false);
    }
}
