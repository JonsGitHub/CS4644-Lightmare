using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPathPuzzle : MonoBehaviour
{
    [SerializeField] private Transform _restartPoint;
    [SerializeField] private float _showDelay;
    [SerializeField] private GameObject _blocker;
    [SerializeField] private FollowPathInterface _interface;

    public List<GameObject> _pathes;

    private int _choice = 0;
    private GameObject _path => _pathes[_choice];

    public HashSet<Tile> _currentPath = new HashSet<Tile>();

    public void Awake()
    {
        foreach(var path in _pathes)
            path.SetActive(false);
    }

    public void Start()
    {
        _blocker.SetActive(true);
        _choice = Random.Range(0, _pathes.Count - 1);
        _path.SetActive(true);
    }

    public void ShowPath()
    {
        StartCoroutine(ShowPathCoroutine(_showDelay));
    }

    private IEnumerator ShowPathCoroutine(float delay)
    {
        _blocker.SetActive(true);
        _interface.Disable();

        foreach (Transform child in _path.transform)
        {
            child.GetComponent<Tile>()?.FadeInIndicator();
            yield return new WaitForSeconds(delay);
        }

        yield return new WaitForSeconds(delay);

        foreach (Transform child in _path.transform)
        {
            child.GetComponent<Tile>()?.FadeOutIndicator();
        }

        _interface.Enable();
        _blocker.SetActive(false);
    }

    public void StepOnTile(Tile tile)
    {
        if (_currentPath.Add(tile) && _currentPath.Count == _path.transform.childCount)
        {
            _interface.Disable();
        }
    }

    // Called by abyss zone trigger
    public void SendPlayBack(bool entered, GameObject obj)
    {
        PlayerController _player;
        if (entered && obj.TryGetComponent(out _player))
        {
            _player.GetComponent<CharacterController>().enabled = false;
            _player.transform.position = _restartPoint.position;
            _player.transform.rotation = _restartPoint.rotation;
            _player.GetComponent<CharacterController>().enabled = true;

            // Reset all the tiles back to hidden
            foreach (var tile in _currentPath)
            {
                tile.FadeOutIndicator();
            }
            _currentPath.Clear();
        }
    }
}
