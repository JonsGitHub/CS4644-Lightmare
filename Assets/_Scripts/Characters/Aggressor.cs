using UnityEngine;

public class Aggressor : MonoBehaviour
{
	[HideInInspector] public bool isPlayerInAlertZone;
	[HideInInspector] public bool isPlayerInAttackZone;
	public Damageable currentTarget; //The StateMachine evaluates its health when needed

	public void OnAlertTriggerChange(bool entered, GameObject who)
	{
		if (isPlayerInAlertZone)
			return; // Attacked outside of alert zone so ignore entering

		isPlayerInAlertZone = entered;

		if (entered && who.TryGetComponent(out Damageable d))
		{
			currentTarget = d;
			currentTarget.OnDie += OnTargetDead;
		}
		else
		{
			currentTarget = null;
		}
	}

	public void OnAttackTriggerChange(bool entered, GameObject who)
	{
		isPlayerInAttackZone = entered;

		//No need to set the target. If we did, we would get currentTarget to null even if
		//a target exited the Attack zone (inner) but stayed in the Alert zone (outer).
	}

	public void Attacked(GameObject who)
    {
		if (isPlayerInAlertZone)
			return;

		isPlayerInAlertZone = true;	
		if (who.TryGetComponent(out Damageable damageable))
        {
			currentTarget = damageable;
			currentTarget.OnDie += OnTargetDead;
		}
	}

	private void OnTargetDead()
	{
		currentTarget = null;
		isPlayerInAlertZone = false;
		isPlayerInAttackZone = false;
	}
}