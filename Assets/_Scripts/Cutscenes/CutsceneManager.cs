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
	[SerializeField] private VoidEventChannelSO _unpauseTimelineEvent = default;

	[Header("Broadcasting on")]
	[SerializeField] private ScreenStateEventChannelSO _screenEventChannel;

	private CutsceneController _activeCutscene;
	private bool _isPaused;
	private bool _isPlaying;

	public bool IsCutscenePlaying => _isPlaying;

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
		if (_unpauseTimelineEvent != null)
        {
			_unpauseTimelineEvent.OnEventRaised += ResumeTimeline;
        }
	}

	void PlayCutscene(CutsceneController activeCutscene)
	{
		_isPlaying = true;

		if (!activeCutscene.FreeMovement)
			_inputReader.EnableDialogueInput();

		_activeCutscene = activeCutscene;
		_activeCutscene.StartUp(); // Prepare the cutscene before playing.

		_isPaused = false;
		_activeCutscene.Director.Play();
		_activeCutscene.Director.stopped += HandleDirectorStopped;

		_screenEventChannel?.RaiseEvent(ScreenState.Cutscene);
	}

	void CutsceneEnded()
	{
		if (_activeCutscene != null)
			_activeCutscene.Director.stopped -= HandleDirectorStopped;

		_activeCutscene.CleanUp(); // Clean up the cutscene after playing.

		_screenEventChannel?.RaiseEvent(ScreenState.EndCutscene);
		_inputReader.EnableGameplayInput();
		_dialogueManager.DialogueEndedAndCloseDialogueUI();
		
		_isPlaying = false;
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
		if (_isPlaying)
        {
			_isPaused = true;
			_activeCutscene.Director.playableGraph.GetRootPlayable(0).SetSpeed(0);
        }
	}

	void ResumeTimeline()
	{
		if (_isPlaying)
		{
			_isPaused = false;
			_activeCutscene.Director.playableGraph.GetRootPlayable(0).SetSpeed(1);
		}
	}
}