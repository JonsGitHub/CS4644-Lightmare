using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Actions/Play Jump Particles")]
public class PlayJumpParticlesActionSO : StateActionSO<PlayJumpParticlesAction> { }

public class PlayJumpParticlesAction : StateAction
{
	//Component references
	private PlayerEffectController _dustController;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_dustController = stateMachine.GetComponent<PlayerEffectController>();
	}

	public override void OnStateEnter()
	{
		_dustController.PlayJumpParticles();
	}

	public override void OnUpdate() { }
}
