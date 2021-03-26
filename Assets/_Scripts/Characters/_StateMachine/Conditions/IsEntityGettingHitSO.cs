using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;


[CreateAssetMenu(fileName = "IsEntityGettingHit", menuName = "State Machines/Conditions/Is Entity Getting Hit")]
public class IsEntityGettingHitSO : StateConditionSO
{
	protected override Condition CreateCondition() => new IsEntityGettingHit();
}

public class IsEntityGettingHit : Condition
{

	private Damageable _damageable;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_damageable = stateMachine.GetComponent<Damageable>();
	}

	protected override bool Statement()
	{
		bool result = false;
		if (_damageable != null)
		{
			result = _damageable.GetHit;
		}
		return result;
	}
}