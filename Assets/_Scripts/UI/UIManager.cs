using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public enum GameScreen
{
	Settings, NoticeBoard, CrystalAppendix
}

public class UIManager : MonoBehaviour
{
	private const float _lerpSpeed = 0.23f;

	private Color _reticleBaseColor = new Color(0.4716981f, 0.4716981f, 0.4716981f, 0.6862745f);
	private Color _reticleNoColor = new Color(0.4716981f, 0.4716981f, 0.4716981f, 0f);

	[SerializeField] private GameObject PauseMenu = default;
	[SerializeField] private GameObject SettingsMenu = default;
	[SerializeField] private GameObject NoticeBoard = default;
	[SerializeField] private GameObject CrystalAppendix = default;

	[SerializeField] private UIDialogueManager _dialogueController = default;
	[SerializeField] private UIInteractionManager _interactionPanel = default;

	[SerializeField] private InputReader _inputReader = default;

	[SerializeField] private TransformAnchor _playerTransformAnchor = default;
	[SerializeField] private HealthBar3D _playerHealthBar = default;
	[SerializeField] private IntEventChannelSO _playerHitChannel = default;

	[SerializeField] private Image _reticleImage = default;
	[SerializeField] private BoolEventChannelSO _aimEventChannel = default;

	[Tooltip("The Distance of occlusion of UI3D Objects")]
	[Range(0, 200)]
	[SerializeField] private int Ui3DOccludingDistance = 25;
	[SerializeField] private RectTransform _3dUiHolder = default;

	[Header("Listening on channels")]
	[Header("Dialogue Events")]
	[SerializeField] private DialogueLineChannelSO _openUIDialogueEvent = default;
	[SerializeField] private VoidEventChannelSO _closeUIDialogueEvent = default;

	[Header("Visual Indicators Events")]
	[SerializeField] private UI3DEventChannelSO _3dUIChannelEvent = default;
	[SerializeField] private GameScreenEventChannelSO _gameScreenEvent = default;

	[Header("Interaction Events")]
	[SerializeField] private InteractionUIEventChannelSO _setInteractionEvent = default;
	[SerializeField] private BoolEventChannelSO _requestUpdateInteraction = default;

	[Header("Player Based Events")]
	[SerializeField] private TransformEventChannelSO _playerInstantiatedChannel = default;
	
	[Header("Main Menu Loading")]
	[SerializeField] private GameSceneSO[] _menuToLoad = default;
	[SerializeField] private LoadEventChannelSO _loadMenu = default;

	[Header("Broadcasting on")]
	[SerializeField] private ScreenStateEventChannelSO _screenEventChannel;

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
		if (_playerInstantiatedChannel)
        {
			_playerInstantiatedChannel.OnEventRaised += GatherPlayerInformation;
        }
		if (_playerHitChannel)
        {
			_playerHitChannel.OnEventRaised += UpdatePlayerHealth;
        }
		if (_aimEventChannel)
        {
			_aimEventChannel.OnEventRaised += AimState;
        }
		if (_requestUpdateInteraction)
        {
			_requestUpdateInteraction.OnEventRaised += UpdateInteraction;
        }
		if (_gameScreenEvent)
        {
			_gameScreenEvent.OnEventRaised += UpdateScreen;
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
		if (_playerInstantiatedChannel)
		{
			_playerInstantiatedChannel.OnEventRaised += GatherPlayerInformation;
		}
		if (_playerHitChannel)
		{
			_playerHitChannel.OnEventRaised -= UpdatePlayerHealth;
		}
		if (_aimEventChannel)
		{
			_aimEventChannel.OnEventRaised -= AimState;
		}
		if (_requestUpdateInteraction)
		{
			_requestUpdateInteraction.OnEventRaised -= UpdateInteraction;
		}
		if (_gameScreenEvent)
		{
			_gameScreenEvent.OnEventRaised -= UpdateScreen;
		}

		_inputReader.pauseEvent -= Pause;
		_inputReader.menuUnpauseEvent -= Unpause;
	}

	private void GatherPlayerInformation(Transform transform)
    {
		_screenEventChannel?.RaiseEvent(ScreenState.Game);

		var playerDamageable = _playerTransformAnchor.Transform.GetComponent<Damageable>();
		_playerHealthBar.MaxHealth = playerDamageable.MaxHealth;
		_playerHealthBar.Health = playerDamageable.CurrentHealth;
	}

	private void Start()
	{
		CloseUIDialogue();
	}

	private void UpdatePlayerHealth(int current)
    {
		_playerHealthBar.Health = current;
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

	public void SetInteractionPanel(InteractionList list)
	{
		_interactionPanel.FillInteractionPanel(list);
	}

	private void UpdateInteraction(bool state)
    {
		_interactionPanel.UpdateList(state);
		_interactionPanel.gameObject.SetActive(state);
    }

	private void UpdateScreen(GameScreen screen)
	{
		Time.timeScale = 0;
		_inputReader.EnableMenuInput();
		_inputReader.DisableMouseCameraControlInput();

		switch (screen)
        {
			case GameScreen.NoticeBoard:
				AddToViewStack(NoticeBoard);
				break;
			case GameScreen.CrystalAppendix:
				AddToViewStack(CrystalAppendix);
				break;
			case GameScreen.Settings:
				AddToViewStack(SettingsMenu);
				break;
		}
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
		if (ViewStack.Count == 0)
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

	public void OnAppendixClicked()
    {
		AddToViewStack(CrystalAppendix);
    }

	/// <summary>
	/// Last update called every frame
	/// </summary>
	private void LateUpdate()
	{	
		if (_playerTransformAnchor.Transform == null)
			return;

		foreach (var ui in Uis)
		{
			if (Vector3.Distance(ui.Transform.position, _playerTransformAnchor.Transform.position) > Ui3DOccludingDistance)
            {
				ui.SetActive(false);
            }
			else
            {
				ui.SetActive(true);
				ui.UpdateUI();
            }
		}
	}

	private void Update3DUIs(Ui3D ui, bool remove)
    {
		if (!remove)
        {
			ui.transform.SetParent(_3dUiHolder);
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

	private void AimState(bool state)
	{
		if (state)
		{
			StartCoroutine(LerpColor(_reticleBaseColor));
		}
		else
		{
			StartCoroutine(LerpColor(_reticleNoColor));
		}
	}

	private IEnumerator LerpColor(Color endColor)
	{
		float timeElapsed = 0;
		var startColor = _reticleImage.color;
		while (timeElapsed < _lerpSpeed)
		{
			_reticleImage.color = Color.Lerp(startColor, endColor, timeElapsed / _lerpSpeed);
			timeElapsed += Time.deltaTime;

			yield return null;
		}
		_reticleImage.color = endColor;
	}
}