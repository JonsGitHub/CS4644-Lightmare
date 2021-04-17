using UnityEngine;

public class PackAggressor : Aggressor
{
	[SerializeField] public TransformEventChannelSO _packEventChannel = default;

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

	public override void FoundTarget() { _packEventChannel.RaiseEvent(currentTarget.transform); }


	public void TargetEnemy(Transform transform) => Attacked(transform.gameObject);
}