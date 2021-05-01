using System;
using UnityEngine;

/// <summary>
/// Scene Data relevant to Scifi Scene
/// </summary>
[Serializable]
public class ScifiSceneData : SceneData
{
    public  Vector3 _puzzlePiecePosition1;
    public  Vector3 _puzzlePiecePosition2;
    public  Vector3 _puzzlePiecePosition3;
    public  Vector3 _puzzlePiecePosition4;
    public  bool _solvedPuzzle;

    public float _grunkHealth1;
    public float _grunkHealth2;
    public float _grunkHealth3;
    public float _grunkHealth4;
    public float _grunkHealth5;
    public float _grunkHealth6;
    public float _grunkHealth7;
    public float _grunkHealth8;
    public float _grunkHealth9;
    public float _grunkHealth10;
    public float _grunkHealth11;
}

class ScifiSceneController : SceneController
{
    [SerializeField] private GameObject _shipPart1;
    [SerializeField] private GameObject _shipPart2;
    [SerializeField] private GameObject _shipPart3;
    [SerializeField] private GameObject _shipPart4;


    [SerializeField] private UnlockingDoor _unlockingDoor;
    [SerializeField] private GameObject _doorCutsceneTrigger;
    [SerializeField] private GameObject _fragment;
    [SerializeField] private GameObject _exitPortal;

    [SerializeField] private Damageable _grunk1;
    [SerializeField] private Damageable _grunk2;
    [SerializeField] private Damageable _grunk3;
    [SerializeField] private Damageable _grunk4;
    [SerializeField] private Damageable _grunk5;
    [SerializeField] private Damageable _grunk6;
    [SerializeField] private Damageable _grunk7;
    [SerializeField] private Damageable _grunk8;
    [SerializeField] private Damageable _grunk9;
    [SerializeField] private Damageable _grunk10;
    [SerializeField] private Damageable _grunk11;

    public override void Load(object data)
    {
        if (data == null)
            return;

        var scifiData = (ScifiSceneData)data;
        
        // Assume solved puzzle and open portal to next level
        if (scifiData._solvedPuzzle)
        {
            _unlockingDoor.SetLockState(false);
            _doorCutsceneTrigger.SetActive(false);
            _unlockingDoor.enabled = false; // prevent second trigger
        }
        if (PlayerData.HasCrystal(PlayerData.Crystal.SciFiCrystal))
        {
            _fragment.SetActive(false);
            _exitPortal.SetActive(true);
        }

        _shipPart1.transform.position = scifiData._puzzlePiecePosition1;
        _shipPart2.transform.position = scifiData._puzzlePiecePosition2;
        _shipPart3.transform.position = scifiData._puzzlePiecePosition3;
        _shipPart4.transform.position = scifiData._puzzlePiecePosition4;

        // Disgusting way to do this but quick and effective
        _grunk1.SetHealth(scifiData._grunkHealth1);
        _grunk2.SetHealth(scifiData._grunkHealth2);
        _grunk3.SetHealth(scifiData._grunkHealth3);
        _grunk4.SetHealth(scifiData._grunkHealth4);
        _grunk5.SetHealth(scifiData._grunkHealth5);
        _grunk6.SetHealth(scifiData._grunkHealth6);
        _grunk7.SetHealth(scifiData._grunkHealth7);
        _grunk8.SetHealth(scifiData._grunkHealth8);
        _grunk9.SetHealth(scifiData._grunkHealth9);
        _grunk10.SetHealth(scifiData._grunkHealth10);
        _grunk11.SetHealth(scifiData._grunkHealth11);
    }

    public override SceneData Save()
    {
        var data = new ScifiSceneData();

        data._puzzlePiecePosition1 = _shipPart1.transform.position;
        data._puzzlePiecePosition2 = _shipPart2.transform.position;
        data._puzzlePiecePosition3 = _shipPart3.transform.position;
        data._puzzlePiecePosition4 = _shipPart4.transform.position;

        data._solvedPuzzle = !_unlockingDoor.Locked;

        data._grunkHealth1 = _grunk1 ?  _grunk1.CurrentHealth : 0;
        data._grunkHealth2 = _grunk2 ?  _grunk2.CurrentHealth : 0;
        data._grunkHealth3 = _grunk3 ?  _grunk3.CurrentHealth : 0;
        data._grunkHealth4 = _grunk4 ?  _grunk4.CurrentHealth : 0;
        data._grunkHealth5 = _grunk5 ?  _grunk5.CurrentHealth : 0;
        data._grunkHealth6 = _grunk6 ?  _grunk6.CurrentHealth : 0;
        data._grunkHealth7 = _grunk7 ?  _grunk7.CurrentHealth : 0;
        data._grunkHealth8 = _grunk8 ?  _grunk8.CurrentHealth : 0;
        data._grunkHealth9 = _grunk9 ?  _grunk9.CurrentHealth : 0;
        data._grunkHealth10 = _grunk10 ?  _grunk10.CurrentHealth : 0;
        data._grunkHealth11 = _grunk11 ?  _grunk11.CurrentHealth :  0;

        return data;
    }

    public override bool SavePosition() => true;
}
