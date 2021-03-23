using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[SerializeField] private GameObject PauseMenu = default;
	[SerializeField] private GameObject SettingsMenu = default;

	[SerializeField] private UIDialogueManager _dialogueController = default;
	[SerializeField] private UIInteractionManager _interactionPanel = default;
	
	[SerializeField] private InputReader _inputReader = default;
	[Header("Listening on channels")]
	[Header("Dialogue Events")]
	[SerializeField] private DialogueLineChannelSO _openUIDialogueEvent = default;
	[SerializeField] private VoidEventChannelSO _closeUIDialogueEvent = default;

	[Header("Interaction Events")]
	[SerializeField] private VoidEventChannelSO _onInteractionEndedEvent = default;
	[SerializeField] private InteractionUIEventChannelSO _setInteractionEvent = default;

	[Header("Main Menu Loading")]
	[SerializeField] private GameSceneSO[] _menuToLoad = default;
	[SerializeField] private LoadEventChannelSO _loadMenu = default;

	private List<Ui3D> Uis = new List<Ui3D>();

	private void OnEnable()
	{
		if (_openUIDialogueEvent)
		{
			_openUIDialogueEvent.OnEventRaised += OpenUIDialogue;
		}
		if (_closeUIDialogueEvent)
		{
			_closeUIDialogueEvent.OnEventRaised += CloseUIDialogue;
		}
		if (_setInteractionEvent)
		{
			_setInteractionEvent.OnEventRaised += SetInteractionPanel;
		}

		_inputReader.pauseEvent += Pause;
		_inputReader.menuUnpauseEvent += Unpause;
	}

    private void OnDisable()
    {
		if (_openUIDialogueEvent)
		{
			_openUIDialogueEvent.OnEventRaised -= OpenUIDialogue;
		}
		if (_closeUIDialogueEvent)
		{
			_closeUIDialogueEvent.OnEventRaised -= CloseUIDialogue;
		}
		if (_setInteractionEvent)
		{
			_setInteractionEvent.OnEventRaised -= SetInteractionPanel;
		}

		_inputReader.pauseEvent -= Pause;
		_inputReader.menuUnpauseEvent -= Unpause;
	}

	private void Start()
	{
		CloseUIDialogue();
	}

	public void OpenUIDialogue(LocalizedString dialogueLine, ActorSO actor)
	{
		_dialogueController.SetDialogue(dialogueLine, actor);
		_dialogueController.gameObject.SetActive(true);
	}
	public void CloseUIDialogue()
	{
		_dialogueController.gameObject.SetActive(false);
	}

	public void SetInteractionPanel(bool isOpenEvent, InteractionType interactionType)
	{
		if (isOpenEvent)
		{
			_interactionPanel.FillInteractionPanel(interactionType);
		}
		_interactionPanel.gameObject.SetActive(isOpenEvent);
	}

	/// <summary>
	/// On Continue Button clicked callback method.
	/// </summary>
	public void OnContinueClicked()
	{
		_inputReader.ManualUnpause();
	}

	/// <summary>
	/// On Save and Exit Button clicked callback method.
	/// </summary>
	public void OnSaveAndExitClicked()
	{
		Time.timeScale = 1; // Un pause the game without relocking the mouse
		Unpause();
		_loadMenu.RaiseEvent(_menuToLoad, true);
	}

    /// <summary>
    /// On Settings Button clicked callback method.
    /// </summary>
    public void OnSettingsClicked()
	{
		PauseMenu.SetActive(false);
		SettingsMenu.SetActive(true);
	}

	/// <summary>
	/// Last update called every frame
	/// </summary>
	private void LateUpdate()
	{
		foreach (var ui in Uis)
		{
			ui.UpdateUI();
		}
	}

	/// <summary>
	/// Adds a Ui3D to be rendered on the canvas
	/// </summary>
	/// <param name="ui">The Ui3D to be added</param>
	public void AddLabel(Ui3D ui)
	{
		ui.AttachTo(transform);
		Uis.Add(ui);
	}

	/// <summary>
	/// Removes a Ui3D from the canvas and destroys the instance.
	/// </summary>
	/// <param name="ui">The Ui3D to be removed</param>
	public void RemoveLabel(Ui3D ui)
	{
		Destroy(ui);
		Uis.Remove(ui);
	}

	/// <summary>
	/// Helper method that resumes the normal operation of the game.
	/// </summary>
	private void Unpause()
	{
		_inputReader.EnableGameplayInput();

		PauseMenu.SetActive(false);
		Time.timeScale = 1;
	}

	/// <summary>
	/// Helper method that pauses the normal operation of the game.
	/// </summary>
	private void Pause()
	{
		_inputReader.EnableMenuInput();

		PauseMenu.SetActive(true);
		Time.timeScale = 0;
	}
}