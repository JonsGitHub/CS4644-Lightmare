using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
	[SerializeField] private VoidEventChannelSO _onSceneReady = default;
	[SerializeField] private AudioCueEventChannelSO _playMusicOn = default;
	[SerializeField] private GameSceneSO _thisSceneSO = default;
	[SerializeField] private AudioConfigurationSO _audioConfig = default;
	[SerializeField] private bool _playOnce = false;

	private void OnEnable()
	{
		_onSceneReady.OnEventRaised += PlayMusic;
	}

	private void OnDisable()
	{
		_onSceneReady.OnEventRaised -= PlayMusic;
	}

	private bool _played = false;

	private void PlayMusic()
	{
		if (!_played)
			_playMusicOn.RaisePlayEvent(_thisSceneSO.musicTrack, _audioConfig);
		
		if (_playOnce)
        {
			_played = true;
		}
	}
}
