using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Is Aim Attacking")]
public class IsAimAttackingConditionSO : StateConditionSO<IsAimAttacking> { }

public class IsAimAttacking : Condition
{
	//Component references
	private PlayerController _player;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_player = stateMachine.GetComponent<PlayerController>();
	}

	protected override bool Statement()
	{
		return _player.aimInput;
	}
}