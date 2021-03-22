using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "DescendAction", menuName = "State Machines/Actions/Descend Action")]
public class DescendActionSO : StateActionSO<DescendAction>
{
}

public class DescendAction : StateAction
{
	//Component references
	private PlayerController _player;
	private float _verticalMovement;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_player = stateMachine.GetComponent<PlayerController>();
	}

	public override void OnUpdate()
	{
		_verticalMovement += Physics.gravity.y * PlayerController.GRAVITY_MULTIPLIER * Time.deltaTime;
		//Note that even if it's added, the above value is negative due to Physics.gravity.y

		//Cap the maximum so the player doesn't reach incredible speeds when freefalling from high positions
		_verticalMovement = Mathf.Clamp(_verticalMovement, PlayerController.MAX_FALL_SPEED, PlayerController.MAX_RISE_SPEED);

		_player.movementVector.y = _verticalMovement;
	}
	
	public override void OnStateEnter()
	{
		_verticalMovement = _player.movementVector.y;

		//Prevents a double jump if the player keeps holding the jump button
		//Basically it "consumes" the input
		_player.jumpInput = false;
	}
}