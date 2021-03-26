using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

[CreateAssetMenu(menuName = "Events/Playable Director Channel")]
public class PlayableDirectorChannelSO : ScriptableObject
{
	public UnityAction<PlayableDirector> OnEventRaised;

	public void RaiseEvent(PlayableDirector playable)
	{
		Debug.Log("Playing Cutscene with " + playable);
		if (OnEventRaised != null)
			OnEventRaised.Invoke(playable);
	}
}