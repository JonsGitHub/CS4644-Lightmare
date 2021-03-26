using System.Linq;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private PathwayConfigSO _movementConfig;

	private int _wayPointIndex = 0;
	private Vector3 _currentDestination;
	private Vector3 _startPosition;
	private float _timeElapsed, _waitTime;

    private void Awake()
    {
		_currentDestination = _movementConfig.Waypoints.First().waypoint;
		_startPosition = transform.position;
		if (Vector3.Distance(_startPosition, _currentDestination) < 0.1f)
        {
			_currentDestination = GetNextDestination();
        }
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
            transform.position = Vector3.Lerp(_startPosition, _currentDestination, _timeElapsed / _movementConfig.Speed);
			_timeElapsed += Time.deltaTime;
		}
		else
        {
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
