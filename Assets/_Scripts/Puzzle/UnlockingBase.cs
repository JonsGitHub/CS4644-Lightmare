using UnityEngine;

public abstract class UnlockingBase : MonoBehaviour
{
    [SerializeField] private int _totalLocks = default;
    private int _current = 0;
    private bool _locked = true;

    [Header("Listening on channels")]
    [SerializeField] private BoolEventChannelSO _unlockingChannel = default;

    [HideInInspector] public bool Locked => _locked;

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
                _locked = false;
                Unlock();
            }
        }
        else
        {
            if (_current == _totalLocks)
            {
                _locked = true;
                Lock();
            }
            _current--;
            if (_current < 0) { _current = 0; }
        }
    }

    public void SetLockState(bool locked)
    {
        _locked = locked;
        if (locked)
            Lock();
        else
            Unlock();
    }

    public abstract void Lock();

    public abstract void Unlock();
}
