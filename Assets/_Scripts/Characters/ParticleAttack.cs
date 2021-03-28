using System.Collections.Generic;
using UnityEngine;

public class ParticleAttack : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    public List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    [SerializeField] private AttackConfigSO _attackConfigSO;

    [Tooltip("Amount of pushing force."), Range(0.1f, 20f)]
    public float PushForce = 2.0f;

    void OnParticleCollision(GameObject other)
    {
        _particleSystem.GetCollisionEvents(other, collisionEvents);
        Damageable damageable;
        if (other.TryGetComponent(out damageable))
        {
            for(int i = 0; i < collisionEvents.Count; ++i)
            {
                damageable.ReceiveAnAttack(_attackConfigSO.AttackStrength);
            }
            return;
        }

        Rigidbody body;
        if (other.TryGetComponent(out body))
        {
            foreach(var collisionEvent in collisionEvents)
            {
                body.AddForceAtPosition(collisionEvent.velocity * PushForce, collisionEvent.intersection);
            }
        }
    }
}
