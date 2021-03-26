using UnityEngine;

/// TODO: Determine if this necessary for our purposes
public class QuestManager : MonoBehaviour
{
	[SerializeField] private QuestAnchorSO _questAnchor = default;

	void Start()
	{
		_questAnchor.StartGame();
	}
}