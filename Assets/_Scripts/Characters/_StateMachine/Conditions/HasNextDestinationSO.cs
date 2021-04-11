using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "HsNextDestination", menuName = "State Machines/Conditions/Hs Next Destination")]
public class HasNextDestinationSO : StateConditionSO<HasNextDestination>
{
}

public class HasNextDestination : Condition
{
	protected new HasNextDestinationSO OriginSO => (HasNextDestinationSO)base.OriginSO;

	private NPCMovementAction _action;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_action = stateMachine.GetComponent<NPCController>()._currentAction;
	}
	
	protected override bool Statement() => _action.HasNextAction;
}