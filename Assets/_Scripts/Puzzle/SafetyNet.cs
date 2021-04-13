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
        _currentCheckpoint = SafePosition.ElementAtOrDefault(index);
    }
}
