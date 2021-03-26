using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
	[SerializeField] private AudioCueEventChannelSO _sfxEventChannel = default;
	[SerializeField] private AudioConfigurationSO _audioConfig = default;

	[SerializeField] private AudioCueSO footstep;

	public void PlayFootstep() => _sfxEventChannel.RaisePlayEvent(footstep, _audioConfig, transform.position);
}