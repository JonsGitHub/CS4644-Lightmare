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
    }

    private int totalEnemiesCount;

    public Wave[] waves;
    private int nextWave = 0;

    public Transform[] groundSpawnPoints;
    public Transform[] rangedSpawnPoints;
    private List<Transform> availablePoints;

    public float timeBetweenWaves = 5.0f;
    private float waveCountdown;

    public SpawnState state = SpawnState.COUNTING;

    private bool _finished = false;

    public bool Finished => _finished;

    public void FlagFinished() => _finished = true;

    void Start()
    {
        Wave _wave = waves[nextWave];
        totalEnemiesCount = _wave.easyCount + _wave.mediumCount + _wave.hardCount + _wave.bossCount;

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
        //Debug.Log("Wave Completed");

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
            Wave _wave = waves[nextWave];
            totalEnemiesCount = _wave.easyCount + _wave.mediumCount + _wave.hardCount + _wave.bossCount;
        }
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

    public void DecrementEnemy()
    {
        totalEnemiesCount--;
        _enemiesCounter.text = "Enemies Remaining:\n" + totalEnemiesCount;
    }

    public void RestartWaves()
    {
        nextWave = 0;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        Wave _wave = waves[nextWave];
        totalEnemiesCount = _wave.easyCount + _wave.mediumCount + _wave.hardCount + _wave.bossCount;

        _waveCounter.text = "Restarting Waves";
        _enemiesCounter.text = null;

        waveCountdown = timeBetweenWaves;

        state = SpawnState.COUNTING;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        _waveCounter.text = "Wave: " + (nextWave + 1);
        _enemiesCounter.text = "Enemies Remaining:\n" + totalEnemiesCount;

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
