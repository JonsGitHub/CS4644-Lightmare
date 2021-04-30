using UnityEngine;

public class ColosseumCanvasController : MonoBehaviour
{
    [SerializeField] private GameObject _bossHealthBar;

    private GameObject _bar;

    private void OnEnable()
    {
        _bar = Instantiate(_bossHealthBar, transform);
        _bar.name = "Boss_HealthBar";
    }

    private void OnDisable()
    {
        Destroy(_bar);
    }
}
