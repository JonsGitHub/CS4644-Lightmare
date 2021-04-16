using System;
using UnityEngine;

/// <summary>
/// Scene Data relevant to Forest Scene
/// </summary>
[Serializable]
public class ForestSceneData : SceneData
{
    public int puzzleChoice;
    public bool puzzleSolved;
    public bool zombieAttackFinished;
}

public class ForestSceneController : SceneController
{
    [SerializeField] private FollowPathPuzzle _followPath = default;

    public override void Load(object data)
    {
        var forestData = (ForestSceneData)data;

        if (forestData.puzzleSolved)
        {
            _followPath.SolvePath(forestData.puzzleChoice);
            
            if (!forestData.zombieAttackFinished && PlayerData.HasCrystal(PlayerData.Crystal.Forest))
            {

                Debug.Log("Has Forest Crystal trigger zombie attack");
            }
        }
    }

    public override SceneData Save()
    {
        var data = new ForestSceneData();

        data.puzzleChoice = _followPath.Choice;
        data.puzzleSolved = _followPath.IsSolved;
        data.zombieAttackFinished = false;

        return data;
    }
}
