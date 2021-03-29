using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SafetyNet : MonoBehaviour
{
    [SerializeField] private List<Transform> SafePosition;

    private Transform _currentCheckpoint;

    private void Awake()
    {
        _currentCheckpoint = SafePosition.First();
    }

    public void Respawn(bool state, GameObject obj)
    {
        PlayerController _player;
        if (state && obj.TryGetComponent(out _player))
        {
            // Prevent application of movement to interrupt teleport
            _player.GetComponent<CharacterController>().enabled = false;
            _player.transform.position = _currentCheckpoint.position;
            _player.transform.rotation = _currentCheckpoint.rotation;
            _player.GetComponent<CharacterController>().enabled = true;
        }
    }

    public void SetCheckpoint(int index)
    {
        _currentCheckpoint = SafePosition.ElementAtOrDefault(index);
    }
}
