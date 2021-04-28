using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageablePart : MonoBehaviour
{
    [SerializeField] private Damageable _base;
    [SerializeField] float _damageMultiplier;

    public void ReceiveAnAttack(int damage)
    {
        _base.ReceiveAnAttack((int)(damage * _damageMultiplier));
    }
}
