using System.Linq;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private PathwayConfigSO _movementConfig;

	private int _wayPointIndex = 0;
	private Vector3 _currentDestination;
	private Vector3 _startPosition;
	private float _timeElapsed, _waitTime;
	private Vector3 _delta;

	public Vector3 Delta => _delta;

    private void Awake()
    {
		_currentDestination = _movementConfig.Waypoints.First().waypoint;
		_startPosition = transform.position;
	}

    private void FixedUpdate()
    {
		if (_waitTime < _movementConfig.StopDuration)
        {
			_waitTime += Time.deltaTime;
			return;
        }

		if (_timeElapsed < _movementConfig.Speed)
        {
			var origin = transform.position;
            transform.position = Vector3.Lerp(_startPosition, _currentDestination, _timeElapsed / _movementConfig.Speed);
			_delta = transform.position - origin;
			_timeElapsed += Time.deltaTime;
		}
		else
        {
			_delta = Vector3.zero;
			transform.position = _currentDestination;
			_startPosition = transform.position;
			_currentDestination = GetNextDestination();
			_timeElapsed = 0;
			_waitTime = 0;
		}
	}

	private Vector3 GetNextDestination()
	{
		Vector3 result = transform.position;
		if (_movementConfig.Waypoints.Count > 0)
		{
			_wayPointIndex = (_wayPointIndex + 1) % _movementConfig.Waypoints.Count;
			result = _movementConfig.Waypoints[_wayPointIndex].waypoint;
		}
		return result;
	}
}
