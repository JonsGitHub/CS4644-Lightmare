using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    [System.Serializable]
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

    public Wave[] waves;
    private int nextWave = 0;

    public Transform[] groundSpawnPoints;
    public Transform[] rangedSpawnPoints;
    private List<Transform> availablePoints;

    public float timeBetweenWaves = 5.0f;
    private float waveCountdown;

    private float searchCountdown = 1.0f;

    public SpawnState state = SpawnState.COUNTING;

    void Start()
    {
        waveCountdown = timeBetweenWaves;
        if (groundSpawnPoints.Length == 0)
        {
            Debug.Log("No Ground Spawn Points");
        }
        if (rangedSpawnPoints.Length == 0)
        {
            Debug.Log("No Ground Spawn Points");
        }
        else
        {
            availablePoints = new List<Transform>(rangedSpawnPoints);
        }
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
        Debug.Log("Wave Completed");

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            //nextWave = 0;
            Debug.Log("ALL WAVES COMPLETE!");
        }
        else
        {
            nextWave++;
        }
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0.0f)
        {
            searchCountdown = 1.0f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }

        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
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
        Debug.Log("Spawning Enemy: " + _enemy.name);
        Transform _sp = groundSpawnPoints[Random.Range(0, groundSpawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);
    }

    void SpawnRangedEnemy(Transform _enemy)
    {
        // Enemy
        Debug.Log("Spawning Enemy: " + _enemy.name);
        int point = Random.Range(0, availablePoints.Count);
        Transform _sp = availablePoints[point];
        Instantiate(_enemy, _sp.position, _sp.rotation);
        availablePoints.RemoveAt(point);
    }
}
