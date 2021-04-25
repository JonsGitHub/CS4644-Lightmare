using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Indicator Transform Event Channel")]
public class IndicatorTransformEventChannelSO: EventChannelBaseSO
{
	public UnityAction<Transform, bool> OnEventRaised;

	public void RaiseEvent(Transform value, bool remove)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value, remove);
	}
}