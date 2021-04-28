using UnityEngine;

public class Killer : MonoBehaviour
{
	public void OnTriggerChangeDetected(bool entered, GameObject obj)
	{
		if (entered && obj.TryGetComponent(out Damageable damageable))
        {
			damageable.Kill();
        }
	}
}
