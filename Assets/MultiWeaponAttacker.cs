using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultiWeaponAttacker : MonoBehaviour
{
	public List<GameObject> _attackColliders = new List<GameObject>();
	[SerializeField] private GameObject _supplemental;

	public void EnableWeapon(int index)
	{
		_attackColliders.ElementAtOrDefault(index)?.SetActive(true);
		_supplemental?.SetActive(true);
	}

	public void DisableWeapon(int index)
	{
		_attackColliders.ElementAtOrDefault(index)?.SetActive(false);
		_supplemental?.SetActive(false);
	}
}
