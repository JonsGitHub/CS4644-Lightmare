using UnityEngine;

public class FollowPathInterface : InterfaceBase
{
    [SerializeField] private FollowPathPuzzle _puzzle;

    public override void Interact()
    {
        _puzzle.ShowPath();
    }

    public void Disable()
    {
        tag = "Untagged";
    }

    public void Enable()
    {
        tag = "Interface";
    }
}
