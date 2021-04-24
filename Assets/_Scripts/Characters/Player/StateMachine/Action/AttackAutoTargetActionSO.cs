using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "AttackAutoTargetAction", menuName = "State Machines/Actions/Auto Attack Action")]
public class AttackAutoTargetActionSO : StateActionSO<AttackAutoTargetAction>
{
	public LayerMask layerMask;
	public TransformAnchor autoTargetAnchor;
}

public class AttackAutoTargetAction : StateAction
{
	//Component references
	private ProjectileAttacker projectileAttacker;
	private PlayerController _player;
	private Ray _ray;
	private RaycastHit _hit;

	protected new AttackAutoTargetActionSO OriginSO => (AttackAutoTargetActionSO)base.OriginSO;

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		projectileAttacker = stateMachine.GetComponent<ProjectileAttacker>();
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
		if (_player.aimInput)
        {
			_ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
			if (Physics.Raycast(_ray, out _hit, OriginSO.layerMask))
            {
				projectileAttacker.Destination = _hit.point;
            }
			else
            {
				projectileAttacker.Destination = _ray.GetPoint(1000);
            }
        }
		else if (OriginSO.autoTargetAnchor.isSet)
        {
			projectileAttacker.Destination = OriginSO.autoTargetAnchor.Transform.position;
        }
		else
        {
			projectileAttacker.Destination = _player.transform.forward * 1000;
        }
	}
}