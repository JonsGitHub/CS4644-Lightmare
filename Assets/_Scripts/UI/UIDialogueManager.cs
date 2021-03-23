using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;

public class UIDialogueManager : MonoBehaviour
{
	[SerializeField] private LocalizeStringEvent _lineText = default;

	[SerializeField] private LocalizeStringEvent _actorNameText = default;

	public void SetDialogue(LocalizedString dialogueLine, ActorSO actor)
	{
		_lineText.StringReference = dialogueLine;
		_actorNameText.StringReference = actor.ActorName;
	}
}