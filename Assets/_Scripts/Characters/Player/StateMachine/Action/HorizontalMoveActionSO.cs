using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "HorizontalMoveAction", menuName = "State Machines/Actions/Horizontal Move Action")]
public class HorizontalMoveActionSO : StateActionSO<HorizontalMoveAction>
{
	[Tooltip("Horizontal XZ plane speed multiplier")]
	public float speed = 8f;
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
		_player.movementVector.x = _player.movementInput.x * OriginSO.speed;
		_player.movementVector.z = _player.movementInput.z * OriginSO.speed;
	}
}