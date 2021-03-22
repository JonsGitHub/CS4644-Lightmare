using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "GroundGravityAction", menuName = "State Machines/Actions/Ground Gravity Action")]
public class GroundGravityActionSO : StateActionSO<GroundGravityAction>
{
	[Tooltip("Vertical movement pulling down the player to keep it anchored to the ground.")]
	public float verticalPull = -5f;
}

public class GroundGravityAction : StateAction
{
	//Component references
	private PlayerController _player;

	protected new GroundGravityActionSO OriginSO => (GroundGravityActionSO)base.OriginSO;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_player = stateMachine.GetComponent<PlayerController>();
	}

	public override void OnUpdate()
	{
		_player.movementVector.y = OriginSO.verticalPull;
	}
}