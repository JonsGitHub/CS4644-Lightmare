using UnityEngine;

public class ParticleAttacker : Attacker
{
	[SerializeField] private ParticleSystem _attackSystem;

    private void Awake()
	{
		_attackSystem.Stop();
	}

    private void OnDestroy()
    {
		_attackSystem.Stop();
	}

	public override void EnableWeapon()
	{
		_attackSystem.Play();
	}

	public override void DisableWeapon()
	{
		_attackSystem.Stop();
	}
}