using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;
using Moment = StateMachine.StateAction.SpecificMoment;

[CreateAssetMenu(fileName = "AnimatorMoveSpeedAction", menuName = "State Machines/Actions/Animator Move Speed Action")]
public class AnimatorMoveSpeedActionSO : StateActionSO
{
	public string parameterName = default;

	protected override StateAction CreateAction() => new AnimatorMoveSpeedAction(Animator.StringToHash(parameterName));
}

public class AnimatorMoveSpeedAction : StateAction
{
	//Component references
	private Animator _animator;
	private PlayerController _player;

	protected new AnimatorMoveSpeedActionSO OriginSO => (AnimatorMoveSpeedActionSO)base.OriginSO;
	private int _parameterHash;

	public AnimatorMoveSpeedAction(int parameterHash)
    {
		_parameterHash = parameterHash;
	}

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_animator = stateMachine.GetComponent<Animator>();
		_player = stateMachine.GetComponent<PlayerController>();
	}
	
	public override void OnUpdate()
	{
		//TODO: do we like that we're using the magnitude here, per frame? Can this be done in a smarter way?
		float normalisedSpeed = _player.movementInput.magnitude;
		_animator.SetFloat(_parameterHash, normalisedSpeed);
	}
}