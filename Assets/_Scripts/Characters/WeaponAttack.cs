using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
	[SerializeField] private AttackConfigSO _attackConfigSO;

	public AttackConfigSO AttackConfig => _attackConfigSO;

	private void Awake()
	{
		gameObject.SetActive(false);
	}

	private void OnTriggerEnter(Collider other)
	{
		// Avoid friendly fire!
		if (!other.CompareTag(gameObject.tag))
		{
			if (other.TryGetComponent(out Damageable damageableComp))
			{
				if (!damageableComp.GetHit)
					damageableComp.ReceiveAnAttack(_attackConfigSO.AttackStrength);
			}
			else if (other.gameObject.TryGetComponent(out DamageablePart part))
			{
				part.ReceiveAnAttack(_attackConfigSO.AttackStrength);
			}
		}
	}
}