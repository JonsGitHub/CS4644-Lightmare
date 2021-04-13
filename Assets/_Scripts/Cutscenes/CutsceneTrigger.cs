using UnityEngine;

/// <summary>
/// Class to trigger a cutscene.
/// </summary>

public class CutsceneTrigger : MonoBehaviour
{
	[SerializeField] private CutsceneController _playableCutscene = default;
	[SerializeField] private LayerMask _layers = default;
	[SerializeField] private bool _playOnSceneStart = default;
	[SerializeField] private bool _playOnce = default;

	[Header("Listening on channels")]
	[SerializeField] private VoidEventChannelSO _sceneReadyEventChannel = default;

	[Header("Broadcasting on channels")]
	[SerializeField] private PlayCutsceneChannelSO _playCutsceneEvent = default;

    private void OnEnable()
    {
		if (_sceneReadyEventChannel)
			_sceneReadyEventChannel.OnEventRaised += SceneReady;
    }

    private void OnDisable()
    {
		if (_sceneReadyEventChannel)
			_sceneReadyEventChannel.OnEventRaised -= SceneReady;
	}

	private void SceneReady()
    {
		if (_playOnSceneStart)
			PlayCutScene();
	}

	private void OnTriggerEnter(Collider other)
	{
		if ((1 << other.gameObject.layer & _layers) != 0)
		{
			PlayCutScene();
		}
	}

	private void PlayCutScene()
    {
		_playCutsceneEvent?.RaiseEvent(_playableCutscene);

		if (_playOnce)
			Destroy(this);
	}
}