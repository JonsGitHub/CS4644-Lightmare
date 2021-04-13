using UnityEngine;

public class Tile : MonoBehaviour
{
    private int _x;
    private int _y;

    [SerializeField] private LayerMask _layers = default;
    [SerializeField] private MeshRenderer _renderer = default;
    [SerializeField] private Animator _animator = default;

    public int X => _x;
    public int Y => _y;

    private void Awake()
    {
        var coordinates = name.Replace("Tile_", "");
        _x = int.Parse("" + coordinates[0]);
        _y = int.Parse("" + coordinates[2]);
    }

    private void OnTriggerEnter(Collider other)
    {
        var puzzle = GetComponentInParent<FollowPathPuzzle>();
        if ((1 << other.gameObject.layer & _layers) != 0)
        {
            SetTileState(puzzle.StepOnTile(this, other.gameObject));
        }
    }

    public void FadeInIndicator() => _animator.Play("Indicator_FadeIn");
    public void FadeOutIndicator() => _animator.Play("Indicator_FadeOut");

    private void SetTileState(bool state)
    {
        if (state)
        {
            _renderer.material.SetColor("_MainColor", Color.green);
        }
        else
        {
            _renderer.material.SetColor("_MainColor", Color.red);
        }
    }
}
