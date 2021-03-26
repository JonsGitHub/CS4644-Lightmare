using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

public enum ZoneType
{
	Alert,
	Attack
}

[CreateAssetMenu(fileName = "PlayerIsInZone", menuName = "State Machines/Conditions/Player Is In Zone")]
public class PlayerIsInZoneSO : StateConditionSO
{
	public ZoneType zone;

	protected override Condition CreateCondition() => new PlayerIsInZone();
}

public class PlayerIsInZone : Condition
{
	private Aggressor _aggressor;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_aggressor = stateMachine.GetComponent<Aggressor>();
	}

	protected override bool Statement()
	{
		bool result = false;
		if (_aggressor != null)
		{
			switch (((PlayerIsInZoneSO)OriginSO).zone)
			{
				case ZoneType.Alert:
					result = _aggressor.isPlayerInAlertZone;
					break;
				case ZoneType.Attack:
					result = _aggressor.isPlayerInAttackZone;
					break;
				default:
					break;
			}
		}
		return result;
	}
}