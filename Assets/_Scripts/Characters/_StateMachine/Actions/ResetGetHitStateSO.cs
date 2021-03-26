using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "ResetGetHitState", menuName = "State Machines/Actions/Reset Get Hit State")]
public class ResetGetHitStateSO : StateActionSO
{
	protected override StateAction CreateAction() => new ResetGetHitState();
}

public class ResetGetHitState : StateAction
{
	private Damageable _damageable;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_damageable = stateMachine.GetComponent<Damageable>();
	}

	public override void OnUpdate()
	{

	}

	public override void OnStateExit()
	{
		_damageable.GetHit = false;
	}
}