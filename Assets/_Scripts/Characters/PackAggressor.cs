using UnityEngine;

public class PackAggressor : Aggressor
{
	[HideInInspector] public TransformEventChannelSO _packEventChannel = default;

    private void OnEnable()
    {
        if (_packEventChannel)
			_packEventChannel.OnEventRaised += TargetEnemy;
    }

    private void OnDisable()
    {
		if (_packEventChannel)
			_packEventChannel.OnEventRaised -= TargetEnemy;
	}

	public void TargetEnemy(Transform transform) => Attacked(transform.gameObject);
}