using UnityEngine;

public class SpawnerTrigger : MonoBehaviour
{
    [SerializeField] private Spawner _spawnerReference = default; 
    [SerializeField] private VoidEventChannelSO _triggerChannel = default;
    
    private void OnEnable()
    {
        if (_triggerChannel)
            _triggerChannel.OnEventRaised += TriggerSpawn; 
    }

    private void OnDisable()
    {
        if (_triggerChannel)
            _triggerChannel.OnEventRaised -= TriggerSpawn;
    }

    private void TriggerSpawn() => _spawnerReference.SingleSpawn();
}
