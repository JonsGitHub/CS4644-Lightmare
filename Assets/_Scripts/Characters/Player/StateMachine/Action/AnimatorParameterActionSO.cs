using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;
using Moment = StateMachine.StateAction.SpecificMoment;

[CreateAssetMenu(fileName = "AnimatorParameterAction", menuName = "State Machines/Actions/Animator Parameter Action")]
public class AnimatorParameterActionSO : StateActionSO
{
	public ParameterType parameterType = default;
	public string parameterName = default;

	public bool boolValue = default;
	public int intValue = default;
	public float floatValue = default;

	public Moment whenToRun = default; // Allows this StateActionSO type to be reused for all 3 state moments

	protected override StateAction CreateAction() => new AnimatorParameterAction(Animator.StringToHash(parameterName));

	public enum ParameterType
	{
		Bool, Int, Float, Trigger,
	}
}

public class AnimatorParameterAction : StateAction
{
	//Component references
	private Animator _animator;

	protected new AnimatorParameterActionSO OriginSO => (AnimatorParameterActionSO)base.OriginSO;
	private int _parameterHash;

	public AnimatorParameterAction(int parameterHash)
	{
		_parameterHash = parameterHash;
	}

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_animator = stateMachine.GetComponent<Animator>();
	}

	public override void OnStateEnter()
	{
		if (OriginSO.whenToRun == SpecificMoment.OnStateEnter)
			SetParameter();
	}
	
	public override void OnStateExit()
	{
		if (OriginSO.whenToRun == SpecificMoment.OnStateExit)
			SetParameter();
	}

	public override void OnUpdate() { }

	private void SetParameter()
	{
		switch (OriginSO.parameterType)
		{
			case AnimatorParameterActionSO.ParameterType.Bool:
				_animator.SetBool(_parameterHash, OriginSO.boolValue);
				break;
			case AnimatorParameterActionSO.ParameterType.Int:
				_animator.SetInteger(_parameterHash, OriginSO.intValue);
				break;
			case AnimatorParameterActionSO.ParameterType.Float:
				_animator.SetFloat(_parameterHash, OriginSO.floatValue);
				break;
			case AnimatorParameterActionSO.ParameterType.Trigger:
				_animator.SetTrigger(_parameterHash);
				break;
		}
	}
}