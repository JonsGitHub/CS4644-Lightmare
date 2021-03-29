using UnityEngine;

/// <summary>
/// Class to trigger a cutscene.
/// </summary>

public class CutsceneTrigger : MonoBehaviour
{
	[SerializeField] private CutsceneController _playableCutscene = default;
	[SerializeField] private bool _playOnStart = default;
	[SerializeField] private bool _playOnce = default;

	[Header("Broadcasting on channels")]
	[SerializeField] private PlayCutsceneChannelSO _playCutsceneEvent = default;

	private void Start()
	{
		if (_playOnStart)
			PlayCutScene();
	}

	private void OnTriggerEnter(Collider other)
	{
		PlayCutScene();
	}

	private void PlayCutScene()
    {
		_playCutsceneEvent?.RaiseEvent(_playableCutscene);

		if (_playOnce)
			Destroy(this);
	}
}