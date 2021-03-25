using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Class to trigger a cutscene.
/// </summary>

public class CutsceneTrigger : MonoBehaviour
{
	[SerializeField] private PlayableDirector _playableDirector = default;
	[SerializeField] private bool _playOnStart = default;
	[SerializeField] private bool _playOnce = default;

	[Header("Broadcasting on channels")]
	[SerializeField] private PlayableDirectorChannelSO _playCutsceneEvent = default;

	private void Start()
	{
		if (_playOnStart)
        {
			PlayCutScene();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		PlayCutScene();
	}

	private void PlayCutScene()
    {
		_playCutsceneEvent?.RaiseEvent(_playableDirector);

		if (_playOnce)
		{
			Destroy(this);
		}
	}
}