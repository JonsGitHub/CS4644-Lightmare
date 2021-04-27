using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartWave : WaveBase
{
    [SerializeField] private WaveSpawner _spawner;

    public override void Lock()
    {
        _spawner.RestartWaves(null);
    }

    public override void Unlock()
    {
        _spawner.RestartWaves(null);
    }
}