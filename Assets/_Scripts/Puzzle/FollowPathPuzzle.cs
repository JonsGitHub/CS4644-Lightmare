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
    private bool _isSolved = false;

    private GameObject _path => _pathes[_choice];

    public HashSet<Tile> _currentPath = new HashSet<Tile>();

    public bool IsSolved => _isSolved;
    public int Choice => _choice;

    public void Awake()
    {
        _isSolved = false;

        foreach (var path in _pathes)
            path.SetActive(false);
    }

    public void Start()
    {
        _blocker.SetActive(true);
        _choice = Random.Range(0, _pathes.Count);
        _path.SetActive(true);
    }

    public void SolvePath(int choice)
    {
        foreach (var path in _pathes)
            path.SetActive(false);

        _isSolved = true;
        _choice = choice;
        _blocker.SetActive(false);

        // Puzzle is solved no need to show the path any longer - instead should now show the covered graveyard state.
        GameObject.Find("Graveyard").GetComponent<Animator>().Play("NormalState");
        GameObject.Find("GraveyardClosingCutsceneTrigger").SetActive(false);

        GameObject.Find("NPC_Isaac_ROOTED").SetActive(false);
        GameObject.Find("NPC_Dog_ROOTED").SetActive(false);
        GameObject.Find("GraveyardCutsceneTrigger").SetActive(false);
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
            _isSolved = true;
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
