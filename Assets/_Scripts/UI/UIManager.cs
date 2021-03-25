using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

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

	[Header("Visual Indicators Events")]
	[SerializeField] private UI3DEventChannelSO _3dUIChannelEvent = default;

	[Header("Interaction Events")]
	//[SerializeField] private VoidEventChannelSO _onInteractionEndedEvent = default; //TODO: Do we need?
	[SerializeField] private InteractionUIEventChannelSO _setInteractionEvent = default;

	[Header("Main Menu Loading")]
	[SerializeField] private GameSceneSO[] _menuToLoad = default;
	[SerializeField] private LoadEventChannelSO _loadMenu = default;

	private Canvas _canvas;
	private Canvas Canvas
    {
		get
        {
			if (_canvas == null)
            {
				_canvas = FindObjectOfType<Canvas>();
            }
			return _canvas;
        }
    }

	private List<Ui3D> Uis = new List<Ui3D>();
	private Stack<GameObject> ViewStack = new Stack<GameObject>();

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
		if (_3dUIChannelEvent)
        {
			_3dUIChannelEvent.OnEventRaised += Update3DUIs;
		}

		_inputReader.pauseEvent += Pause;
		_inputReader.menuUnpauseEvent += Unpause;

		_inputReader.EnableGameplayInput();
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

	public void AddToViewStack(GameObject layer)
    {
		// Set the top of the viewstack to inactive if it exists
		if (ViewStack.Count > 0)
			ViewStack.Peek().SetActive(false);

		layer.SetActive(true);
		ViewStack.Push(layer);
    }

	public void RemoveTopFromViewStack()
    {
		if (ViewStack.Count == 0)
			return;

		var obj = ViewStack.Pop();
		obj.SetActive(false);
		if (obj == PauseMenu)
        {
			_inputReader.EnableGameplayInput();
			_inputReader.EnableMouseCameraControlInput();
			Time.timeScale = 1;
			return;
		}

		if (ViewStack.Count != 0)
        {
			ViewStack.Peek().SetActive(true);
        }
    }

	/// <summary>
	/// On Save and Exit Button clicked callback method.
	/// </summary>
	public void OnSaveAndExitClicked()
	{
		Time.timeScale = 1;
		_inputReader.EnableMenuInput();
		ViewStack.Pop().SetActive(false); // Set Pause Menu to inactive
		_loadMenu.RaiseEvent(_menuToLoad, true);
	}

    /// <summary>
    /// On Settings Button clicked callback method.
    /// </summary>
    public void OnSettingsClicked()
	{
		AddToViewStack(SettingsMenu);
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

	private void Update3DUIs(Ui3D ui, bool remove)
    {
		if (!remove)
        {
			ui.transform.SetParent(Canvas.transform);
			ui.transform.SetAsFirstSibling();
			Uis.Add(ui);
		}
		else
        {
			Destroy(ui);
			Uis.Remove(ui);
		}
    }

	/// <summary>
	/// Helper method that resumes the normal operation of the game.
	/// </summary>
	private void Unpause()
	{
		RemoveTopFromViewStack();
	}

	/// <summary>
	/// Helper method that pauses the normal operation of the game.
	/// </summary>
	private void Pause()
	{
		_inputReader.EnableMenuInput();
		AddToViewStack(PauseMenu);
		Time.timeScale = 0;
	}
}