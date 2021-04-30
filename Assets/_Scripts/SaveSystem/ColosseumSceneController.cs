using System;
using UnityEngine;

/// <summary>
/// Scene Data relevant to Forest Scene
/// </summary>
[Serializable]
public class ColosseumSceneData : SceneData
{
    public bool finishedWaves;
    public Vector3 fragmentPosition;
}

public class ColosseumSceneController : SceneController
{
    [SerializeField] private GameObject _openingCutscene;
    [SerializeField] private GameObject _exitPortal;
    [SerializeField] private WaveSpawner _waveSpawner;
    [SerializeField] private GameObject _fragment;
    [SerializeField] private GameObject _entrance;

    public override void Load(object data)
    {
        if (data == null)
            return;

        var colosseumData = (ColosseumSceneData)data;

        // Has crystal so just assume they have finished
        if (PlayerData.HasCrystal(PlayerData.Crystal.ColosseumCrystal))
        {
            _entrance.SetActive(false);
            _openingCutscene.SetActive(false);
            _waveSpawner.FlagFinished();
            _exitPortal.SetActive(true);
        }
        else if (colosseumData.finishedWaves)
        {
            _entrance.SetActive(false);
            var fragment = Instantiate(_fragment, colosseumData.fragmentPosition, Quaternion.identity);
            _openingCutscene.SetActive(false);
            _waveSpawner.FlagFinished();
        }
    }

    public override SceneData Save()
    {
        var data = new ColosseumSceneData();

        data.finishedWaves = _waveSpawner.Finished;
        
        var fragment = FindObjectOfType<FragmentController>();
        data.fragmentPosition = fragment ? fragment.transform.position : Vector3.negativeInfinity;

        return data;
    }

    public override bool SavePosition() => _waveSpawner.Finished;
}
