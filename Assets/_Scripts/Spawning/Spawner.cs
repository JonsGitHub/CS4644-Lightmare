using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField] private bool _startSpawningOnStart = true;
    [SerializeField] private float _spawnRate = default;
    [SerializeField] private AssetReference _entityReference;
    [SerializeField] protected string _overrideName = default;
    [SerializeField] protected int _maxEnemy = default;

    protected GameObject _prefab;
    protected bool _alive;

    protected void Awake()
    {
        Addressables.LoadAssetAsync<GameObject>(_entityReference).Completed += OnLoadDone;
    }

    private void OnLoadDone(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj)
    {
        _prefab = obj.Result;
    }

    protected void Start()
    {
        if (_startSpawningOnStart)
            StartSpawning();
    }

    protected void OnDestroy()
    {
        _alive = false; // Shutdown coroutine
    }

    public void StopSpawning(bool clean = false)
    {
        _alive = false;

        if (clean)
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<Damageable>()?.Kill();
            }
        }
    }

    public void StartSpawning()
    {
        _alive = true;

        if (transform.childCount < _maxEnemy)
            SpawnEnemy();

        StartCoroutine(Spawning());
    }

    public void SingleSpawn() => SpawnEnemy();

    protected IEnumerator Spawning()
    {
        while (_alive)
        {
            if (transform.childCount < _maxEnemy)
                SpawnEnemy();
            yield return new WaitForSeconds(_spawnRate);
        }
    }

    protected abstract void SpawnEnemy();
}
