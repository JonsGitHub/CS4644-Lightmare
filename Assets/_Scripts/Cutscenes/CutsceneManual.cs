using UnityEngine;

/// <summary>
/// Class to manually trigger a cutscene.
/// </summary>

public class CutsceneManual : MonoBehaviour
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

	public void PlayCutScene()
    {
		_playCutsceneEvent?.RaiseEvent(_playableCutscene);

		if (_playOnce)
			Destroy(this);
	}
}