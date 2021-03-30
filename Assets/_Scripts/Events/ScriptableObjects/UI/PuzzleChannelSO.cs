using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Puzzel Event Channel")]
public class PuzzleChannelSO : ScriptableObject
{
	public UnityAction<Puzzle> OnEventRaised;
	
	public void RaiseEvent(Puzzle puzzle)
	{
		OnEventRaised?.Invoke(puzzle);
	}
}