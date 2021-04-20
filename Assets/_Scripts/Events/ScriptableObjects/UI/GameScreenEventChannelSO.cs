using UnityEngine.Events;
using UnityEngine;


[CreateAssetMenu(menuName = "Events/Game Screen Event Channel")]
public class GameScreenEventChannelSO : ScriptableObject
{
	public UnityAction<GameScreen> OnEventRaised;
	public void RaiseEvent(GameScreen value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}
}