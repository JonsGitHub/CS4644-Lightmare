using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.Assertions;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    //[SerializeField] private BoolEventChannelSO _unlockingChannel = default;
    [SerializeField] private BoolEventChannelSO _finalwaveChannel = default;
    [SerializeField] private TransformEventChannelSO _frameObjectChannel = default;

    [SerializeField] private TextMeshProUGUI _waveCounter = default;
    [SerializeField] private TextMeshProUGUI _enemiesCounter = default;
    [SerializeField] private GameObject _bossHealthBar = default;
    [SerializeField] private GameObject _bossTitle = default;
    [SerializeField] private Animator _maladyEntrance = default;
    [SerializeField] private GameObject _maladyProp = default;
 
    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    [Serializable]
    public class Wave
    {
        public string name;
        public GameObject gSlime;
        public GameObject beholder;
        public GameObject oSlime;
        public GameObject wolf;
        public GameObject zombie;
        public GameObject bossEnemy;
        public int gSlimeCount;
        public int beholderCount;
        public int oSlimeCount;
        public int wolfCount;
        public int zombieCount;
        public int bossCount;
        public float rate;
    }

    private int totalEnemiesCount;
    private bool _newGame = false;

    public Wave[] waves;
    private int nextWave = 0;

    public Transform[] groundSpawnPoints;
    public Transform[] rangedSpawnPoints;
    private List<Transform> availablePoints;

    public float timeBetweenWaves = 5.0f;
    private float waveCountdown;

    private PlayerController _player;

    public SpawnState state = SpawnState.COUNTING;

    private bool _finished = false;

    public bool Finished => _finished;

    public void FlagFinished()
    {
        _maladyEntrance.SetBool("IsFighting", true);
        _finished = true;
    }

    void Start()
    {
        Wave _wave = waves[nextWave];
        totalEnemiesCount = _wave.gSlimeCount + _wave.beholderCount +
            _wave.oSlimeCount + _wave.wolfCount + _wave.zombieCount + _wave.bossCount;

        waveCountdown = timeBetweenWaves;

        Assert.IsTrue(groundSpawnPoints.Length != 0);
        Assert.IsTrue(rangedSpawnPoints.Length != 0);
        availablePoints = new List<Transform>(rangedSpawnPoints);

        _waveCounter.text = "Starting Wave";
    }

    private void OnEnable()
    {
        if (_frameObjectChannel != null)
            _frameObjectChannel.OnEventRaised += RestartWaves;

        _player = FindObjectOfType<PlayerController>();
    }

    private void OnDisable()
    {
        if (_frameObjectChannel != null)
            _frameObjectChannel.OnEventRaised -= RestartWaves;
    }

    void Update()
    {
        if (_newGame)
        {
            _newGame = false;
        }
        if (state == SpawnState.WAITING)
        {
            // Check if enemies are still alive
            if (!EnemyIsAlive())
            {
                // Start a new wave
                StartNextWave();
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                if (nextWave == 4)
                {
                    state = SpawnState.SPAWNING; // Freeze on spawning state
                    _finalwaveChannel?.RaiseEvent(true);
                    nextWave++;
                    _waveCounter.text = "";
                    _maladyEntrance.SetBool("IsFighting", true);
                    return;
                }
                else 
                {
                    // Start spawning wave
                    StartCoroutine("SpawnWave");
                }
            }
        }
        else
        {
            if (nextWave == 0)
            {
                _waveCounter.text = "First Wave in " + waveCountdown.ToString("0");
            }
            else if (nextWave + 1 == waves.Length)
            {
                _waveCounter.text = "Final Wave in " + waveCountdown.ToString("0");
            }
            else
            {
                _waveCounter.text = "Next Wave in " + waveCountdown.ToString("0");
            }
            waveCountdown -= Time.deltaTime;
        }
    }

    void StartNextWave()
    {
        _enemiesCounter.text = "";

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        //if (nextWave + 1 > waves.Length - 1)
        //{
        //    _waveCounter.gameObject.SetActive(false);
        //    _enemiesCounter.gameObject.SetActive(false);

        //    _finished = true;
        //    _unlockingChannel?.RaiseEvent(true);
        //    Destroy(gameObject);
        //}
        //else
        //{
            nextWave++;
            Wave _wave = waves[nextWave];
            totalEnemiesCount = _wave.gSlimeCount + _wave.beholderCount +
                _wave.oSlimeCount + _wave.wolfCount + _wave.zombieCount + _wave.bossCount;
        //}
    }

    // Perhaps tie into the on death trigger of enemies instead of polling count.
    bool EnemyIsAlive()
    {
        if (totalEnemiesCount == 0)
        {
            return false;
        }
        return true;
    }

    public void DecrementEnemy(Damageable script)
    {
        totalEnemiesCount--;
        _enemiesCounter.text = "Enemies Remaining:\n" + totalEnemiesCount;
    }

    public void RestartWaves(Transform transform)
    {
        StopCoroutine("SpawnWave");
        nextWave = 0;

        _player = FindObjectOfType<PlayerController>();

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }

        Wave _wave = waves[nextWave];
        totalEnemiesCount = _wave.gSlimeCount + _wave.beholderCount +
            _wave.oSlimeCount + _wave.wolfCount + _wave.zombieCount + _wave.bossCount;

        _finalwaveChannel?.RaiseEvent(false);
        _maladyEntrance.SetBool("IsFighting", false);
        _bossHealthBar.SetActive(false);
        _bossTitle.SetActive(false);
        _waveCounter.text = "Restarting Waves";
        _enemiesCounter.text = null;
        _maladyProp.SetActive(true);

        waveCountdown = timeBetweenWaves;

        state = SpawnState.COUNTING;

        _newGame = true;
    }

    IEnumerator SpawnWave()
    {
        Wave _wave = waves[nextWave];

        _waveCounter.text = "Wave: " + (nextWave + 1);
        _enemiesCounter.text = "Enemies Remaining:\n" + totalEnemiesCount;

        state = SpawnState.SPAWNING;
        availablePoints = new List<Transform>(rangedSpawnPoints);
        
        //Spawn green slimes enemy
        for (int i = 0; i < _wave.gSlimeCount; i++)
        {
            SpawnEnemy(_wave.gSlime);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        //Spawn wolf enemy
        for (int i = 0; i < _wave.wolfCount; i++)
        {
            SpawnEnemy(_wave.wolf);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        //Spawn beholder enemy
        for (int i = 0; i < _wave.beholderCount; i++)
        {
            SpawnRangedEnemy(_wave.beholder);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        //Spawn orange slime enemy
        for (int i = 0; i < _wave.oSlimeCount; i++)
        {
            SpawnEnemy(_wave.oSlime);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        //Spawn zombie enemy
        for (int i = 0; i < _wave.zombieCount; i++)
        {
            SpawnEnemy(_wave.zombie);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        //Spawn boss enemy
        for (int i = 0; i < _wave.bossCount; i++)
        {
            SpawnEnemy(_wave.bossEnemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy(GameObject _enemy)
    {
        // Enemy
        var _sp = groundSpawnPoints[Random.Range(0, groundSpawnPoints.Length)];
        var _e = Instantiate(_enemy, _sp.position, _sp.rotation);
        _e.GetComponent<Damageable>().OnKilled += DecrementEnemy;

        if (_e.TryGetComponent(out Aggressor aggressor) && _player != null)
            aggressor.Attacked(_player?.gameObject);
    }

    void SpawnRangedEnemy(GameObject _enemy)
    {
        // Enemy
        int point = Random.Range(0, availablePoints.Count);
        var _sp = availablePoints[point];
        availablePoints.RemoveAt(point);
        var _e = Instantiate(_enemy, _sp.position, _sp.rotation);
        _e.GetComponent<Damageable>().OnKilled += DecrementEnemy;

        if (_e.TryGetComponent(out Aggressor aggressor) && _player != null)
            aggressor.Attacked(_player?.gameObject);
    }
}
