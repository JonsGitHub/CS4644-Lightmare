using UnityEngine;

public class ManualMusicPlayer : MonoBehaviour
{
	[SerializeField] private AudioCueEventChannelSO _playMusicOn = default;
	[SerializeField] private AudioConfigurationSO _audioConfig = default;
	[SerializeField] private AudioCueSO _track;

	public void PlayMusic()
	{
		_playMusicOn.RaisePlayEvent(_track, _audioConfig);
	}
}
