using System;
using UnityEngine;

/// <summary>
/// Scene Data relevant to Mausoleum Scene
/// </summary>
[Serializable]
public class MausoleumSceneData : SceneData
{
    public int puzzleChoice;
    public int puzzleSeed;
    public bool puzzleSolved;
}

public class MausoleumSceneController : SceneController
{
    [SerializeField] private StoryPuzzle _storyPuzzle;
    [SerializeField] private GameObject _fragment;
    [SerializeField] private GameObject _fragmentTrigger;
    [SerializeField] private GameObject _barrier;

    public override void Load(object data)
    {
        if (PlayerData.HasCrystal(PlayerData.Crystal.Forest))
        {
            _fragment.SetActive(false);
            _fragmentTrigger.SetActive(false);
        }

        if (data == null)
            return;

        var mausoleumData = (MausoleumSceneData)data;

        if (mausoleumData.puzzleSolved)
        {
            _barrier.SetActive(false);
            _storyPuzzle.SolvePuzzle(mausoleumData.puzzleChoice, mausoleumData.puzzleSeed);
        }
    }

    public override SceneData Save()
    {
        var data = new MausoleumSceneData();
        data.puzzleChoice   = _storyPuzzle.Choice;
        data.puzzleSeed     = _storyPuzzle.Seed;
        data.puzzleSolved   = _storyPuzzle.Solved;
        return data;
    }
}
