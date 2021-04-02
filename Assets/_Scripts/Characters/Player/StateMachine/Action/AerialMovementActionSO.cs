using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "AerialMovementAction", menuName = "State Machines/Actions/Aerial Movement Action")]
public class AerialMovementActionSO : StateActionSO
{
	public float Speed => _speed;
	public float Acceleration => _acceleration;

	[Tooltip("Desired horizontal movement speed while in the air")]
	[SerializeField] [Range(0.1f, 100f)] private float _speed = 10f;
	[Tooltip("The acceleration applied to reach the desired speed")]
	[SerializeField] [Range(0.1f, 100f)] private float _acceleration = 20f;

	protected override StateAction CreateAction() => new AerialMovementAction();
}

public class AerialMovementAction : StateAction
{
	protected new AerialMovementActionSO OriginSO => (AerialMovementActionSO)base.OriginSO;

	private PlayerController _player;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_player = stateMachine.GetComponent<PlayerController>();
	}

	private int counter = 0;
	private Vector3 prevInput = Vector3.zero;
	private const int numBlockingFrames = 10;
	
	public override void OnUpdate()
	{
		Vector3 velocity = _player.movementVector;
		Vector3 input = _player.movementInput;

		// Attempt to stop fast brake in air when moving forward and tapping back once
		if (counter > 0)
		{
			input = Vector3.zero;
			counter--;
		}
		else if (Vector3.Dot(input, prevInput) <= 0) // Opposing input vectors facing opposite or far left/right direction
		{
			counter = numBlockingFrames;
			prevInput = input;
		}

		float speed = OriginSO.Speed;
		float acceleration = OriginSO.Acceleration;

		SetVelocityPerAxis(ref velocity.x, input.x, acceleration, speed);
		SetVelocityPerAxis(ref velocity.z, input.z, acceleration, speed);

		_player.movementVector = velocity;
	}

	private void SetVelocityPerAxis(ref float currentAxisSpeed, float axisInput, float acceleration, float targetSpeed)
	{
		if (axisInput == 0f)
		{
			if (currentAxisSpeed != 0f)
			{
				ApplyAirResistance(ref currentAxisSpeed);
			}
		}
		else
		{
			(float absVel, float absInput) = (Mathf.Abs(currentAxisSpeed), Mathf.Abs(axisInput));
			(float signVel, float signInput) = (Mathf.Sign(currentAxisSpeed), Mathf.Sign(axisInput));
			targetSpeed *= absInput;

			if (signVel != signInput || absVel < targetSpeed)
			{
				currentAxisSpeed += axisInput * acceleration * Time.deltaTime;
				currentAxisSpeed = Mathf.Clamp(currentAxisSpeed, -targetSpeed, targetSpeed);
			}
			else
			{
				ApplyAirResistance(ref currentAxisSpeed);
			}
		}
	}

	private void ApplyAirResistance(ref float value)
	{
		float sign = Mathf.Sign(value);

		value -= sign * PlayerController.AIR_RESISTANCE * Time.deltaTime;
		if (Mathf.Sign(value) != sign)
			value = 0;
	}
}