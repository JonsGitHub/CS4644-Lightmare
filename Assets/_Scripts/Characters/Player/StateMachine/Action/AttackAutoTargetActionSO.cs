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
	private PlayerController _player;
	private Ray _ray;

	protected new AttackAutoTargetActionSO OriginSO => (AttackAutoTargetActionSO)base.OriginSO;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_particleAttack = stateMachine.GetComponentInChildren<ParticleAttack>();
		_player = stateMachine.GetComponent<PlayerController>();
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
		if (_player.aimAttackInput)
        {
			// TODO: Look into better way of rotating the fire point origin to match offset camera
			_ray = Camera.main.ViewportPointToRay(new Vector3(0.52F, 0.5F, 0));
			_particleAttack.transform.LookAt(_ray.GetPoint(200));
        }
		else if (OriginSO.autoTargetAnchor.isSet)
        {
			_particleAttack.transform.LookAt(OriginSO.autoTargetAnchor.Transform);
        }
		else
        {
			_particleAttack.transform.localEulerAngles = Vector3.zero;
        }
	}
}