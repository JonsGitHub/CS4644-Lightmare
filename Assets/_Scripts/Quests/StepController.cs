using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// this script needs to be put on the actor, and takes care of the current step to accomplish.
/// the step contains a dialogue and maybe an event.
/// </summary>
public class StepController : MonoBehaviour
{
	[Header("Data")]
	[SerializeField] private ActorSO _actor = default;
	[SerializeField] private DialogueDataSO _defaultDialogue = default;
	[SerializeField] private QuestAnchorSO _questAnchor = default;

	[Header("Listening to channels")]
	[SerializeField] private DialogueActorChannelSO _interactionEvent = default;
	[SerializeField] private VoidEventChannelSO _winDialogueEvent = default;
	[SerializeField] private VoidEventChannelSO _loseDialogueEvent = default;

	[Header("Broadcasting on channels")]
	[SerializeField] private DialogueDataChannelSO _startDialogueEvent = default;

	private DialogueDataSO _currentDialogue;

	public LocalizedString Name => _actor.ActorName;

	private void Start()
	{
		if (_winDialogueEvent)
		{ 
			_winDialogueEvent.OnEventRaised += PlayWinDialogue; 
		}
		if (_loseDialogueEvent)
		{ 
			_loseDialogueEvent.OnEventRaised += PlayLoseDialogue; 
		}
		if (_interactionEvent)
        {
			_interactionEvent.OnEventRaised += PlayInteractionEvent;
        }
	}

	void PlayDefaultDialogue()
	{

		if (_defaultDialogue != null)
		{
			_currentDialogue = _defaultDialogue;
			StartDialogue();
		}

	}

	/// <summary>
	/// start a dialogue when interaction some Steps need to be instantanious. 
	/// And do not need the interact button. When interaction again, restart same dialogue.
	/// </summary>
	public void InteractWithCharacter()
	{
		DialogueDataSO displayDialogue = _questAnchor.InteractWithCharacter(_actor, false, false);
		if (displayDialogue != null)
		{
			_currentDialogue = displayDialogue;
			StartDialogue();
		}
		else
		{
			PlayDefaultDialogue();
		}
	}

	void StartDialogue()
	{
		if (_startDialogueEvent != null)
		{
			_startDialogueEvent.RaiseEvent(_currentDialogue);
		}
	}

	private void PlayLoseDialogue()
	{
		if (_questAnchor != null)
		{
			DialogueDataSO displayDialogue = _questAnchor.InteractWithCharacter(_actor, true, false);
			if (displayDialogue != null)
			{
				_currentDialogue = displayDialogue;
				StartDialogue();
			}

		}
	}

	private void PlayWinDialogue()
	{
		if (_questAnchor != null)
		{
			DialogueDataSO displayDialogue = _questAnchor.InteractWithCharacter(_actor, true, true);
			if (displayDialogue != null)
			{
				_currentDialogue = displayDialogue;
				StartDialogue();
			}
		}
	}

	private void PlayInteractionEvent(ActorSO actorSO)
    {
		// TODO: determine if this should be implemented here
    }
}