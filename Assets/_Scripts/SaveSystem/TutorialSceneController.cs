using System;
using UnityEngine;

/// <summary>
/// Scene Data relevant to Forest Scene
/// </summary>
[Serializable]
public class TutorialSceneData : SceneData
{
    public int _currentCheckpoint;
    public bool _solvedPuzzle;
    public bool _solvedCombat;

    public int _targetOneHealth;
    public int _targetTwoHealth;
    public int _targetThreeHealth;
}

public class TutorialSceneController : SceneController
{
    [SerializeField] private GameObject _openingCutsceneTrigger;
    [SerializeField] private GameObject _oldManCutsceneTrigger;
    [SerializeField] private SafetyNet _safetyNet;
    [SerializeField] private MirrorGameplayController _mirror;

    [SerializeField] private UnlockingDoor _unlockingDoor;
    [SerializeField] private BoxCollider _basket;

    [SerializeField] private UnlockingDoor _combatDoor;
    [SerializeField] private Damageable _targetOne;
    [SerializeField] private Damageable _targetTwo;
    [SerializeField] private Damageable _targetThree;

    public override void Load(object data)
    {
        // Since they are continuing - we are just going to assume they have seen the cutscene before
        _openingCutsceneTrigger.SetActive(false);

        if (data == null)
            return;

        var tutorialData = (TutorialSceneData)data;
        if (tutorialData._solvedPuzzle)
        {
            _basket.enabled = false;
            _unlockingDoor.SetLockState(false);
            _oldManCutsceneTrigger.SetActive(false);
        }

        if (tutorialData._solvedCombat)
        {
            _combatDoor.SetLockState(false);
            _targetOne.Kill();
            _targetTwo.Kill();
            _targetThree.Kill();
        }
        else
        {
            _targetOne.SetHealth(tutorialData._targetOneHealth);
            _targetTwo.SetHealth(tutorialData._targetTwoHealth);
            _targetThree.SetHealth(tutorialData._targetThreeHealth);
        }

        _safetyNet.SetCheckpoint(tutorialData._currentCheckpoint);
        switch (tutorialData._currentCheckpoint)
        {
            case 0:
                _mirror.PlayTransition("Platforming");
                break;
            case 1:
                _mirror.PlayTransition("Apple");
                break;
            case 2:
                _mirror.PlayTransition("Combat");
                break;
            case 3:
                _mirror.PlayTransition("Portal");
                break;
        }
    }

    public override SceneData Save()
    {
        var data = new TutorialSceneData();
        data._currentCheckpoint = _safetyNet.CurrentIndex;
        
        data._solvedPuzzle = !_unlockingDoor.Locked;
        data._solvedCombat = !_combatDoor.Locked;

        data._targetOneHealth = !_targetOne ? 0 : _targetOne.CurrentHealth;
        data._targetTwoHealth = !_targetTwo ? 0 : _targetTwo.CurrentHealth;
        data._targetThreeHealth = !_targetThree ? 0 : _targetThree.CurrentHealth;

        return data;
    }
}
