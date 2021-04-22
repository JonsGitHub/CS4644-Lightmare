using System;
using UnityEngine;

/// <summary>
/// Scene Data relevant to Forest Scene
/// </summary>
[Serializable]
public class ColosseumSceneData : SceneData
{
    public bool finishedWaves;
}

public class ColosseumSceneController : SceneController
{
    [SerializeField] private GameObject _openingCutscene;
    [SerializeField] private WaveSpawner _waveSpawner;

    public override void Load(object data)
    {
        if (data == null)
            return;

        var colosseumData = (ColosseumSceneData)data;
        if (colosseumData.finishedWaves)
        {
            _openingCutscene.SetActive(false);
            _waveSpawner.FlagFinished();
        }
    }

    public override SceneData Save()
    {
        var data = new ColosseumSceneData();
        data.finishedWaves = _waveSpawner.Finished;
        return data;
    }
}
