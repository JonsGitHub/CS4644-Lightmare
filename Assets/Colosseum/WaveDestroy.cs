using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveDestroy : MonoBehaviour
{
    void OnDestroy()
    {
        if (GameObject.Find("WaveSpawner") != null)
        {
            GameObject spawner = GameObject.Find("WaveSpawner");
            WaveSpawner _waveSpawner = spawner.GetComponent<WaveSpawner>();
            if (gameObject.layer == 12)
            {
                _waveSpawner.DecrementEnemy();
            }
            else if (gameObject.layer == 10)
            {
                _waveSpawner.RestartWaves();
            }
        }
    }
}
