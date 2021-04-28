using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "PlayerFaceTarget", menuName = "State Machines/Actions/Player Face Enemy")]
public class PlayerFaceTargetSO : StateActionSO<PlayerFaceTarget>
{
	public TransformAnchor targetAnchor;
}

public class PlayerFaceTarget : StateAction
{
	TransformAnchor _target;
	Transform _actor;
	PlayerController _player;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_actor = stateMachine.transform;
		_player = stateMachine.GetComponent<PlayerController>();
		_target = ((PlayerFaceTargetSO)OriginSO).targetAnchor;
	}

	public override void OnUpdate()
	{
		if (_target.isSet)
		{
			var prevRotation = _player._followTarget.rotation;
			Vector3 relativePos = _target.Transform.position - _actor.position;
			relativePos.y = 0f; // Force rotation to be only on Y axis.

			Quaternion rotation = Quaternion.LookRotation(relativePos);
			_actor.rotation = rotation;
			_player._followTarget.rotation = prevRotation;
		}
	}
}