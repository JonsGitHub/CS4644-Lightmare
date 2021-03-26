using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Actions/Play AudioCue")]
public class PlayAudioCueActionSO : StateActionSO<PlayAudioCueAction>
{
	public AudioCueSO audioCue = default;
	public AudioCueEventChannelSO audioCueEventChannel = default;
	public AudioConfigurationSO audioConfiguration = default;
}

public class PlayAudioCueAction : StateAction
{
	private Transform _stateMachineTransform;

	private new PlayAudioCueActionSO OriginSO => (PlayAudioCueActionSO)base.OriginSO;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_stateMachineTransform = stateMachine.transform;
	}

	public override void OnUpdate() { }

	public override void OnStateEnter()
	{
		OriginSO.audioCueEventChannel.RaisePlayEvent(OriginSO.audioCue, OriginSO.audioConfiguration, _stateMachineTransform.position);
	}
}