using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SafetyNet : MonoBehaviour
{
    [Serializable]
    public struct SafetyPoint
    {
        public Transform Position;
        public GameObject Trigger;
    }

    [SerializeField] private List<SafetyPoint> SafePosition;

    private Transform _currentCheckpoint;

    private int _index;

    public int CurrentIndex => _index;

    private void Awake()
    {
        _currentCheckpoint = SafePosition.First().Position;
    }

    public void Respawn(bool state, GameObject obj)
    {
        PlayerController _player;
        if (state && obj.TryGetComponent(out _player))
        {
            // Drop anything in their hand
            _player.GetComponent<InteractionManager>().ForceDrop();

            // Prevent application of movement to interrupt teleport
            var controller = _player.GetComponent<CharacterController>();
            controller.enabled = false;
            _player.transform.position = _currentCheckpoint.position;
            _player.transform.rotation = _currentCheckpoint.rotation;
            controller.enabled = true;
        }
    }

    public void SetCheckpoint(int index)
    {
        _index = index;
        _currentCheckpoint = SafePosition.ElementAtOrDefault(index).Position;

        var current = index;
        while (current > 0)
        {
            var point = SafePosition.ElementAt(index);
            if (point.Trigger != null)
            {
                Destroy(point.Trigger);
            }
            current--;
        }
    }
}
