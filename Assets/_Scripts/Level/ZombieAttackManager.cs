using System;
using System.Collections;
using UnityEngine;

public class ZombieAttackManager : MonoBehaviour
{
    [SerializeField] private Spawner _zombieSpawner = default;
    [SerializeField] private GameObject _blocker = default;
    [SerializeField] private GameObject _puzzleCharacters = default;
    [SerializeField] private GameObject _preCharacters = default;
    [SerializeField] private GameObject _postCharacters = default;
    [SerializeField] private GameObject _endingAttack = default;
    [SerializeField] private CutsceneManual _endingCutscene = default;
    [SerializeField] private LocationExit _mausoleumExit = default;

    [SerializeField] private TransformEventChannelSO _playerInstantiated = default;

    private GameObject _player;
    private bool _ended = false;
    private bool _finished = false;

    public bool FinishedAttack
    {
        get => _finished;
        set => _finished = value;
    }

    private void Awake()
    {
        _zombieSpawner.gameObject.SetActive(false);
        _preCharacters.SetActive(false);
        _postCharacters.SetActive(false);
    }

    private void OnDisable()
    {
        if (_playerInstantiated)
            _playerInstantiated.OnEventRaised -= OnPlayerSpawned;
    }

    private void OnPlayerSpawned(Transform value) => value.GetComponentInParent<Damageable>().OnDie += EndAttack;

    public void StartAttack()
    {
        if (_playerInstantiated)
            _playerInstantiated.OnEventRaised += OnPlayerSpawned;

        _mausoleumExit.gameObject.SetActive(false);

        _blocker.SetActive(true);
        _puzzleCharacters.SetActive(false);
        _preCharacters.SetActive(true);

        _zombieSpawner.gameObject.SetActive(true);
        _zombieSpawner.StartSpawning();

        StartCoroutine(EndAttackDelay(60, EndAttack));
    }

    public void EndAttack()
    {
        if (_playerInstantiated)
            _playerInstantiated.OnEventRaised -= OnPlayerSpawned;

        if (_ended)
            return;

        _endingAttack.SetActive(true);
        _blocker.SetActive(false);

        _ended = true;
        _zombieSpawner.StopSpawning();

        _player = FindObjectOfType<PlayerController>().gameObject;
        _player.GetComponent<Damageable>().OnDie -= EndAttack;
        
        // Start the ending cutscene now
        _endingCutscene.PlayCutScene();

        _player.SetActive(false);
    }

    public void PreStep()
    {
        _puzzleCharacters.SetActive(true);

        _preCharacters.SetActive(false);
        _postCharacters.SetActive(false);
        _mausoleumExit.gameObject.SetActive(true);
    }

    public void PostStep()
    {
        _puzzleCharacters.SetActive(false);

        _player?.SetActive(true);
        _preCharacters.SetActive(false);
        _postCharacters.SetActive(true);

        _mausoleumExit.gameObject.SetActive(true);
        _endingAttack.SetActive(false);

        _zombieSpawner.StopSpawning(true);

        _finished = true;
    }

    public IEnumerator EndAttackDelay(float delay, Action task)
    {
        yield return new WaitForSeconds(delay);
        task.Invoke();
    }
}
