using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpotSpawner : Spawner
{
    [SerializeField] private List<Transform> _spawnSpots = new List<Transform>();

    private new void Awake()
    {
        base.Awake();
    }

    private new void Start()
    {
        base.Start();
    }

    private new void OnDestroy()
    {
        base.OnDestroy();
    }

    protected override void SpawnEnemy()
    {
        if (_prefab == null) // Asset isn't ready yet
            return;

        var newEnemy = Instantiate(_prefab, GetPositionAroundPoint(transform.position), Quaternion.identity);
        newEnemy.transform.SetParent(transform); // Keep the Scene tree clean

        if (_overrideName != null && _overrideName.Length > 0)
            newEnemy.name = _overrideName;
    }

    // Compute a random target position around the starting position.
    private Vector3 GetPositionAroundPoint(Vector3 position)
    {
        if (_spawnSpots.Count == 0)
            return Vector3.zero;
        return _spawnSpots.ElementAt(Random.Range(0, _spawnSpots.Count)).position;
    }
}
