using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "HorizontalMoveAction", menuName = "State Machines/Actions/Horizontal Move Action")]
public class HorizontalMoveActionSO : StateActionSO<HorizontalMoveAction>
{
	[Tooltip("Horizontal XZ plane speed multiplier")]
	public float speed = 8f;

	[Tooltip("Horizontal XZ plane run speed multiplier")]
	public float runSpeed = 15f;
}

public class HorizontalMoveAction : StateAction
{
	//Component references
	private PlayerController _player;

	protected new HorizontalMoveActionSO OriginSO => (HorizontalMoveActionSO)base.OriginSO;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_player = stateMachine.GetComponent<PlayerController>();
	}

	public override void OnUpdate()
	{
		_player.movementVector.x = _player.movementInput.x;
		_player.movementVector.z = _player.movementInput.z;
		
		_player.movementVector.y = 0;
		_player.movementVector.Normalize();

		_player.movementVector *= _player.isRunning ? OriginSO.runSpeed : OriginSO.speed;
	}
}