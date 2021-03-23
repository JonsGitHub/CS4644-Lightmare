using UnityEngine;

public class QuestManager : MonoBehaviour
{
	[SerializeField] private QuestAnchorSO _questAnchor = default;

	void Start()
	{
		_questAnchor.StartGame();
	}
}