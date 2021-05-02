using System.Linq;
using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// Takes care of all things dialogue, whether they are coming from within a Timeline or just from the interaction with a character, or by any other mean.
/// Keeps track of choices in the dialogue (if any) and then gives back control to gameplay when appropriate.
/// </summary>
public class DialogueManager : MonoBehaviour
{
	//	[SerializeField] private ChoiceBox _choiceBox; // TODO: Demonstration purpose only. Remove or adjust later.

	[SerializeField] private InputReader _inputReader = default;
	[SerializeField] private CutsceneManager _cutsceneManager = default;

	private int _counter = 0;
	private bool _reachedEndOfDialogue { get => _counter >= _currentDialogue?.DialogueLines.Count; }
	
	[Header("Voice Audio Fields")]
	[SerializeField] private AudioConfigurationSO _audioConfig = default;

	[Header("Listening on channels")]
	[SerializeField] private DialogueDataChannelSO _startDialogue = default;

	[Header("BoradCasting on channels")]
	[SerializeField] private DialogueLineChannelSO _openUIDialogueEvent = default;
	[SerializeField] private DialogueDataChannelSO _endDialogue = default;
	[SerializeField] private AudioCueEventChannelSO _voiceEventChannel = default;
	//[SerializeField] private VoidEventChannelSO _continueWithStep = default; // For choice dialogues
	[SerializeField] private VoidEventChannelSO _closeDialogueUIEvent = default;

	private DialogueDataSO _currentDialogue = default;

	public bool PlayingDialogue => _currentDialogue != null;

	private void Start()
	{
		if (_startDialogue != null)
		{
			_startDialogue.OnEventRaised += DisplayDialogueData;
		}
	}

	/// <summary>
	/// Displays DialogueData in the UI, one by one.
	/// </summary>
	/// <param name="dialogueDataSO"></param>
	public void DisplayDialogueData(DialogueDataSO dialogueDataSO)
	{
		BeginDialogueData(dialogueDataSO);
		if (_currentDialogue != null)
        {
			var current = _currentDialogue.DialogueLines[_counter];
			DisplayDialogueLine(current.Line, current.Actor, current.Audio);
        }
	}

	/// <summary>
	/// Prepare DialogueManager when first time displaying DialogueData. 
	/// <param name="dialogueDataSO"></param>
	private void BeginDialogueData(DialogueDataSO dialogueDataSO)
	{
		_counter = 0;
		_inputReader.EnableDialogueInput();
		_inputReader.advanceDialogueEvent += OnAdvance;
		_currentDialogue = dialogueDataSO;
	}

	/// <summary>
	/// Displays a line of dialogue in the UI, by requesting it to the <c>DialogueManager</c>.
	/// This function is also called by <c>DialogueBehaviour</c> from clips on Timeline during cutscenes.
	/// </summary>
	/// <param name="dialogueLine"></param>
	public void DisplayDialogueLine(LocalizedString dialogueLine, ActorSO actor, AudioCueSO audio = null)
	{
		if (_openUIDialogueEvent != null)
		{
			_openUIDialogueEvent.RaiseEvent(dialogueLine, actor);
		}
		if (_voiceEventChannel !=  null && audio != null)
        {
			_voiceEventChannel.RaisePlayEvent(audio, _audioConfig);
        }
	}

	private void OnAdvance()
	{
		_counter++;

		if (_currentDialogue?.DialogueLines.Last().Audio is AudioCueSO audio)
			_voiceEventChannel?.RaiseStopEvent(AudioCueKey.Invalid);

		if (!_reachedEndOfDialogue)
		{
			var current = _currentDialogue.DialogueLines[_counter];
			DisplayDialogueLine(current.Line, current.Actor);
		}
		else
		{
			DialogueEndedAndCloseDialogueUI();
		}
	}

	public void DialogueEndedAndCloseDialogueUI()
	{
		if (_endDialogue != null)
			_endDialogue.RaiseEvent(_currentDialogue);
		if (_closeDialogueUIEvent != null)
			_closeDialogueUIEvent.RaiseEvent();

		_inputReader.advanceDialogueEvent -= OnAdvance;

		if (!_cutsceneManager.IsCutscenePlaying)
			_inputReader.EnableGameplayInput();

		_currentDialogue = null;
	}
}