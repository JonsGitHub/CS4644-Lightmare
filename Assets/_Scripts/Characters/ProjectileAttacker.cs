using UnityEngine;

public class ProjectileAttacker : Attacker
{
	[SerializeField] private Transform _firePoint;
	[SerializeField] private ProjectileAttack _projectile;
	[HideInInspector] public Vector3 Destination;

	public override void EnableWeapon()
	{
		InstantiateProjectile();
	}

	public override void DisableWeapon() { }

	private void InstantiateProjectile() => Instantiate(_projectile, _firePoint.position, Quaternion.identity).Fire(Destination);
}
