using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "ApplyMovementVectorAction", menuName = "State Machines/Actions/Apply Movement Vector Action")]
public class ApplyMovementVectorActionSO : StateActionSO<ApplyMovementVectorAction>
{
}

public class ApplyMovementVectorAction : StateAction
{
	//Component references
	private PlayerController _player;
	private CharacterController _characterController;

	protected new ApplyMovementVectorActionSO OriginSO => (ApplyMovementVectorActionSO)base.OriginSO;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_player = stateMachine.GetComponent<PlayerController>();
		_characterController = stateMachine.GetComponent<CharacterController>();
	}
	
	public override void OnUpdate()
	{
		_characterController.Move(_player.movementVector * Time.deltaTime);
		_player.movementVector = _characterController.velocity;
	}
}