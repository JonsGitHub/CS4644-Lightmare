using UnityEngine;

public class DialogueStarter : MonoBehaviour
{
    [SerializeField] private DialogueDataSO _defaultDialogue = default;

    [Header("Broadcasting on channels")]
    [SerializeField] private DialogueDataChannelSO _startDialogueEvent = default;

    [Header("Listening to channels")]
    [SerializeField] private VoidEventChannelSO _triggerEventChannel = default;

    private void OnEnable()
    {
        if (_triggerEventChannel)
        {
            _triggerEventChannel.OnEventRaised += StartDialogue;
        }
    }

    private void OnDisable()
    {
        if (_triggerEventChannel)
        {
            _triggerEventChannel.OnEventRaised -= StartDialogue;
        }
    }

    private void StartDialogue()
    {
        if (_startDialogueEvent)
        {
            _startDialogueEvent.RaiseEvent(_defaultDialogue);
        }
    }
}
