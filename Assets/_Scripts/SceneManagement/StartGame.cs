using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class contains the function to call when play button is pressed
/// </summary>
public class StartGame : MonoBehaviour
{
	[SerializeField] private VoidEventChannelSO _onSceneReady = default;

	public LoadEventChannelSO onPlayButtonPress;
	public GameSceneSO[] locationsToLoad;
	public bool showLoadScreen;

    private void Awake()
    {
		GetComponent<Button>().interactable = false;
		_onSceneReady.OnEventRaised += EnableInteraction;
	}

    private void OnDestroy()
    {
		_onSceneReady.OnEventRaised -= EnableInteraction;
    }

	public void OnPlayButtonPress()
	{
		onPlayButtonPress.RaiseEvent(locationsToLoad, showLoadScreen);
	}

	public void OnResetSaveDataPress()
	{
		// TODO: Implement a Reset System
	}

	private void EnableInteraction()
	{
		GetComponent<Button>().interactable = true;
	}
}
