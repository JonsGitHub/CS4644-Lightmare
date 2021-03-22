using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

/// <summary>
/// An Action to clear a <see cref="PlayerController.movementVector"/> at the <see cref="StateAction.SpecificMoment"/> <see cref="StopMovementActionSO.Moment"/>
/// </summary>
[CreateAssetMenu(fileName = "StopMovementAction", menuName = "State Machines/Actions/Stop Movement Action")]
public class StopMovementActionSO : StateActionSO
{
	[SerializeField] private StateAction.SpecificMoment _moment = default;
	public StateAction.SpecificMoment Moment => _moment;

	protected override StateAction CreateAction() => new StopMovementAction();
}

public class StopMovementAction : StateAction
{
	private PlayerController _player;

	protected new StopMovementActionSO OriginSO => (StopMovementActionSO)base.OriginSO;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_player = stateMachine.GetComponent<PlayerController>();
	}

	public override void OnUpdate()
	{
		if (OriginSO.Moment == SpecificMoment.OnUpdate)
			_player.movementVector = Vector3.zero;
	}

	public override void OnStateEnter()
	{
		if (OriginSO.Moment == SpecificMoment.OnStateEnter)
			_player.movementVector = Vector3.zero;
	}

	public override void OnStateExit()
	{
		if (OriginSO.Moment == SpecificMoment.OnStateExit)
			_player.movementVector = Vector3.zero;
	}
}