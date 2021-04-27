using UnityEngine;

public class KIller : MonoBehaviour
{
	public void OnTriggerChangeDetected(bool entered, GameObject obj)
	{
		Debug.Log("Entered " + obj);
		if (entered && obj.TryGetComponent(out Damageable damageable))
        {
			Debug.Log("Damageable " + damageable);
			damageable.Kill();
        }
	}
}
