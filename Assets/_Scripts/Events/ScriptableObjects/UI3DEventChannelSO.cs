using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/UI 3D Event Channel")]
public class UI3DEventChannelSO : ScriptableObject
{
	public UnityAction<Ui3D, bool> OnEventRaised;

	public void RaiseEvent(Ui3D ui, bool remove)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(ui, remove);
	}
}