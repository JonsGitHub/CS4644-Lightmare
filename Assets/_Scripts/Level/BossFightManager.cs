using UnityEngine;

public class BossFightManager : MonoBehaviour
{
    [SerializeField] private Damageable _bossDamageable;
    [SerializeField] private HealthBar3D _healthBar;
    [SerializeField] private Spawner _minionSpawner;
    [SerializeField] private GameObject _pillar1;
    [SerializeField] private GameObject _pillar2;
    [SerializeField] private GameObject _pillar3;
    [SerializeField] private Transform _respawnReposition;
    [SerializeField] private TendrilsManager _tendrilsLocator;
    [SerializeField] private Animator _misfortune;

    [SerializeField] private TransformEventChannelSO _playerRespawning;

    private bool _startedFight = false;

    private void OnEnable()
    {
        if (_playerRespawning)
            _playerRespawning.OnEventRaised += RestartFight;
    }

    private void OnDisable()
    {
        if (_playerRespawning)
            _playerRespawning.OnEventRaised -= RestartFight;
    }

    private void RestartFight(Transform playerLookPoint)
    {
        if (_startedFight)
        {
            StartFight();
        }
    }

    private void OnPlayerDeath()
    {
        // Player has died so clean up the minions
        _minionSpawner.StopSpawning(true);

        _bossDamageable.ResetHealth();
    }

    public void StartFight()
    {
        _startedFight = true;

        GameObject.FindGameObjectWithTag("Player").GetComponent<Damageable>().OnDie += OnPlayerDeath;

        foreach(var point in GameObject.FindGameObjectsWithTag("SpawnLocation"))
        {
            point.transform.position = _respawnReposition.position;
            point.transform.rotation = _respawnReposition.rotation;
        }

        _bossDamageable.ResetHealth();
        _healthBar.MaxHealth = _bossDamageable.MaxHealth;
        Destroy(_pillar1);
        Destroy(_pillar2);
        Destroy(_pillar3);

        _misfortune.Play("Misfortune_Idle");
        _minionSpawner.gameObject.SetActive(true);
        _minionSpawner.StartSpawning();
    }

    private const float attackCoolDown = 6f;
    private float currentTime = 0;
    private bool attacking = false;

    private void LateUpdate()
    {
        if (!_startedFight)
            return;

        _healthBar.Health = _bossDamageable.CurrentHealth;
        
        if (_bossDamageable.IsDead)
        {
            _minionSpawner.StopSpawning(true);

            // Trigger Final Cutscene
            Debug.Log("Killed Misfortune!");
        }

        if (!attacking)
        {
            if (currentTime >= attackCoolDown)
            {
                _misfortune.Play("Misfortune_Attack_" + (_tendrilsLocator.CurrentZone + 1));
                _tendrilsLocator.AttackingZone(_tendrilsLocator.CurrentZone);
                attacking = true;
                currentTime = 0;
            }
            currentTime += Time.deltaTime;
        }
    }

    public void FinishedAttack()
    {
        attacking = false;
        _tendrilsLocator.Clear();
    }
}
