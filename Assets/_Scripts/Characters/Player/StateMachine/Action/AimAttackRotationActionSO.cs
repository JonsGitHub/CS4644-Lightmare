using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "AimAttackRotationAction", menuName = "State Machines/Actions/Aim Attack Rotation Action")]
public class AimAttackRotationActionSO : StateActionSO<AimAttackRotationAction>
{
	public TransformAnchor _cameraTransformAnchor = default;
}

public class AimAttackRotationAction : StateAction
{
	//Component references
	private Transform _transform;
	private PlayerController _player;

	protected new AimAttackRotationActionSO OriginSO => (AimAttackRotationActionSO)base.OriginSO;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_transform = stateMachine.GetComponent<Transform>();
		_player = stateMachine.GetComponent<PlayerController>();
	}

	public override void OnStateEnter()
	{
		RotateTowardsCameraAngle();
	}

    public override void OnUpdate()
	{
		RotateTowardsCameraAngle();
	}

	private void RotateTowardsCameraAngle()
	{
		var angles = _player._followTarget.transform.rotation;

		var eulerAngles = _transform.eulerAngles;
		_transform.eulerAngles = new Vector3(eulerAngles.x, OriginSO._cameraTransformAnchor.Transform.rotation.eulerAngles.y, eulerAngles.z);
	
		_player._followTarget.transform.rotation = angles;
	}
}