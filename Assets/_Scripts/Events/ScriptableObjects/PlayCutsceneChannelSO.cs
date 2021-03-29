using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Playable Cutscene Channel")]
public class PlayCutsceneChannelSO : ScriptableObject
{
	public UnityAction<CutsceneController> OnEventRaised;

	public void RaiseEvent(CutsceneController playable)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(playable);
	}
}