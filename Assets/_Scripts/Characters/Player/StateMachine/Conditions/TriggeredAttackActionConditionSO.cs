using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Trigger Attack Action")]
public class TriggeredAttackActionConditionSO : StateConditionSO<TriggeredAttackAction> { }

public class TriggeredAttackAction : Condition
{
	//Component references
	private PlayerController _player;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_player = stateMachine.GetComponent<PlayerController>();
	}

	protected override bool Statement()
	{
		if (_player.attackInput)
        {
			_player.attackInput = false;
			return true;
        }
		return false;
	}
}