using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Started Moving")]
public class IsMovingConditionSO : StateConditionSO<IsMovingCondition>
{
	public float treshold = 0.02f;
}

public class IsMovingCondition : Condition
{
	private PlayerController _player;
	private new IsMovingConditionSO OriginSO => (IsMovingConditionSO)base.OriginSO;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_player = stateMachine.GetComponent<PlayerController>();
	}

	protected override bool Statement()
	{
		Vector3 movementVector = _player.movementInput;
		movementVector.y = 0f;
		return movementVector.sqrMagnitude > OriginSO.treshold;
	}
}