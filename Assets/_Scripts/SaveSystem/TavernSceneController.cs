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
    public override void Load(object data)
    {
        var tavernData = (TavernSceneData)data;
    }

    public override SceneData Save()
    {
        var data = new TavernSceneData();
        return data;
    }
}
