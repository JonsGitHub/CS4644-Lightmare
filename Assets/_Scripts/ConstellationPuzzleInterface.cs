using UnityEngine;

public class ConstellationPuzzleInterface : InterfaceBase
{
    [SerializeField] private PuzzleChannelSO _puzzleChannel;
    [SerializeField] private Puzzle _puzzle;
    [SerializeField] private CutsceneManual _cutsceneManual;

    public override void Interact()
    {
        // Add zooming cutscene before raising event
        _cutsceneManual.PlayCutScene();

        _puzzleChannel.RaiseEvent(_puzzle);
    }
}
