using System;
using UnityEngine;

/// <summary>
/// Scene Data relevant to Forest Scene
/// </summary>
[Serializable]
public class TavernSceneData : SceneData
{
}

class TavernSceneController : SceneController
{
    [SerializeField] private GameObject _hunter;

    public override void Load(object data)
    {
        var tavernData = (TavernSceneData)data;

        if (PlayerData.HasCrystal(PlayerData.Crystal.DeerCrystal))
        {
            _hunter.SetActive(false);
        }
        else
        {
            _hunter.SetActive(true);
        }
    }

    public override SceneData Save()
    {
        var data = new TavernSceneData();
        return data;
    }
}
