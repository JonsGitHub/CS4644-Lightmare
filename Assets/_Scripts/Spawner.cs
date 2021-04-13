using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float _spawnRate = default;
    [SerializeField] private float _spawnRadius = default;
    [SerializeField] private int _maxEnemy = default;
    [SerializeField] private AssetReference _entityReference;
    [SerializeField] private string _overrideName = default;

    private bool _alive = true;
    private GameObject _prefab;

    private void Awake()
    {
        Addressables.LoadAssetAsync<GameObject>(_entityReference).Completed += OnLoadDone;
    }

    private void Start()
    {
        if (transform.childCount < _maxEnemy)
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
        if (_prefab == null) // Asset isn't ready yet
            return;

        var newEnemy = Instantiate(_prefab, GetPositionAroundPoint(transform.position), Quaternion.identity);
        newEnemy.transform.SetParent(transform); // Keep the Scene tree clean

        if (_overrideName != null && _overrideName.Length > 0)
            newEnemy.name = _overrideName;
    }

    private void OnLoadDone(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj)
    {
        _prefab = obj.Result;
    }

    // Compute a random target position around the starting position.
    private Vector3 GetPositionAroundPoint(Vector3 position)
    {
        return position + new Vector3(Random.Range(-1, 1), 0.0f, Random.Range(-1, 1)).normalized * Random.Range(_spawnRadius / 2, _spawnRadius);
    }
}
