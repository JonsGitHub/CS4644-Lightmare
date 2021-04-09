using UnityEngine;

public class PulsingEmission : MonoBehaviour
{
    [SerializeField] private Renderer _meshRenderer = default;
    [SerializeField] private float _pulseRate = default;
    [SerializeField] private float _minEmission = default;
    [SerializeField] private float _maxEmission = default;

    private float _oscillateRange;
    private float _oscillateOffset;

    private void Start()
    {
        _oscillateRange = (_minEmission - _maxEmission) / 2f;
        _oscillateOffset = _oscillateRange + _maxEmission;
    }

    private void Update()
    {
        _meshRenderer.material.SetFloat("_EmissionIntensity", _oscillateOffset + Mathf.Sin(Time.time * _pulseRate) * _oscillateRange);
    }
}
