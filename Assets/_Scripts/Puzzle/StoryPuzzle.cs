using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class StoryPuzzle : MonoBehaviour
{
    private List<StoryColumn> _storyColumns;

    [Header("Broadcasting on channels")]
    [SerializeField] private BoolEventChannelSO _unlockingChannel = default;
    [SerializeField] private VoidEventChannelSO _failedChannel = default;

    [System.Serializable]
    public class Story
    {
        public List<string> List = new List<string>();
    }

    [Header("Story Data")]
    public List<Story> _stories;

    private int _choice = 0;
    private int _seed = 0;
    private bool _solved = false;
    private List<string> _story => _stories[_choice].List;

    private List<string> _currentSelection = new List<string>();

    public int Choice => _choice;
    public int Seed => _seed;
    public bool Solved => _solved;

    void Start()
    {
        _storyColumns = GetComponentsInChildren<StoryColumn>().ToList();
        Assert.IsTrue(_storyColumns.Count == 4);

        _choice = Random.Range(0, _stories.Count);

        // Setup break up story onto columns
        SetupPuzzle();
    }

    public void SolvePuzzle(int choice, int seed)
    {
        _solved = true;
        _choice = choice;
        SetupPuzzle(seed);

        for (int i = 0; i < 4; ++i)
        {
            foreach (var col in _storyColumns)
            {
                if (col.Text.Equals(_story[i]))
                {
                    col.SetNumber(i);
                    break;
                }
            }
        }

        // Puzzle is solved no need to show the path any longer - instead should now show the covered graveyard state.
        GetComponentInChildren<StoryResetColumn>()?.Disable(); // Disable resetting column
        foreach (var col in _storyColumns)
            col.tag = "Untagged";
        _unlockingChannel.RaiseEvent(true);
    }

    public void ResetPuzzle()
    {
        foreach (var col in _storyColumns)
        {
            col.SetNumber(-1);
        }
        _currentSelection.Clear();
    }

    public void SelectColumn(StoryColumn column, string text)
    {
        _currentSelection.Add(text);

        if (_currentSelection.Count == 4)
        {
            if (_story.SequenceEqual(_currentSelection))
            {
                // Correct
                GetComponentInChildren<StoryResetColumn>()?.Disable(); // Disable resetting column
                _unlockingChannel.RaiseEvent(true);

                // Disable all of the story columns
                foreach (var col in _storyColumns)
                    col.tag = "Untagged";

                _solved = true;
            }
            else
            {
                // Incorrect
                _failedChannel?.RaiseEvent();
                ResetPuzzle();
                return;
            }
        }
        column.SetNumber(_currentSelection.Count);
    }

    private void SetupPuzzle(int seed = -1)
    {
        List<string> _shuffled = new List<string>(_story); // Copy list

        // Version of Fisher - Yates shuffle
        _seed = seed == -1 ? Random.Range(0, 1000000000) : seed;
        var rng = new System.Random(_seed);
        int n = _shuffled.Count;
        while(n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            var temp = _shuffled[k];
            _shuffled[k] = _shuffled[n];
            _shuffled[n] = temp;
        }

        for (int i = 0; i < _storyColumns.Count; ++i)
        {
            _storyColumns[i].SetStoryText(_shuffled[i]);
        }
    }

    private void OnDestroy()
    {
        foreach(var column in _storyColumns)
        {
            Destroy(column);
        }
    }
}
