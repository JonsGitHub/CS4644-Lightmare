using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "RaiseVoidEventAction", menuName = "State Machines/Actions/Raise Void Event Action")]
public class RaiseVoidEventActionSO : StateActionSO
{
	public VoidEventChannelSO voidEvent;

	protected override StateAction CreateAction() => new RaiseVoidEventAction();
}

public class RaiseVoidEventAction : StateAction
{
	private VoidEventChannelSO _voidEvent;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_voidEvent = ((RaiseVoidEventActionSO)OriginSO).voidEvent;
	}

	public override void OnUpdate() { }
	
	public override void OnStateEnter()
	{
		_voidEvent.RaiseEvent();
	}
	public override void OnStateExit()
	{
	}
}