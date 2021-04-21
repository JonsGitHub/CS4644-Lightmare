using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private AttackConfigSO _attackConfigSO;

    public AttackConfigSO AttackConfig => _attackConfigSO;
    private Vector3 shootDir;
    public void Setup(Vector3 dir)
    {
        shootDir = dir;
        Destroy(gameObject, 5);
    }

    void Update()
    {
        transform.position += shootDir * 20.0f * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Avoid friendly fire!
        if (other.gameObject.layer != 12)
        {
            if (other.TryGetComponent(out Damageable damageableComp))
            {
                if (!damageableComp.GetHit)
                {
                    damageableComp.ReceiveAnAttack(_attackConfigSO.AttackStrength);
                    Destroy(gameObject);
                }
            }
        }
    }
}
