using System.Collections.Generic;
using UnityEngine;

public class IndicatorManager : MonoBehaviour
{
    [SerializeField] private GameObject _offScreenIndicator;
    [SerializeField] private IndicatorTransformEventChannelSO _indicatorTransformEvent;
    [SerializeField] private float _screenPadding;

    private class EnemyIndicator
    {
        public GameObject Indicator;
        public Transform Target;
    }

    private List<EnemyIndicator> _tracked = new List<EnemyIndicator>();

    private void OnEnable()
    {
        if (_indicatorTransformEvent)
        {
            _indicatorTransformEvent.OnEventRaised += OnIndicatorTransform;
        }
    }

    private void OnDisable()
    {
        if (_indicatorTransformEvent)
        {
            _indicatorTransformEvent.OnEventRaised -= OnIndicatorTransform;
        }
    }

    private void OnIndicatorTransform(Transform transform, bool tracking)
    {
        if (tracking)
            TrackTransform(transform);
        else
            RemoveTransform(transform);
    }

    private void TrackTransform(Transform target)
    {
        var offscreenIndicator = Instantiate(_offScreenIndicator, transform);

        var indicator = new EnemyIndicator()
        {
            Indicator = offscreenIndicator,
            Target = target
        };
        _tracked.Add(indicator);
    }

    private void RemoveTransform(Transform transform)
    {
        var find = _tracked.Find(x => x.Target != null && x.Target.gameObject == transform.gameObject);
        if (find != null)
        {
            Destroy(find.Indicator);
            _tracked.Remove(find);
        }
    }

    private void LateUpdate()
    {
        var screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2.0f;

        foreach (var indicator in _tracked)
        {
            if (indicator.Target == null)
            {
                indicator.Indicator.SetActive(false);
                continue;
            }

            var screenPosition = Camera.main.WorldToScreenPoint(indicator.Target.position);

            if (screenPosition.z >= 0 && screenPosition.x >= 0 && screenPosition.x <= Screen.width
                                     && screenPosition.y >= 0 && screenPosition.y <= Screen.height)
            {
                indicator.Indicator.SetActive(false);
                continue;
            }
            else
            {
                if (screenPosition.z < 0) // behind the camera
                    screenPosition *= -1;

                screenPosition -= screenCenter;
                var indicatorAngle = Vector3.SignedAngle(Vector3.up, screenPosition, Vector3.forward);

                // Calculate vector to determine which border axis intersects first
                float divX = ((Screen.width * 0.5f) - _screenPadding) / Mathf.Abs(screenPosition.x);
                float divY = ((Screen.height * 0.5f) - _screenPadding) / Mathf.Abs(screenPosition.y);

                // x-border first - put x-one to border and adjust y-one accordingly
                if (divX < divY)
                {
                    float angle = Vector3.SignedAngle(Vector3.right, screenPosition, Vector3.forward);
                    screenPosition.x = Mathf.Sign(screenPosition.x) * ((Screen.width * 0.5f) - _screenPadding);
                    screenPosition.y = Mathf.Tan(Mathf.Deg2Rad * angle) * screenPosition.x;
                }
                else // y-border first - put y-one to border and adjust x-one accordingly
                {
                    float angle = Vector3.SignedAngle(Vector3.up, screenPosition, Vector3.forward);
                    screenPosition.y = Mathf.Sign(screenPosition.y) * ((Screen.height * 0.5f) - _screenPadding);
                    screenPosition.x = -Mathf.Tan(Mathf.Deg2Rad * angle) * screenPosition.y;
                }

                indicator.Indicator.transform.position = screenPosition + screenCenter;
                indicator.Indicator.transform.rotation = Quaternion.Euler(new Vector3(0, 0, indicatorAngle));
                indicator.Indicator.SetActive(true);
            }
        }
    }
}
