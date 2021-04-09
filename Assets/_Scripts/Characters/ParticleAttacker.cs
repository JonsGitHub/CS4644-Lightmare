using UnityEngine;

public class ParticleAttacker : Attacker
{
	[SerializeField] private ParticleSystem _attackSystem;
	[SerializeField] private Animator _supplementalAnimation;

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

	public void PlayActivation()
    {
		_supplementalAnimation?.Play("Activate");
    }

	public override void DisableWeapon()
	{
		_attackSystem.Stop();
	}
}