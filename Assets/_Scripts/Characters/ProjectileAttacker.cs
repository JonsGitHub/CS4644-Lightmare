using UnityEngine;

public class ProjectileAttacker : Attacker
{
	[SerializeField] private Transform _firePoint;
	[SerializeField] private GameObject _projectile;
	[SerializeField] private float _projectileSpeed;
	[SerializeField] private float _arcRange = 1f;

	[HideInInspector] public Vector3 Destination;

	public override void EnableWeapon()
	{
		InstantiateProjectile();
	}

	public override void DisableWeapon()
	{
	}

	private void InstantiateProjectile()
    {
		var projectile = Instantiate(_projectile, _firePoint.position, Quaternion.identity);
		projectile.GetComponent<Rigidbody>().velocity = (Destination - _firePoint.position).normalized * _projectileSpeed;
	}
}
