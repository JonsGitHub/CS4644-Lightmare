using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class DialogueBehaviour : PlayableBehaviour
{
	// NOTE: This Dialogue behaviour seems to only currently work consistently with one line of dialogue per clip

	//[SerializeField] private LocalizedString _dialogueLine = default;
	//[SerializeField] private ActorSO _actor = default;

	[SerializeField] private DialogueDataSO _dialogueDataSO = default;

	[SerializeField] private bool _pauseWhenClipEnds = default; //This won't work if the clip ends on the very last frame of the Timeline

	[HideInInspector] public DialogueDataChannelSO _startDialogue = default;
	//[HideInInspector] public DialogueLineChannelSO PlayDialogueEvent;
	[HideInInspector] public VoidEventChannelSO PauseTimelineEvent;
	private VoidEventChannelSO _closeDialogueUIEvent = default;
	private bool _dialoguePlayed;

	/// <summary>
	/// Displays a line of dialogue on screen by interfacing with the <c>CutsceneManager</c>. 
	/// </summary>
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		if (_dialoguePlayed)
			return;

		if (Application.isPlaying)  //TODO: Find a way to "play" dialogue lines even when scrubbing the Timeline not in Play Mode
		{
			// Need to ask the CutsceneManager if the cutscene is playing, since the graph is not actually stopped/paused: it's just going at speed 0.
			if (playable.GetGraph().IsPlaying())
			//&& cutsceneManager.IsCutscenePlaying) Need to find an alternative to this 
			{
				if (_dialogueDataSO != null)
				{
					if (_startDialogue != null)
						_startDialogue.RaiseEvent(_dialogueDataSO);
					_dialoguePlayed = true;
				}
				else
				{
					Debug.LogWarning("This clip contains no DialogueLine");
				}
			}

			if (playable.GetGraph().IsDone() && PauseTimelineEvent.GetType().Equals(_closeDialogueUIEvent))
            {
				_closeDialogueUIEvent.RaiseEvent();
			}
		}
	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		// The check on _dialoguePlayed is needed because OnBehaviourPause is called also at the beginning of the Timeline,
		// so we need to make sure that the Timeline has actually gone through this clip (i.e. called OnBehaviourPlay) at least once before we stop it
		if (Application.isPlaying
			&& playable.GetGraph().IsPlaying()
			&& !playable.GetGraph().GetRootPlayable(0).IsDone()
			&& _dialoguePlayed)
		{
			if (_pauseWhenClipEnds)
				if (PauseTimelineEvent != null)
				{
					PauseTimelineEvent.OnEventRaised();
				}
		}
	}
}