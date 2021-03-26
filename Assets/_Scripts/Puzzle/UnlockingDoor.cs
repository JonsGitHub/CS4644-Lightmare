using System.Collections;
using System.Linq;
using UnityEngine;

public class UnlockingDoor : UnlockingBase
{
    [Tooltip("Movement Config of the path to lock/unlock door position. Where the first is the locked position and second is the unlocked position.")]
    [SerializeField] private PathwayConfigSO _movementConfig;

    public override void Lock()
    {
        StartCoroutine(LerpPosition(_movementConfig.Waypoints.First().waypoint));
    }

    public override void Unlock()
    {
        StartCoroutine(LerpPosition(_movementConfig.Waypoints.Last().waypoint));
    }

    IEnumerator LerpPosition(Vector3 endPosition)
    {
        float timeElapsed = 0;
        var startPosition = transform.position;
        while (timeElapsed < _movementConfig.Speed)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, timeElapsed / _movementConfig.Speed);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        transform.position = endPosition;
    }
}
