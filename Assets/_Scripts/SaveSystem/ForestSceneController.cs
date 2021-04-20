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

    public bool slimeDoorSolved;
}

public class ForestSceneController : SceneController
{
    [SerializeField] private FollowPathPuzzle _followPath = default;
    [SerializeField] private ZombieAttackManager _zombieAttackManager = default;
    [SerializeField] private Animator _graveyardAnimator = default;

    [SerializeField] private UnlockingBase _slimeDoor = default;
    [SerializeField] private GameObject _preSlimes = default;
    [SerializeField] private GameObject _boss = default;

    [SerializeField] private GameObject _missingGirl = default;

    public override void Load(object data)
    {
        if (data == null)
            return;
        
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
                _zombieAttackManager.PostStep();
            }
        }
        else
        {
            _graveyardAnimator.Play("PuzzleState");
        }

        if (forestData.slimeDoorSolved)
        {
            _preSlimes.SetActive(false);
            _slimeDoor.SetLockState(false);

            if (PlayerData.HasCrystal(PlayerData.Crystal.SlimeCrystal))
            {
                _boss.SetActive(false);
            }
        }
        else
        {
            _preSlimes.SetActive(true);
            _slimeDoor.SetLockState(true);
        }

        if (PlayerData.HasCrystal(PlayerData.Crystal.WolfCrystal))
        {
            _missingGirl.SetActive(false);
        }
        else
        {
            _missingGirl.SetActive(true);
        }
    }

    public override SceneData Save()
    {
        var data = new ForestSceneData();

        data.puzzleChoice = _followPath.Choice;
        data.puzzleSolved = _followPath.IsSolved;
        data.zombieAttackFinished = _zombieAttackManager.FinishedAttack;
        data.slimeDoorSolved = !_slimeDoor.Locked;

        return data;
    }
}
