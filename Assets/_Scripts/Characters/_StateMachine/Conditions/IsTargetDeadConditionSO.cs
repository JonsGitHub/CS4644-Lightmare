using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsTargetDeadCondition", menuName = "State Machines/Conditions/Is Target Dead Condition")]
public class IsTargetDeadConditionSO : StateConditionSO
{
	protected override Condition CreateCondition() => new IsTargetDeadCondition();
}

public class IsTargetDeadCondition : Condition
{
	private Aggressor _aggressor;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_aggressor = stateMachine.GetComponent<Aggressor>();
	}

	protected override bool Statement()
	{
		return _aggressor.currentTarget == null || _aggressor.currentTarget.IsDead;
	}
}