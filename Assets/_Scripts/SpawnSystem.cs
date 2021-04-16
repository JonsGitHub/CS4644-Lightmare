using System;
using System.Linq;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
	[Header("Asset References")]
	[SerializeField] private PlayerController _playerPrefab = default;
	[SerializeField] private TransformAnchor _playerTransformAnchor = default;
	[SerializeField] private TransformEventChannelSO _playerInstantiatedChannel = default;
	[SerializeField] private PathAnchor _pathTaken = default;
	[SerializeField] private bool _spawnPlayer = true;

	[Header("Scene References")]
	private Transform[] _spawnLocations;

	[Header("Scene Ready Event")]
	[SerializeField] private VoidEventChannelSO _OnSceneReady = default; //Raised when the scene is loaded and set active

	private int _defaultSpawnIndex = 0;

	private void OnEnable()
	{
		if (_OnSceneReady != null && _spawnPlayer)
		{
			_OnSceneReady.OnEventRaised += SpawnPlayer;
		}
	}

	private void OnDisable()
	{
		if (_OnSceneReady != null && _spawnPlayer)
		{
			_OnSceneReady.OnEventRaised -= SpawnPlayer;
		}
	}

	private void SpawnPlayer()
	{
		GameObject[] spawnLocationsGO = GameObject.FindGameObjectsWithTag("SpawnLocation");

		_spawnLocations = new Transform[spawnLocationsGO.Length];
		for (int i = 0; i < spawnLocationsGO.Length; ++i)
		{
			_spawnLocations[i] = spawnLocationsGO[i].transform;
		}
		Spawn(FindSpawnIndex(_pathTaken?.Path ?? null));
	}

	void Reset()
	{
		AutoFill();
	}

	/// <summary>
	/// This function tries to autofill some of the parameters of the component, so it's easy to drop in a new scene
	/// </summary>
	[ContextMenu("Attempt Auto Fill")]
	private void AutoFill()
	{
		if (_spawnLocations == null || _spawnLocations.Length == 0)
			_spawnLocations = transform.GetComponentsInChildren<Transform>(true).Where(t => t != this.transform).ToArray();
	}

	private void Spawn(int spawnIndex)
	{
		Transform spawnLocation = GetSpawnLocation(spawnIndex, _spawnLocations);
		PlayerController playerInstance = InstantiatePlayer(_playerPrefab, spawnLocation);

		var lookAtTransform = playerInstance.transform;
		foreach (Transform child in playerInstance.transform)
		{
			if (child.CompareTag("CameraTarget"))
            {
				lookAtTransform = child;
				break;
            }
		}

		_playerTransformAnchor.Transform = playerInstance.transform;
		_playerInstantiatedChannel.RaiseEvent(lookAtTransform); // The CameraSystem will pick this up to frame the player

		// Set Player Health to saved health
		if (PlayerData.CurrentHealth != 0)
			playerInstance.GetComponent<Damageable>()?.SetHealth(PlayerData.CurrentHealth);
	}

	private Transform GetSpawnLocation(int index, Transform[] spawnLocations)
	{
		if (spawnLocations == null || spawnLocations.Length == 0)
			throw new Exception("No spawn locations set.");

		index = Mathf.Clamp(index, 0, spawnLocations.Length - 1);
		return spawnLocations[index];
	}

	private int FindSpawnIndex(PathSO pathTaken)
	{
		if (pathTaken == null)
        {
			if (!_spawnLocations[_defaultSpawnIndex].name.Equals("DefaultLocation"))
			{
				_defaultSpawnIndex = Array.FindIndex(_spawnLocations, element => element.name.Equals("DefaultLocation"));
			}
			return _defaultSpawnIndex;
        }

		int index = Array.FindIndex(_spawnLocations, element =>
			element?.GetComponent<LocationEntrance>()?.EntrancePath == pathTaken
		);

		return (index < 0) ? _defaultSpawnIndex : index;
	}

	private PlayerController InstantiatePlayer(PlayerController playerPrefab, Transform spawnLocation)
	{
		if (playerPrefab == null)
			throw new Exception("Player Prefab can't be null.");

		PlayerController playerInstance = Instantiate(playerPrefab, spawnLocation.position, spawnLocation.rotation);

		return playerInstance;
	}
}
