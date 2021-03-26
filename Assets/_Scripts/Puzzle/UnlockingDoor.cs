using System.Collections;
using UnityEngine;

public class UnlockingDoor : UnlockingBase
{
    [SerializeField] private Transform OpenPosition;
    [SerializeField] private Transform ClosedPosition;
    [SerializeField] private float transitionDuration = 1.5f;

    private Vector3 openPos;
    private Vector3 closedPos;

    private void Awake()
    {
        openPos = OpenPosition.position;
        closedPos = ClosedPosition.position;
    }

    public override void Lock()
    {
        StartCoroutine(LerpPosition(closedPos));
    }

    public override void Unlock()
    {
        StartCoroutine(LerpPosition(openPos));
    }

    IEnumerator LerpPosition(Vector3 endPosition)
    {
        float timeElapsed = 0;
        var startPosition = transform.position;
        while (timeElapsed < transitionDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, timeElapsed / transitionDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        transform.position = endPosition;
    }
}
