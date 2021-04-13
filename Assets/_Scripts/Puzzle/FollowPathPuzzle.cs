using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPathPuzzle : MonoBehaviour
{
    [SerializeField] private Vector2Int _dimensions;
    [SerializeField] private Vector2Int _startPoint;
    [SerializeField] private Transform _restartPoint;
    [SerializeField] private float _showDelay;
    [SerializeField] private GameObject _blocker;
    [SerializeField] private FollowPathInterface _interface;

    private Tile[,] _potentialPoints;

    [System.Serializable]
    public class Point
    {
        public List<Vector2Int> List = new List<Vector2Int>();
    }

    public List<Point> _pathes;

    private int _choice = 0;
    private List<Vector2Int> _path => _pathes[_choice].List;

    public HashSet<Tile> _currentPath = new HashSet<Tile>();

    public void Start()
    {
        _potentialPoints = new Tile[_dimensions.x, _dimensions.y];

        _blocker.SetActive(true);

        // Find all potential points based on children
        FindPoints();

        _choice = Random.Range(0, _pathes.Count - 1);
    }

    public void ShowPath()
    {
        StartCoroutine(LightUpPath(_showDelay));
    }

    private IEnumerator LightUpPath(float delay)
    {
        _blocker.SetActive(true);
        _interface.Disable();

    #if UNITY_EDITOR
        if (_potentialPoints == null)
            yield return null;
    #endif

        for (int i = 0; i < _path.Count; ++i)
        {
            if (i > 1)
            {
                _potentialPoints[_path[i - 2].x, _path[i - 2].y].FadeOutIndicator();
            }
            _potentialPoints[_path[i].x, _path[i].y].FadeInIndicator();
            yield return new WaitForSeconds(delay);
        }
        _potentialPoints[_path[_path.Count - 2].x, _path[_path.Count - 2].y].FadeOutIndicator();
        yield return new WaitForSeconds(delay);

        _potentialPoints[_path[_path.Count - 1].x, _path[_path.Count - 1].y].FadeOutIndicator();
        yield return new WaitForSeconds(delay);

        _interface.Enable();
        _blocker.SetActive(false);
    }

    public bool StepOnTile(Tile tile, GameObject player)
    {
        if (_path.Contains(new Vector2Int(tile.X, tile.Y)))
        {
            if (_currentPath.Add(tile) && _currentPath.Count == _path.Count)
            {
                _interface.Disable();
            }
            return true;
        }
        else
        {
            PlayerController _player;
            if (player.TryGetComponent(out _player))
            {
                StartCoroutine(SendPlayerBack(_player));
            }
            return false;
        }
    }

    private IEnumerator SendPlayerBack(PlayerController _player)
    {
        yield return new WaitForSeconds(0.1f);

        // Prevent application of movement to interrupt teleport
        _player.GetComponent<CharacterController>().enabled = false;
        yield return new WaitForSeconds(0.75f); // Pause for a second to make the player fully appreciate how wrong they are
        _player.transform.position = _restartPoint.position;
        _player.transform.rotation = _restartPoint.rotation;
        _player.GetComponent<CharacterController>().enabled = true;
    }

    private void FindPoints()
    {
        foreach(Tile tile in transform.GetComponentsInChildren<Tile>())
        {
            _potentialPoints[tile.X, tile.Y] = tile;
        }
    }
}
