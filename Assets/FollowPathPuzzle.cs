using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPathPuzzle : MonoBehaviour
{
    [SerializeField] private Vector2Int _dimensions;
    [SerializeField] private Vector2Int _startPoint;
    [SerializeField] private Transform _restartPoint;
    [SerializeField] private float _showDelay;
    [SerializeField] private GameObject _tileHolder;

    private Tile[,] _potentialPoints;

    public List<Vector2Int> _path;
    public List<(MeshRenderer, Color)> _currentPath = new List<(MeshRenderer, Color)>();

    public void Start()
    {
        _potentialPoints = new Tile[_dimensions.x, _dimensions.y];
        
        // Find all potential points based on children
        FindPoints();

        var total = 0;
        for (int i = 0; i < _dimensions.x; ++i)
        {
            for (int j = 0; j < _dimensions.y; ++j)
            {
                if (_potentialPoints[i,j] != null)
                {
                    total++;
                }
            }
        }
        Debug.Log(total);
    }

    public void ShowPath()
    {
        StartCoroutine(LightUpPath(_showDelay));
    }

    private IEnumerator LightUpPath(float delay)
    {
        Stack<(MeshRenderer, Color)> tiles = new Stack<(MeshRenderer, Color)>();
        
        foreach(var coord in _path)
        {
            MeshRenderer renderer = null;
            if ((bool)_potentialPoints[coord.x, coord.y]?.TryGetComponent(out renderer))
            {
                tiles.Push((renderer, renderer.material.color));

                renderer.material.color = Color.green;
                yield return new WaitForSeconds(delay);
            }
        }

        while(tiles.Count > 0)
        {
            (var renderer, var originalColor) = tiles.Pop();
            renderer.material.color = originalColor;
        }
    }

    public void StepOnTile(Tile tile, GameObject player)
    {
        var renderer = tile.gameObject.GetComponent<MeshRenderer>();
        _currentPath.Add((renderer, renderer.material.color));
        
        if (_path.Contains(new Vector2Int(tile.X, tile.Y)))
        {
            renderer.material.color = Color.green;
        }
        else
        {
            renderer.material.color = Color.red;
            PlayerController _player;
            if (player.TryGetComponent(out _player))
            {
                StartCoroutine(SendPlayerBack(_player));
            }
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
        foreach(Tile tile in _tileHolder.GetComponentsInChildren<Tile>())
        {
            _potentialPoints[tile.X, tile.Y] = tile;
        }
    }
}
