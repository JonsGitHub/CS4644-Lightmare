using UnityEngine;

public class WeaponAttacker : Attacker
{
	[SerializeField] private GameObject _attackCollider;

	public override void EnableWeapon()
	{
		_attackCollider.SetActive(true);
	}

	public override void DisableWeapon()
	{
		_attackCollider.SetActive(false);
	}
}