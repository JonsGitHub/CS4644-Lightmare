using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
	[SerializeField] private InputReader _inputReader = default;
	[SerializeField] private DialogueManager _dialogueManager = default;

	[Header("Listening on channels")]
	[SerializeField] private PlayCutsceneChannelSO _playCutsceneEvent = default;
	[SerializeField] private DialogueLineChannelSO _playDialogueEvent = default;
	[SerializeField] private VoidEventChannelSO _pauseTimelineEvent = default;

	private CutsceneController _activeCutscene;
	private bool _isPaused;

	bool IsCutscenePlaying => _activeCutscene.Director.playableGraph.GetRootPlayable(0).GetSpeed() != 0d;

	private void OnEnable()
	{
		_inputReader.advanceDialogueEvent += OnAdvance;
	}

	private void OnDisable()
	{
		_inputReader.advanceDialogueEvent -= OnAdvance;
	}
	private void Start()
	{
		if (_playCutsceneEvent != null)
		{

			_playCutsceneEvent.OnEventRaised += PlayCutscene;

		}
		if (_playDialogueEvent != null)
		{

			_playDialogueEvent.OnEventRaised += PlayDialogueFromClip;

		}
		if (_pauseTimelineEvent != null)
		{

			_pauseTimelineEvent.OnEventRaised += PauseTimeline;

		}
	}
	void PlayCutscene(CutsceneController activeCutscene)
	{
		_inputReader.EnableDialogueInput();

		_activeCutscene = activeCutscene;
		_activeCutscene.StartUp(); // Prepare the cutscene before playing.

		_isPaused = false;
		_activeCutscene.Director.Play();
		_activeCutscene.Director.stopped += HandleDirectorStopped;
	}

	void CutsceneEnded()
	{
		if (_activeCutscene != null)
			_activeCutscene.Director.stopped -= HandleDirectorStopped;

		_activeCutscene.CleanUp(); // Clean up the cutscene after playing.

		_inputReader.EnableGameplayInput();
		_dialogueManager.DialogueEndedAndCloseDialogueUI();
	}

	private void HandleDirectorStopped(PlayableDirector director) => CutsceneEnded();

	void PlayDialogueFromClip(LocalizedString dialogueLine, ActorSO actor)
	{
		_dialogueManager.DisplayDialogueLine(dialogueLine, actor);
	}

	/// <summary>
	/// This callback is executed when the player presses the button to advance dialogues. If the Timeline is currently paused due to a <c>DialogueControlClip</c>, it will resume its playback.
	/// </summary>
	private void OnAdvance()
	{
		if (_isPaused)
			ResumeTimeline();
	}

	/// <summary>
	/// Called by <c>DialogueControlClip</c> on the Timeline.
	/// </summary>
	void PauseTimeline()
	{
		_isPaused = true;
		_activeCutscene.Director.playableGraph.GetRootPlayable(0).SetSpeed(0);
	}

	void ResumeTimeline()
	{
		_isPaused = false;
		_activeCutscene.Director.playableGraph.GetRootPlayable(0).SetSpeed(1);
	}
}