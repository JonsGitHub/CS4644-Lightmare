using UnityEngine;

public class StoryResetColumn : InterfaceBase
{
    public override void Interact()
    {
        GetComponentInParent<StoryPuzzle>().ResetPuzzle();
    }

    public void Disable()
    {
        tag = "Untagged";
    }
}
