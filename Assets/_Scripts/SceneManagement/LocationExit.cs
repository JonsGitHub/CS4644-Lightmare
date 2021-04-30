using UnityEngine;

/// <summary>
/// This class goes on a trigger which, when entered, sends the player to another Location
/// </summary>

public class LocationExit : InterfaceBase
{
	[Header("Loading settings")]
	[SerializeField] private GameSceneSO[] _locationsToLoad = default;
	[SerializeField] private bool _showLoadScreen = default;
	[SerializeField] private PathAnchor _pathTaken = default;
	[SerializeField] private PathSO _exitPath = default;

	[Header("Broadcasting on")]
	[SerializeField] private LoadEventChannelSO _locationExitLoadChannel = default;

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent(out PlayerController player))
			LoadLocation();
	}


	public void ManualTrigger() => LoadLocation();

	public override void Interact() => LoadLocation();

	private void LoadLocation()
    {
		UpdatePathTaken();
		_locationExitLoadChannel.RaiseEvent(_locationsToLoad, _showLoadScreen);
	}

	private void UpdatePathTaken()
	{
		if (_pathTaken != null)
			_pathTaken.Path = _exitPath;
	}
}
