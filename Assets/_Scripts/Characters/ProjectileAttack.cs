﻿using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
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
        if (other.gameObject.TryGetComponent(out Damageable damageable))
        {
            damageable.ReceiveAnAttack(_attackConfigSO.AttackStrength);
        }
        other.rigidbody?.AddForceAtPosition(other.relativeVelocity * PushForce, other.GetContact(0).point);

        Destroy(gameObject);
    }
}
