using UnityEngine;

public class Destroyer : MonoBehaviour
{
	public void OnTriggerChangeDetected(bool entered, GameObject obj)
	{
		if (entered)
			Destroy(obj);
	}
}
