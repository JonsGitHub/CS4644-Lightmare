using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    [SerializeField] private AttackConfigSO _attackConfigSO;

    [Tooltip("Amount of pushing force."), Range(0.1f, 20f)]
    public float PushForce = 2.0f;

    private void Awake()
    {
        Destroy(gameObject, 15);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Damageable damageable))
        {
            damageable.ReceiveAnAttack(_attackConfigSO.AttackStrength);
        }
        other.rigidbody?.AddForceAtPosition(other.relativeVelocity * PushForce, other.GetContact(0).point);

        Destroy(gameObject);
    }
}
