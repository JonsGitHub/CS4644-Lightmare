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

    [SerializeField] private GameObject _hunter = default;
    [SerializeField] private UnlockingBase _slimeDoor = default;
    [SerializeField] private GameObject _preSlimes = default;
    [SerializeField] private GameObject _boss = default;

    [SerializeField] private GameObject _missingGirl = default;

    [SerializeField] private GameObject _deer = default;

    public override void Load(object data)
    {
        _missingGirl.SetActive(!PlayerData.HasCrystal(PlayerData.Crystal.WolfCrystal));
        _deer.SetActive(!PlayerData.HasCrystal(PlayerData.Crystal.DeerCrystal));

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
                if (!forestData.zombieAttackFinished)
                {
                    _zombieAttackManager.PreStep();
                }
                else
                {
                    _zombieAttackManager.PostStep();
                }
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
                _hunter.SetActive(false);
            }
        }
        else
        {
            _hunter.SetActive(true);
            _preSlimes.SetActive(true);
            _slimeDoor.SetLockState(true);
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
