using UnityEngine;

public class SafetyNet : MonoBehaviour
{
    [SerializeField] private Transform SafePosition;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController _player;
        if (other.TryGetComponent(out _player))
        {
            // Prevent application of movement to interrupt teleport
            _player.GetComponent<CharacterController>().enabled = false;
            _player.transform.position = SafePosition.position;
            _player.transform.rotation = SafePosition.rotation;
            _player.GetComponent<CharacterController>().enabled = true;
        }
    }
}
