using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This class contains the function to call when play button is pressed
/// </summary>
public class StartGame : MonoBehaviour
{
	[SerializeField] private VoidEventChannelSO _onSceneReady = default;

	public LoadEventChannelSO onPlayButtonPress;
	public GameSceneSO[] locationsToLoad;
	public bool showLoadScreen;

	//public SaveSystem saveSystem;
	public TextMeshProUGUI startText;

	//public Button resetSaveDataButton;
	private bool _hasSaveData = false;

    private void Awake()
    {
		GetComponent<Button>().interactable = false;

		_onSceneReady.OnEventRaised += EnableInteraction;
	}

	private void Start()
	{
		//_hasSaveData = saveSystem.LoadSaveDataFromDisk();

		if (_hasSaveData)
		{
			startText.text = "Continue";
			//resetSaveDataButton.gameObject.SetActive(true);
		}
		else
		{
			//resetSaveDataButton.gameObject.SetActive(false);
		}
	}

    private void OnDestroy()
    {
		_onSceneReady.OnEventRaised -= EnableInteraction;
    }

	public void OnPlayButtonPress()
	{
		if (!_hasSaveData)
		{
			//saveSystem.WriteEmptySaveFile();
			//Start new game
			onPlayButtonPress.RaiseEvent(locationsToLoad, showLoadScreen);
		}
		else
		{
			//Load Game
			StartCoroutine(LoadSaveGame());
		}
	}

	public void OnResetSaveDataPress()
	{
		_hasSaveData = false;
		startText.text = "Play";
		//resetSaveDataButton.gameObject.SetActive(false);
	}

	public IEnumerator LoadSaveGame()
	{
		//yield return StartCoroutine(saveSystem.LoadSavedInventory());

		//var locationGuid = saveSystem.saveData._locationId;
		//var asyncOperationHandle = Addressables.LoadAssetAsync<LocationSO>(locationGuid);
		//yield return asyncOperationHandle;
		//if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
		//{
		//	var locationSo = asyncOperationHandle.Result;
		//	onPlayButtonPress.RaiseEvent(new[] { (GameSceneSO)locationSo }, showLoadScreen);
		//}
		yield return null;
	}

	private void EnableInteraction()
	{
		GetComponent<Button>().interactable = true;
	}
}
