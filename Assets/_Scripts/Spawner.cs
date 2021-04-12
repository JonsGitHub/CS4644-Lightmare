using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float _spawnRate = default;
    [SerializeField] private float _spawnRadius = default;
    [SerializeField] private int _maxEnemy = default;
    [SerializeField] private GameObject _enemy = default;

    private bool _alive = true;

    private void Start()
    {
        SpawnEnemy();

        StartCoroutine(Spawning());
    }

    private void OnDestroy()
    {
        _alive = false; // Shutdown coroutine
    }

    private IEnumerator Spawning()
    {
        while(_alive)
        {
            if (transform.childCount < _maxEnemy)
                SpawnEnemy();
            yield return new WaitForSeconds(_spawnRate);
        }
    }

    private void SpawnEnemy()
    {
        var newEnemy = Instantiate(_enemy, GetPositionAroundPoint(transform.position), Quaternion.identity);
        newEnemy.transform.SetParent(transform); // Keep the Scene tree clean
    }

    // Compute a random target position around the starting position.
    private Vector3 GetPositionAroundPoint(Vector3 position)
    {
        return position + new Vector3(Random.Range(-1, 1), 0.0f, Random.Range(-1, 1)).normalized * Random.Range(_spawnRadius / 2, _spawnRadius);
    }
}
