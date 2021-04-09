using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "AttackAutoTargetAction", menuName = "State Machines/Actions/Auto Attack Action")]
public class AttackAutoTargetActionSO : StateActionSO<AttackAutoTargetAction>
{
	public TransformAnchor autoTargetAnchor;
}

public class AttackAutoTargetAction : StateAction
{
	//Component references
	private ParticleAttack _particleAttack;

	protected new AttackAutoTargetActionSO OriginSO => (AttackAutoTargetActionSO)base.OriginSO;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_particleAttack = stateMachine.GetComponentInChildren<ParticleAttack>();
	}

	public override void OnStateEnter()
    {
		RotateTowardsTarget();
	}

	public override void OnUpdate() 
	{
		RotateTowardsTarget();
	}

	private void RotateTowardsTarget()
    {
		if (OriginSO.autoTargetAnchor.isSet)
        {
			_particleAttack.transform.LookAt(OriginSO.autoTargetAnchor.Transform);
        }
		else
        {
			_particleAttack.transform.localEulerAngles = Vector3.zero;
        }
	}
}