using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "RotateAction", menuName = "State Machines/Actions/Rotate Action")]
public class RotateActionSO : StateActionSO<RotateAction>
{
	[Tooltip("Smoothing for rotating the character to their movement direction")]
	public float turnSmoothTime = 0.2f;
}

public class RotateAction : StateAction
{
	//Component references
	private PlayerController _player;
	private Transform _transform;

	private float _turnSmoothSpeed; //Used by Mathf.SmoothDampAngle to smoothly rotate the character to their movement direction
	private const float ROTATION_TRESHOLD = .02f; // Used to prevent NaN result causing rotation in a non direction

	protected new RotateActionSO OriginSO => (RotateActionSO)base.OriginSO;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_player = stateMachine.GetComponent<PlayerController>();
		_transform = stateMachine.GetComponent<Transform>();
	}
	
	public override void OnUpdate()
	{
		Vector3 horizontalMovement = _player.movementVector;
		horizontalMovement.y = 0f;

		if (horizontalMovement.sqrMagnitude >= ROTATION_TRESHOLD)
		{
			var angles = _player._followTarget.transform.rotation;

			float targetRotation = Mathf.Atan2(_player.movementVector.x, _player.movementVector.z) * Mathf.Rad2Deg;
			_transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(_transform.eulerAngles.y, targetRotation, ref _turnSmoothSpeed, OriginSO.turnSmoothTime);
			
			_player._followTarget.transform.rotation = angles;
		}
	}
}