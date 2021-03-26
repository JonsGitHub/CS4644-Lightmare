using UnityEngine;

public abstract class UnlockingBase : MonoBehaviour
{
    [SerializeField] private int _totalLocks = default;
    private int _current = 0;

    [Header("Listening on channels")]
    [SerializeField] private BoolEventChannelSO _unlockingChannel = default;

    private void OnEnable()
    {
        if (_unlockingChannel)
        {
            _unlockingChannel.OnEventRaised += DetermineLockState;
        }
    }

    private void OnDisable()
    {
        if (_unlockingChannel)
        {
            _unlockingChannel.OnEventRaised -= DetermineLockState;
        }
    }

    private void DetermineLockState(bool activation)
    {
        if (activation)
        {
            _current++;
            if (_current == _totalLocks)
            {
                Unlock();
            }
        }
        else
        {
            if (_current == _totalLocks)
            {
                Lock();
            }
            _current--;
        }
    }

    public abstract void Lock();

    public abstract void Unlock();
}
