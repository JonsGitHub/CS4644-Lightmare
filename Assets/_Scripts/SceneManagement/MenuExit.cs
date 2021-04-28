using UnityEngine;

public class MenuExit : MonoBehaviour
{
	[Header("Loading settings")]
	[SerializeField] private InputReader _inputReader;
	[SerializeField] private MenuSO[] _menuToLoad = default;

	[Header("Broadcasting on")]
	[SerializeField] private LoadEventChannelSO _loadMenuChannel = default;

	public void ManualTrigger() => LoadMenu();

	private void LoadMenu()
    {
		_inputReader.EnableMenuInput();
		_inputReader.DisableMouseCameraControlInput();
		_loadMenuChannel.RaiseEvent(_menuToLoad, true);
	}
}
