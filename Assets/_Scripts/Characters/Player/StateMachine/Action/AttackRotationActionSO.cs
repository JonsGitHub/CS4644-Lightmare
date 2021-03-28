using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "FaceCameraAction", menuName = "State Machines/Actions/Face Camera Action")]
public class AttackRotationActionSO : StateActionSO<AttackRotationAction>
{
	[Tooltip("Smoothing for rotating the character to their movement direction")]
	public float turnSmoothTime = 0.2f;

	[Tooltip("Angle offset from the particle firing origin to cursor.")]
	public float angleOffset = 7;

	public TransformAnchor _cameraTransformAnchor = default;
}

public class AttackRotationAction : StateAction
{
	//Component references
	private ParticleAttack _particleAttack;
	private Transform _transform;

	protected new AttackRotationActionSO OriginSO => (AttackRotationActionSO)base.OriginSO;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_particleAttack = stateMachine.GetComponentInChildren<ParticleAttack>();
		_transform = stateMachine.GetComponent<Transform>();
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
		var eulerAngles = _transform.eulerAngles;
		_transform.eulerAngles = new Vector3(eulerAngles.x, OriginSO._cameraTransformAnchor.Transform.rotation.eulerAngles.y, eulerAngles.z);

		eulerAngles = _particleAttack.gameObject.transform.eulerAngles;
		_particleAttack.gameObject.transform.eulerAngles = new Vector3(OriginSO._cameraTransformAnchor.Transform.rotation.eulerAngles.x + OriginSO.angleOffset, eulerAngles.y, eulerAngles.z);
	}
}