using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private int _unlockId = default;
    [SerializeField] private int _requiredAmount = 1;

    [Header("Broadcasting on")]
    [SerializeField] private BoolEventChannelSO _unlockChannel = default;
    [SerializeField] private VoidEventChannelSO _incorrectEventChannel = default;

    private int _currentAmount = 0;

    private void OnTriggerEnter(Collider other)
    {
        Key key;
        if (other.TryGetComponent(out key))
        {
            if (key.ID == _unlockId)
            {
                _currentAmount++;
                if (_currentAmount == _requiredAmount)
                {
                    _unlockChannel.RaiseEvent(_currentAmount >= _requiredAmount);
                }
            }
            else
            {
                if (_incorrectEventChannel)
                    _incorrectEventChannel.RaiseEvent();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Key key;
        if (other.TryGetComponent(out key) && key.ID == _unlockId)
        {
            if (_currentAmount == _requiredAmount)
            {
                _unlockChannel.RaiseEvent(false);
            }
            _currentAmount--;
        }
    }
}
