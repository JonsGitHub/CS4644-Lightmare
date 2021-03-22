using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

//[CreateAssetMenu(menuName = "State Machines/Conditions/Always False")]
public class AlwaysFalseConditionSO : StateConditionSO<AlwaysFalseCondition> { }

public class AlwaysFalseCondition : Condition
{
	protected override bool Statement() => false;
}
