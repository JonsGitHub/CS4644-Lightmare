using System.Collections;
using UnityEngine;

public class Aggressor : MonoBehaviour
{
	[HideInInspector] public bool isPlayerInAlertZone;
	[HideInInspector] public bool isPlayerInAttackZone;
	public Damageable currentTarget; //The StateMachine evaluates its health when needed\
	
	private bool resetHealth = false;

	public virtual void FoundTarget() { }
    
	public void OnAlertTriggerChange(bool entered, GameObject who)
	{
		if (entered && isPlayerInAlertZone)
			return; // Attacked outside of alert zone so ignore entering

		isPlayerInAlertZone = entered;
		if (who.TryGetComponent(out Damageable d))
        {
			if (entered)
			{
				resetHealth = false;

				currentTarget = d;
				currentTarget.OnDie += OnTargetDead;
				FoundTarget();
			}
			else
			{
				currentTarget = null;
				if (TryGetComponent(out Damageable damageable))
                {
					resetHealth = true;
					StartCoroutine(ResetHealthDelayed(damageable));
                }
			}
		}
	}

	private IEnumerator ResetHealthDelayed(Damageable damageable, float delay = 3f)
    {
		yield return new WaitForSeconds(delay);

		if (resetHealth)
			damageable.ResetHealth();
    }

	public void OnAttackTriggerChange(bool entered, GameObject who)
	{
		isPlayerInAttackZone = entered;

		//No need to set the target. If we did, we would get currentTarget to null even if
		//a target exited the Attack zone (inner) but stayed in the Alert zone (outer).
	}

	public void Attacked(GameObject who)
    {
		if (who == null || isPlayerInAlertZone)
			return;

		isPlayerInAlertZone = true;	
		if (who.TryGetComponent(out Damageable damageable))
        {
			resetHealth = false;

			currentTarget = damageable;
			currentTarget.OnDie += OnTargetDead;
			FoundTarget();
		}
	}

	private void OnTargetDead()
	{
		currentTarget = null;
		isPlayerInAlertZone = false;
		isPlayerInAttackZone = false;
	}
}