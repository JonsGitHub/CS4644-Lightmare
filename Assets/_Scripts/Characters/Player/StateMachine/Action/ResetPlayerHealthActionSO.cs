using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "ResetPlayerHealthAction", menuName = "State Machines/Actions/Reset Player Health Action")]
public class ResetPlayerHealthActionSO : StateActionSO<ResetPlayerHealthAction>
{
}

public class ResetPlayerHealthAction : StateAction
{
	protected new ResetPlayerHealthActionSO OriginSO => (ResetPlayerHealthActionSO)base.OriginSO;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		var damageable = stateMachine.GetComponent<Damageable>();
		PlayerData.SetHealth(damageable.MaxHealth);
		PlayerData.Save();
	}
	
	public override void OnUpdate()
	{
	}
}