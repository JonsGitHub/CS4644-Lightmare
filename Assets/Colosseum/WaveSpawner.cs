using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.Assertions;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private BoolEventChannelSO _unlockingChannel = default;

    [SerializeField] private TextMeshProUGUI _waveCounter = default;
    [SerializeField] private TextMeshProUGUI _enemiesCounter = default;

    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    [Serializable]
    public class Wave
    {
        public string name;
        public Transform easyEnemy;
        public Transform mediumEnemy;
        public Transform hardEnemy;
        public Transform bossEnemy;
        public int easyCount;
        public int mediumCount;
        public int hardCount;
        public int bossCount;
        public float rate;

        public int TotalEnemiesCount => easyCount + mediumCount + hardCount + bossCount;
    }

    public Wave[] waves;
    private int nextWave = 0;

    public Transform[] groundSpawnPoints;
    public Transform[] rangedSpawnPoints;
    private List<Transform> availablePoints;

    public float timeBetweenWaves = 5.0f;
    private float waveCountdown;

    private float searchCountdown = 1.0f;

    public SpawnState state = SpawnState.COUNTING;

    private bool _finished = false;

    public bool Finished => _finished;

    public void FlagFinished() => _finished = true;

    void Start()
    {
        waveCountdown = timeBetweenWaves;

        Assert.IsTrue(groundSpawnPoints.Length != 0);
        Assert.IsTrue(rangedSpawnPoints.Length != 0);
        availablePoints = new List<Transform>(rangedSpawnPoints);

        _waveCounter.text = "Starting Wave";
    }

    void Update()
    {
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
                // Start spawning wave
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    void StartNextWave()
    {
        //Debug.Log("Wave Completed");

        _waveCounter.text = "Starting Next Wave";
        _enemiesCounter.text = "";

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            _waveCounter.gameObject.SetActive(false);
            _enemiesCounter.gameObject.SetActive(false);

            _finished = true;
            _unlockingChannel?.RaiseEvent(true);
            Destroy(gameObject);
        }
        else
        {
            nextWave++;
        }
    }

    // Perhaps tie into the on death trigger of enemies instead of polling count.
    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0.0f)
        {
            searchCountdown = 1.0f;
            var enemiesCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            
            _enemiesCounter.text = "Enemies Remaining:\n" + enemiesCount;
            if (enemiesCount == 0)
                return false;
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        _waveCounter.text = "Wave: " + (nextWave + 1);
        _enemiesCounter.text = "Enemies Remaining:\n" + _wave.TotalEnemiesCount;

        state = SpawnState.SPAWNING;
        availablePoints = new List<Transform>(rangedSpawnPoints);

        //Spawn easy enemy
        for (int i = 0; i < _wave.easyCount; i++)
        {
            SpawnEnemy(_wave.easyEnemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        //Spawn medium enemy
        for (int i = 0; i < _wave.mediumCount; i++)
        {
            SpawnRangedEnemy(_wave.mediumEnemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        //Spawn hard enemy
        for (int i = 0; i < _wave.hardCount; i++)
        {
            SpawnEnemy(_wave.hardEnemy);
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

    void SpawnEnemy(Transform _enemy)
    {
        // Enemy
        //Debug.Log("Spawning Enemy: " + _enemy.name);
        Transform _sp = groundSpawnPoints[Random.Range(0, groundSpawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);
    }

    void SpawnRangedEnemy(Transform _enemy)
    {
        // Enemy
        //Debug.Log("Spawning Enemy: " + _enemy.name);
        int point = Random.Range(0, availablePoints.Count);
        Transform _sp = availablePoints[point];
        Instantiate(_enemy, _sp.position, _sp.rotation);
        availablePoints.RemoveAt(point);
    }
}
