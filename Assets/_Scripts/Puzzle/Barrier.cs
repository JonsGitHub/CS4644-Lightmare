using UnityEngine;

public class Barrier : MonoBehaviour
{
    [Header("Listening on channels")]
    [SerializeField] private BoolEventChannelSO _unlockingChannel = default;

    private void OnEnable()
    {
        if (_unlockingChannel)
        {
            _unlockingChannel.OnEventRaised += Unlock;
        }
    }

    private void OnDisable()
    {
        if (_unlockingChannel)
        {
            _unlockingChannel.OnEventRaised -= Unlock;
        }
    }

    private void Awake()
    {
        gameObject?.SetActive(true);
    }

    private void Unlock(bool state)
    {
        if (true)
            gameObject?.SetActive(false);
    }
}
