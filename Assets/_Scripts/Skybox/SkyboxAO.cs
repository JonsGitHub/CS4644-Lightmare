using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// A version of a skybox manager that will only transition between the two
/// skybox effects upon getting closer to a center point.
/// </summary>
public class SkyboxAO : MonoBehaviour
{
    [SerializeField] private float _outerRadius = default;
    [SerializeField] private float _innerRadius = default;

    [SerializeField] private Material _blendedSkybox;
    [SerializeField] private Light _light;
    [SerializeField] private float _startIntensity;
    [SerializeField] private float _endIntensity;
    private float intensityDifference, bottomIntensity;

    private Transform _player = null;

    private float difference;

    private bool _flatlined = false;

    private void Awake()
    {
        difference = _outerRadius - _innerRadius;
        intensityDifference = Mathf.Abs(_endIntensity - _startIntensity);
        bottomIntensity = Mathf.Min(_startIntensity, _endIntensity);
        Assert.IsTrue(difference > 0);

        _flatlined = false;

        _blendedSkybox.SetFloat("_Blend", 0);
        DynamicGI.UpdateEnvironment();
    }

    public void OnTriggerChangeDetected(bool entered, GameObject obj)
    {
        if (entered)
        {
            if (obj.CompareTag("Player"))
            {
                _player = obj.transform;
            }
        }
        else
        {
            if (obj.CompareTag("Player"))
            {
                _player = null;
            }
        }
    }

    private void Update()
    {
        if (_player)
        {
            var distance = Vector3.Distance(transform.position, _player.position) - _innerRadius;
            var endRatio = distance < 0 ? 0 : distance / difference;

            if (!_flatlined)
            {
                _blendedSkybox.SetFloat("_Blend", Mathf.Clamp01(1 - endRatio));
                _light.intensity = (intensityDifference * endRatio) + bottomIntensity;
                DynamicGI.UpdateEnvironment();
            }
            
            if (endRatio == 0)
                _flatlined = true;
            else
                _flatlined = false;
        }
    }
}
