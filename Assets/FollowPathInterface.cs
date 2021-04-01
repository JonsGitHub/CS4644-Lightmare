using UnityEngine;

public class FollowPathInterface : InterfaceBase
{
    [SerializeField] private FollowPathPuzzle _puzzle;

    public override void Interact()
    {
        _puzzle.ShowPath();
    }
}
