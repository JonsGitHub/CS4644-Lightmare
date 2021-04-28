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
		_player.movementVector.y += Physics.gravity.y * Time.deltaTime;
	}
	
	public override void OnStateEnter()
	{
		_player.movementVector.y += Mathf.Sqrt(OriginSO.initialJumpForce * -PlayerController.GRAVITY_MULTIPLIER * Physics.gravity.y);
	}
}