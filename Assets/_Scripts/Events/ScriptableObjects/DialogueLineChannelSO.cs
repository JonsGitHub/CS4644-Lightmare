using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "Events/Dialogue line Channel")]
public class DialogueLineChannelSO : ScriptableObject
{
	public UnityAction<LocalizedString, ActorSO> OnEventRaised;
	public void RaiseEvent(LocalizedString line, ActorSO actor)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(line, actor);
	}
}