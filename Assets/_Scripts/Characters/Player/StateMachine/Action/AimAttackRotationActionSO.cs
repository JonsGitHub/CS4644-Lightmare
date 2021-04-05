using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "AimAttackRotationAction", menuName = "State Machines/Actions/Aim Attack Rotation Action")]
public class AimAttackRotationActionSO : StateActionSO<AimAttackRotationAction>
{
	[Tooltip("Smoothing for rotating the character to their movement direction")]
	public float turnSmoothTime = 0.2f;

	public TransformAnchor _cameraTransformAnchor = default;

	[Header("Broadcasting on channels")]
	public BoolEventChannelSO _aimEventChannel = default;

	public TransformAnchor targetAnchor;
}

public class AimAttackRotationAction : StateAction
{
	//Component references
	private Transform _transform;
	private Transform _aimTarget;

	private Ray _ray;

	protected new AimAttackRotationActionSO OriginSO => (AimAttackRotationActionSO)base.OriginSO;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_transform = stateMachine.GetComponent<Transform>();

		var temp = GameObject.Find("Aiming Target");
		if (temp == null)
			temp = new GameObject("Aiming Target");
		
		_aimTarget = temp.transform;
	}

	public override void OnStateEnter()
	{
		// Update the camera focus
		OriginSO._aimEventChannel.RaiseEvent(true);

		RotateTowardsCameraAngle();
	}

    public override void OnStateExit()
    {
		// TODO: Find a better way then projecting a ray from main camera
		_ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
		_aimTarget.position = _ray.origin + _ray.direction * 100;
		
		OriginSO.targetAnchor.Transform = _aimTarget;

		// Update the camera focus
		OriginSO._aimEventChannel.RaiseEvent(false);
	}

    public override void OnUpdate()
	{
		RotateTowardsCameraAngle();
	}

	private void RotateTowardsCameraAngle()
	{
		var eulerAngles = _transform.eulerAngles;
		_transform.eulerAngles = new Vector3(eulerAngles.x, OriginSO._cameraTransformAnchor.Transform.rotation.eulerAngles.y, eulerAngles.z);
	}
}