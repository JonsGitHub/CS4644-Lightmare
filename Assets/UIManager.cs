using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class UIManager : MonoBehaviour
{
	[SerializeField]
	private UIDialogueManager _dialogueController = default;

	[SerializeField]
	private UIInteractionManager _interactionPanel = default;

	[SerializeField] private InputReader _inputReader = default;
	[Header("Listening on channels")]
	[Header("Dialogue Events")]
	[SerializeField] private DialogueLineChannelSO _openUIDialogueEvent = default;
	[SerializeField] private VoidEventChannelSO _closeUIDialogueEvent = default;

	[Header("Interaction Events")]
	[SerializeField] private VoidEventChannelSO _onInteractionEndedEvent = default;
	[SerializeField] private InteractionUIEventChannelSO _setInteractionEvent = default;

	private void OnEnable()
	{
		//Check if the event exists to avoid errors
		if (_openUIDialogueEvent != null)
		{
			_openUIDialogueEvent.OnEventRaised += OpenUIDialogue;
		}
		if (_closeUIDialogueEvent != null)
		{
			_closeUIDialogueEvent.OnEventRaised += CloseUIDialogue;
		}
		if (_setInteractionEvent != null)
		{
			_setInteractionEvent.OnEventRaised += SetInteractionPanel;
		}

	}

	private void Start()
	{
		CloseUIDialogue();
	}

	public void OpenUIDialogue(LocalizedString dialogueLine, ActorSO actor)
	{
		_dialogueController.SetDialogue(dialogueLine, actor);
		_dialogueController.gameObject.SetActive(true);
	}
	public void CloseUIDialogue()
	{

		_dialogueController.gameObject.SetActive(false);
	}

	public void SetInteractionPanel(bool isOpenEvent, InteractionType interactionType)
	{
		if (isOpenEvent)
		{
			_interactionPanel.FillInteractionPanel(interactionType);
		}
		_interactionPanel.gameObject.SetActive(isOpenEvent);

	}
}