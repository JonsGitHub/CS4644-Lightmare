using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class StoryPuzzle : MonoBehaviour
{
    private List<StoryColumn> _storyColumns;

    [Header("Broadcasting on channels")]
    [SerializeField] private BoolEventChannelSO _unlockingChannel = default;

    [System.Serializable]
    public class Story
    {
        public List<string> List = new List<string>();
    }

    [Header("Story Data")]
    public List<Story> _stories;

    private int _choice = 0;
    private List<string> _story => _stories[_choice].List;

    private List<string> _currentSelection = new List<string>();

    void Start()
    {
        _storyColumns = GetComponentsInChildren<StoryColumn>().ToList();
        Assert.IsTrue(_storyColumns.Count == 4);

        _choice = Random.Range(0, _stories.Count - 1);

        // Setup break up story onto columns
        SetupPuzzle();
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
                {
                    col.tag = "Untagged";
                }
            }
            else
            {
                // Incorrect
                ResetPuzzle();
                return;
            }
        }
        column.SetNumber(_currentSelection.Count);
    }

    private void SetupPuzzle()
    {
        List<string> _shuffled = new List<string>(_story); // Copy list

        //TODO: keep track of correct order for later comparison?

        // Version of Fisher - Yates shuffle
        var rng = new System.Random();
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
