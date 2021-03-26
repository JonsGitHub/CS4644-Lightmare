using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/// TODO: Determine whether this is applicable for our purposes
public class GoalManager : MonoBehaviour
{
    [SerializeField] private float _thresholdDistance = 2;

    [Header("Listening on channels")]
    [SerializeField] private VoidEventChannelSO _onSceneReady = default;

    [Tooltip("Stack of goal markers in progressive order")]
    public List<Transform> Markers;

    private PlayerController _player;
    private NavMeshPath _path = null;
    private LineRenderer _lineRenderer;
    
    private void OnEnable()
    {
        if (_onSceneReady)
        {
            _onSceneReady.OnEventRaised += SceneSetupInitialization;
        }
    }

    private void OnDisable()
    {
        if (_onSceneReady)
        {
            _onSceneReady.OnEventRaised -= SceneSetupInitialization;
        }
    }

    private void SceneSetupInitialization()
    {
        _player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
        _lineRenderer = GetComponent<LineRenderer>();

        if (Markers.Count > 0)
        {
            _path = new NavMeshPath();
            NavMesh.CalculatePath(_player.transform.position, Markers.First().position, NavMesh.AllAreas, _path);
        }
    }

    private void LateUpdate()
    {
        if (_path == null || Markers.Count == 0)
            return;
        
        NavMesh.CalculatePath(_player.transform.position, Markers.First().position, NavMesh.AllAreas, _path);

        if (_path.corners.Length == 0)
            return;

        _lineRenderer.positionCount = _path.corners.Length;
        _lineRenderer.SetPositions(_path.corners);

        var currentPosition = _player.transform.position;
        if (Vector3.Distance(currentPosition, Markers.First().position) < _thresholdDistance)
        {
            Markers.RemoveAt(0);
            if (Markers.Count == 0)
            {
                _lineRenderer.positionCount = 0;
            }
        }
    }
}
