using UnityEngine;
using UnityEngine.Events;

public class GameExitListener : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO _gameExitChannel = default;

    public UnityEvent OnEventRaised;

	private void OnEnable()
	{
		if (_gameExitChannel != null)
			_gameExitChannel.OnEventRaised += ExitGame;
	}

	private void OnDisable()
	{
		if (_gameExitChannel != null)
			_gameExitChannel.OnEventRaised -= ExitGame;
	}

	private void ExitGame()
	{
		if (OnEventRaised != null)
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}
}
