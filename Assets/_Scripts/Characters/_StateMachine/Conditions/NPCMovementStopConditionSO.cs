using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/NPC Movement Stop Elapsed")]
public class NPCMovementStopConditionSO : StateConditionSO<NPCMovementStopCondition>
{

}

public class NPCMovementStopCondition : Condition
{
	private float _startTime;
	private NPCController _npcMovement;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_npcMovement = stateMachine.GetComponent<NPCController>();
	}

	public override void OnStateEnter()
	{
		_startTime = Time.time;
	}

	protected override bool Statement() => Time.time >= _startTime + _npcMovement.NPCMovementConfig.StopDuration;
}