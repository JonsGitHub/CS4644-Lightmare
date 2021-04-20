using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private LayerMask _layers = default;
    [SerializeField] private Animator _animator = default;

    private void OnTriggerEnter(Collider other)
    {
        var puzzle = GetComponentInParent<FollowPathPuzzle>();
        if ((1 << other.gameObject.layer & _layers) != 0)
        {
            FadeInIndicator();
            puzzle.StepOnTile(this);
            //SetTileState(puzzle.StepOnTile(this, other.gameObject));
        }
    }

    public void FadeInIndicator() => _animator.Play("Fade_In");
    public void FadeOutIndicator() => _animator.Play("Fade_Out");
}
