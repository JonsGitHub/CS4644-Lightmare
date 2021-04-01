using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "SlideAction", menuName = "State Machines/Actions/Slide Action")]
public class SlideActionSO : StateActionSO<SlideAction> { }

public class SlideAction : StateAction
{
	private PlayerController _player;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_player = stateMachine.GetComponent<PlayerController>();
	}

	public override void OnUpdate()
	{
		Vector3 velocity = _player.movementVector;
		float speed = -Physics.gravity.y * PlayerController.GRAVITY_MULTIPLIER * Time.deltaTime;

		Vector3 hitNormal = _player.lastHit.normal;
		var vector = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
		Vector3.OrthoNormalize(ref hitNormal, ref vector);

		// Cheap way to avoid overshooting the character, which causes it to move away from the slope
		if (Mathf.Sign(vector.x) == Mathf.Sign(velocity.x))
			vector.x *= 0.5f;
		if (Mathf.Sign(vector.z) == Mathf.Sign(velocity.z))
			vector.z *= 0.5f;

		velocity += vector * speed;

		_player.movementVector = velocity;
	}
}