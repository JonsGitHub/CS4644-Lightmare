using UnityEngine;

public class UnlockingCutscene : UnlockingBase
{
    [SerializeField] private CutsceneManual _cutscene;

    public override void Lock() { }

    public override void Unlock() 
    {
        _cutscene.PlayCutScene();
    }
}
