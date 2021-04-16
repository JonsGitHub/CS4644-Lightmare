using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsSliding", menuName = "State Machines/Conditions/Is Sliding")]
public class IsSlidingConditionSO : StateConditionSO<IsSlidingCondition> { }

public class IsSlidingCondition : Condition
{
	private CharacterController _characterController;
	private PlayerController _player;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_characterController = stateMachine.GetComponent<CharacterController>();
		_player = stateMachine.GetComponent<PlayerController>();
	}

	protected override bool Statement()
	{
		return false;

		//if (_player.lastHit == null)
		//	return false;

		//float currentSlope = Vector3.Angle(Vector3.up, _player.lastHit.normal);
		//return (currentSlope >= _characterController.slopeLimit);
	}
}