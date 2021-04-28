using System.Collections.Generic;
using UnityEngine;

public class MisfortuneCutsceneController : MonoBehaviour
{
    [SerializeField] private Exploder _exploder;
    [SerializeField] private List<Exploder> _explodingObelisks;
    [SerializeField] private LocationExit _exit;

    [SerializeField] private SkyboxManager _skyboxManager;

    public void ExplodeBarrier() => _exploder.Explode();

    public void ExplodeObelisk(int index) => _explodingObelisks[index].Explode();

    public void LoadNextLevel() => _exit.ManualTrigger();

    public void SwitchSkybox() => _skyboxManager.SwitchToSecondSkybox();
}
