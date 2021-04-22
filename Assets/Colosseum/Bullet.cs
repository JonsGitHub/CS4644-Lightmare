using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private AttackConfigSO _attackConfigSO;
    private Vector3 shootDir;

    public void Setup(Vector3 dir)
    {
        shootDir = dir;
        Destroy(gameObject, 10);
    }

    void Update()
    {
        transform.position += shootDir * 20.0f * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.TryGetComponent(out Damageable damageable) && !damageable.GetHit)
        {
            damageable.ReceiveAnAttack(_attackConfigSO.AttackStrength);
        }
        Destroy(gameObject); // Destroy regardless of what it collides with
    }
}
