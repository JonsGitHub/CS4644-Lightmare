using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "FaceTarget", menuName = "State Machines/Actions/Face Protagonist")]
public class FaceTargetSO : StateActionSO<FaceTarget>
{
	public TransformAnchor targetAnchor;
}

public class FaceTarget : StateAction
{
	TransformAnchor _target;
	Transform _actor;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_actor = stateMachine.transform;
		_target = ((FaceTargetSO)OriginSO).targetAnchor;
	}

	public override void OnUpdate()
	{
		if (_target.isSet)
		{
			Vector3 relativePos = _target.Transform.position - _actor.position;
			relativePos.y = 0f; // Force rotation to be only on Y axis.

			Quaternion rotation = Quaternion.LookRotation(relativePos);
			_actor.rotation = rotation;
		}
	}
}