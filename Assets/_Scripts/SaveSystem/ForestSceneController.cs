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
    [SerializeField] private ZombieAttackManager _zombieAttackManager = default;
    [SerializeField] private Animator _graveyardAnimator = default;

    public override void Load(object data)
    {
        var forestData = (ForestSceneData)data;

        if (forestData.puzzleSolved)
        {
            _graveyardAnimator.Play("NormalState");
            _followPath.SolvePath(forestData.puzzleChoice);
            
            if (!forestData.zombieAttackFinished && PlayerData.HasCrystal(PlayerData.Crystal.Forest))
            {
                _zombieAttackManager.StartAttack();
            }
            else
            {
                _zombieAttackManager.FinishedAttack = forestData.zombieAttackFinished;
            }
        }
        else
        {
            _graveyardAnimator.Play("PuzzleState");
        }
    }

    public override SceneData Save()
    {
        var data = new ForestSceneData();

        data.puzzleChoice = _followPath.Choice;
        data.puzzleSolved = _followPath.IsSolved;
        data.zombieAttackFinished = _zombieAttackManager.FinishedAttack;

        return data;
    }
}
