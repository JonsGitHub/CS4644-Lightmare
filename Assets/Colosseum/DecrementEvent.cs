using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecrementEvent : WaveBase
{
    [SerializeField] private WaveSpawner _spawner;

    public override void Lock()
    {
        _spawner.DecrementEnemy();
    }

    public override void Unlock()
    {
        _spawner.DecrementEnemy();
    }
}
