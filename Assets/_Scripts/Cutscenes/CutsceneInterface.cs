using UnityEngine;

/// <summary>
/// Class to trigger a cutscene through an interface.
/// </summary>

public class CutsceneInterface : InterfaceBase
{
	[SerializeField] private CutsceneController _playableCutscene = default;
	[SerializeField] private bool _playOnStart = default;
	[SerializeField] private bool _playOnce = default;

	[Header("Broadcasting on channels")]
	[SerializeField] private PlayCutsceneChannelSO _playCutsceneEvent = default;
	[SerializeField] private InterfaceBaseEventChannelSO _removeInteraction = default;

	private void Start()
	{
		if (_playOnStart)
			PlayCutScene();
	}

	public void PlayCutScene()
	{
		_playCutsceneEvent?.RaiseEvent(_playableCutscene);

		if (_playOnce)
        {
			Destroy(gameObject);
        }
	}

    public override void Interact()
    {
		PlayCutScene();
	}

    private void OnDestroy()
    {
		_removeInteraction.RaiseEvent(this);
	}
}