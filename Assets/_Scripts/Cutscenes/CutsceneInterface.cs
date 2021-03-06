using UnityEngine;

/// <summary>
/// Class to trigger a cutscene through an interface.
/// </summary>

public class CutsceneInterface : InterfaceBase
{
	[SerializeField] private CutsceneController _playableCutscene = default;
	[SerializeField] private bool _playOnStart = default;
	[SerializeField] private bool _playOnce = default;
	[SerializeField] private Transform _reposition = default;

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
		if (_reposition != null)
		{
			var _player = GameObject.FindObjectOfType<PlayerController>();

			_player.GetComponent<CharacterController>().enabled = false;
			_player.transform.position = _reposition.position;
			_player.transform.rotation = _reposition.rotation;
			_player.GetComponent<CharacterController>().enabled = true;
		}
		
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