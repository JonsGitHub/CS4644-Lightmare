using UnityEngine;

public enum ScreenState
{
    Game, Menu, Cutscene, EndCutscene
}

public class GameScreenManager : MonoBehaviour
{
    [Header("Listening on")]
    [SerializeField] private ScreenStateEventChannelSO _screenEventChannel;

    private bool _cutsceneLock = false;

    private void Awake()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (_screenEventChannel)
            _screenEventChannel.OnEventRaised += UpdateScreenState;
    }

    private void OnDisable()
    {
        if (_screenEventChannel)
            _screenEventChannel.OnEventRaised -= UpdateScreenState;
    }

    private void UpdateScreenState(ScreenState newState)
    {
        // Reset screen state
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);

        if (newState.Equals(ScreenState.EndCutscene)) // Recieved signal that the cutscene has ended so can transition back to game state
            _cutsceneLock = false;

        switch (newState)
        {
            case ScreenState.EndCutscene:
            case ScreenState.Game:
                if (_cutsceneLock) return; // Ignore and wait for the cutscene to handle exiting screen state.
                
                transform.Find("HealthBar3D").gameObject.SetActive(true);
                break;
            case ScreenState.Menu:
                break;
            case ScreenState.Cutscene:
                _cutsceneLock = true;
                break;
        }
    }
}
