using UnityEngine.Events;
using UnityEngine;


[CreateAssetMenu(menuName = "Events/Interface Base Event Channel")]
public class InterfaceBaseEventChannelSO : ScriptableObject
{
	public UnityAction<InterfaceBase> OnEventRaised;
	public void RaiseEvent(InterfaceBase value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}
}