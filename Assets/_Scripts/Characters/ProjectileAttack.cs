using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    [SerializeField] private GameObject _hitImpact;
    [SerializeField] private AttackConfigSO _attackConfigSO;
    [SerializeField] private float _projectileSpeed = 30;
    [SerializeField] private float _lifeSpan = 15;

    [Tooltip("Amount of pushing force."), Range(0.1f, 20f)]
    public float PushForce = 2.0f;

    private void Awake()
    {
        Destroy(gameObject, _lifeSpan);
    }

    public void Fire(Vector3 destination)
    {
        transform.LookAt(destination);
        GetComponent<Rigidbody>().velocity = (destination - transform.position).normalized * _projectileSpeed;
        GetComponent<Cinemachine.CinemachineImpulseSource>()?.GenerateImpulse(Camera.main.transform.forward);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (tag.Equals(other.gameObject.tag))
            return;

        var contact = other.GetContact(0);

        // Give damage to damageable if found
        if (other.gameObject.TryGetComponent(out Damageable damageable))
        {
            damageable.ReceiveAnAttack(_attackConfigSO.AttackStrength);
        }
        else if (other.gameObject.TryGetComponent(out DamageablePart part))
        {
            part.ReceiveAnAttack(_attackConfigSO.AttackStrength);
        }

        // Add force at the collision contact point to give "impact"
        other.rigidbody?.AddForceAtPosition(other.relativeVelocity * PushForce, contact.point);
        
        // Spawn hit impact effect if avaliable at contact point's normal
        if (_hitImpact)
        {
            var impact = Instantiate(_hitImpact, contact.point, Quaternion.FromToRotation(Vector3.up, contact.normal));
            Destroy(impact, 2); // Clean up hit impact
        }
        Destroy(gameObject); // Clean up
    }
}
