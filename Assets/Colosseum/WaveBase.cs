using UnityEngine;

public abstract class WaveBase : MonoBehaviour
{
    [Header("Listening on channels")]
    [SerializeField]
    private BoolEventChannelSO _unlockingChannel = default;

    private bool _locked = true;

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
            _locked = false;
            Unlock();
        }
        else
        {
            _locked = true;
            Lock();
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
