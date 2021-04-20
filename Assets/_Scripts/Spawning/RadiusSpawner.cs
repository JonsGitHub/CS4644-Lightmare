using UnityEngine;

public class RadiusSpawner : Spawner
{
    [SerializeField] private float _spawnRadius = default;

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
        return position + new Vector3(Random.Range(-1, 1), 0.0f, Random.Range(-1, 1)).normalized * Random.Range(_spawnRadius / 2, _spawnRadius);
    }
}
