using UnityEngine.Events;
using UnityEngine;


[CreateAssetMenu(menuName = "Events/Screen State Event Channel")]
public class ScreenStateEventChannelSO : ScriptableObject
{
	public UnityAction<ScreenState> OnEventRaised;
	public void RaiseEvent(ScreenState value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}
}