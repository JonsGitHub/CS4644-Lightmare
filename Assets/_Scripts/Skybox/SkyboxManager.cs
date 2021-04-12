using System.Collections;
using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    [SerializeField] private Material _blendedSkybox;
    [SerializeField] private Light _light;
    [SerializeField] private float _startIntensity;
    [SerializeField] private float _endIntensity;

    private const float _lerpSpeed = 0.41f;

    public void SwitchToSecondSkybox()
    {
        StartCoroutine(SwitchingSkybox(0, 1));
        StartCoroutine(LerpingLightIntensity(_startIntensity, _endIntensity));
    }

    public IEnumerator SwitchingSkybox(float start, float end)
    {
        float timeElapsed = 0;
        while (timeElapsed < _lerpSpeed)
        {
            _blendedSkybox.SetFloat("_Blend", Mathf.Lerp(start, end, timeElapsed / _lerpSpeed));
            timeElapsed += Time.deltaTime;
            yield return null;
            DynamicGI.UpdateEnvironment();
        }
        _blendedSkybox.SetFloat("_Blend", end);
    }

    public IEnumerator LerpingLightIntensity(float start, float end)
    {
        float timeElapsed = 0;
        while (timeElapsed < _lerpSpeed)
        {
            _light.intensity =  Mathf.Lerp(start, end, timeElapsed / _lerpSpeed);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _light.intensity = end;
    }
}
