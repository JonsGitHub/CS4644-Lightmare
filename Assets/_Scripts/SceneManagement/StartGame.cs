using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class contains the function to call when play button is pressed
/// </summary>
public class StartGame : MonoBehaviour
{
	public LoadEventChannelSO onPlayButtonPress;
	public GameSceneSO[] locationsToLoad;
	public bool showLoadScreen;

	public void OnContinueButtonPress()
    {
		PlayerData.Load(); // Load in previous player data if it exists

		// Should load last scene played
		//onPlayButtonPress.RaiseEvent(locationsToLoad, showLoadScreen);
	}

	public void OnPlayButtonPress()
	{
		onPlayButtonPress.RaiseEvent(locationsToLoad, showLoadScreen);
	}

	public void OnResetSaveDataPress()
	{
		// TODO: Implement a Reset System
	}
}
