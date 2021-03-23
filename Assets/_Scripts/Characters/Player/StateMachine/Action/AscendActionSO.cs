using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "AscendAction", menuName = "State Machines/Actions/Ascend Action")]
public class AscendActionSO : StateActionSO<AscendAction>
{
	[Tooltip("The initial upwards push when pressing jump. This is injected into verticalMovement, and gradually cancelled by gravity")]
	public float initialJumpForce = 6f;
}

public class AscendAction : StateAction
{
	//Component references
	private PlayerController _player;

	private float _verticalMovement;
	private float _gravityContributionMultiplier;

	protected new AscendActionSO OriginSO => (AscendActionSO)base.OriginSO;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_player = stateMachine.GetComponent<PlayerController>();
	}
	
	public override void OnUpdate()
	{
		_gravityContributionMultiplier += PlayerController.GRAVITY_COMEBACK_MULTIPLIER;
		_gravityContributionMultiplier *= PlayerController.GRAVITY_DIVIDER; //Reduce the gravity effect
		_verticalMovement += Physics.gravity.y * PlayerController.GRAVITY_MULTIPLIER * Time.deltaTime * _gravityContributionMultiplier;
		//Note that even if it's added, the above value is negative due to Physics.gravity.y

		_player.movementVector.y = _verticalMovement;
	}
	
	public override void OnStateEnter()
	{
		_verticalMovement = OriginSO.initialJumpForce;
	}
}