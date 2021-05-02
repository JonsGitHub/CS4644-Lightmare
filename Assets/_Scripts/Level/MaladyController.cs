using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class MaladyController : MonoBehaviour
{
    [SerializeField] private GameObject _minion;
    [SerializeField] private int _numberOfMinions = 2;
    [SerializeField] private NPCMovementConfigSO _movementConfig;
    [SerializeField] private WeaponAttacker _weaponAttacker;
    [SerializeField] private ProjectileAttacker _projectileAttacker;
    [SerializeField] private HealthBar3D _healthBar;
    [SerializeField] private GameObject _title;

    [SerializeField] private BoolEventChannelSO _finishedChannel = default;

    public List<Transform> _spawnPoints = new List<Transform>();

    public Transform Target;
    private NavMeshAgent _agent;
    private Animator _animator;
    private Damageable _damageable;

    private bool _inMeleeRange = false;
    private bool _inProjectileRange = false;

    private int _currentMinionCount = 0;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _damageable = GetComponent<Damageable>();
    }

    private HealthBar3D HealthBar
    {
        get
        {
            if (_healthBar == null)
            {
                _healthBar = GameObject.Find("Boss_HealthBar")?.GetComponent<HealthBar3D>();
                
                if (_healthBar)
                    _healthBar.MaxHealth = _damageable.MaxHealth;
            }
            return _healthBar;
        }
    }

    private void OnEnable()
    {
        // Not the best way but eh
        _title = GameObject.Find("Boss_Title");

        var _player = FindObjectOfType<PlayerController>();
        _player?.Targeted(transform, true);
        Target = _player?.transform;

        _damageable.OnDie += OnMaladyDeath;

        _animator.Play("Idle");

        _agent.speed = _movementConfig.Speed;
        _agent.stoppingDistance = 3;
    }

    private void OnDisable()
    {
        _damageable.OnDie -= OnMaladyDeath;
    }

    private Vector3 heightOffset = new Vector3(0, 1.5f, 0);

    public void EnableMeleeWeapon() => _weaponAttacker.EnableWeapon();
    public void DisableMeleeWeapon() => _weaponAttacker.DisableWeapon();
    public void EnableProjectileWeapon()
    {
        transform.LookAt(Target);
        _projectileAttacker.Destination = Target.position + heightOffset;
        _projectileAttacker.EnableWeapon();
    }

    public void CastSwordSpell()
    {
        for (; _currentMinionCount < _numberOfMinions; ++_currentMinionCount)
        {
            var minion = Instantiate(_minion, _spawnPoints[Random.Range(0, _spawnPoints.Count)].position, Quaternion.identity);
            minion.GetComponent<Damageable>().OnKilled += DeadMinion;
            minion.GetComponent<Aggressor>().Attacked(Target?.gameObject);
        }
    }

    private void DeadMinion(Damageable who)
    {
        _currentMinionCount--;
    }

    public void FinishCast()
    {
        summoning = false;
        summonCountDown = 0;
    }

    public void TriggerProjectileRange(bool entered, GameObject who)
    {
        _inProjectileRange = entered;
    }

    public void TriggerAttackRange(bool entered, GameObject who)
    {
        _inMeleeRange = entered;
    }

    private const float meleeCoolDown = 1.5f;
    private float meleeCountDown = 0;
    private const float projectileCoolDown = 4;
    private float projectileCountDown = 0;
    private const float summonCoolDown = 32;
    private float summonCountDown = 32;
    private bool summoning = false;

    private bool _isDead = false;
    private bool meleeAttacking = false;
    private bool projectileAttacking = false;

    private void Update()
    {
        if (Target == null || _isDead || summoning)
            return;

        if (!meleeAttacking && !projectileAttacking)
        {
            _agent.isStopped = false;
            _agent.SetDestination(Target.position);
            _animator.SetBool("IsWalking", _agent.velocity.magnitude > 0.01f);
        }

        if (_inProjectileRange)
        {
            if (_inMeleeRange && !meleeAttacking && meleeCountDown > meleeCoolDown)
            {
                _agent.isStopped = true;
                transform.LookAt(Target);
                StartCoroutine("MeleeAttackCoolDown");
                _animator.SetBool("IsAttacking", true);
            }
            else if (!_inMeleeRange && !projectileAttacking && projectileCountDown > projectileCoolDown)
            {
                // Chance projectile attack here otherwise keep moving closer to the target
                var rand = Random.Range(0, 11);
                if (rand > 0)
                {
                    _agent.isStopped = true;
                    _animator.SetTrigger("SpellTrigger");
                    StartCoroutine("ProjectileAttackCoolDown");
                }
                else
                {
                    projectileCountDown = 0;
                }
            }
            else if (_currentMinionCount == 0 && summonCountDown > summonCoolDown)
            {
                _agent.isStopped = true;
                summoning = true;
                _animator.SetTrigger("SummonTrigger");
            }
        }

        meleeCountDown += Time.deltaTime;
        projectileCountDown += Time.deltaTime;
        summonCountDown += Time.deltaTime;
    }

    private void LateUpdate()
    {
        if (HealthBar)
            HealthBar.Health = _damageable.CurrentHealth;
    }

    private void OnMaladyDeath()
    {
        Target.GetComponent<PlayerController>()?.Targeted(transform, false);

        _isDead = true;
        _agent.isStopped = true;
        _animator.SetTrigger("IsDead");

        _finishedChannel.RaiseEvent(true);

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy == gameObject)
                continue;

            Destroy(enemy);
        }

        FindObjectOfType<WaveSpawner>()?.FlagFinished(); // Flag the waves as done

        _title.SetActive(false);
        HealthBar?.SetActive(false);

        Destroy(gameObject, 6);
    }

    private IEnumerator MeleeAttackCoolDown()
    {
        meleeAttacking = true;
        yield return new WaitForSeconds(3.5f);
        meleeAttacking = false;
        meleeCountDown = 0;
        projectileCountDown = 0;
        _animator.SetBool("IsAttacking", false);
    }

    private IEnumerator ProjectileAttackCoolDown()
    {
        projectileAttacking = true;
        yield return new WaitForSeconds(3);
        projectileAttacking = false;
        projectileCountDown = 0;
    }
}
