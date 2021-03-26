using UnityEngine;

public class NPCMovementConfigSO : ScriptableObject
{
	[Tooltip("Waypoint stop duration")]
	[SerializeField] private float _stopDuration;

	[Tooltip("Roaming speed")]
	[SerializeField] private float _speed;

	public float Speed => _speed;
	public float StopDuration => _stopDuration;
}