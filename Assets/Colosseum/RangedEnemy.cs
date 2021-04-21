using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [SerializeField]
    private TransformAnchor _playerTransform;

    [SerializeField]
    public float projSpeed = 10.0f;

    [SerializeField]
    public Transform projectile;

    // Start is called before the first frame update
    void Start()
    {
        float fireRate = Random.Range(1.0f, 2.0f);
        InvokeRepeating("Shoot", 2, fireRate);
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerTransform.isSet)
        {
            transform.LookAt(_playerTransform.Transform.position);
        }
    }

    void Shoot()
    {
        if (_playerTransform.isSet)
        {
            Vector3 adjustHeight = new Vector3(0, 1.5f, 0);
            Transform bulletTrans = Instantiate(projectile, transform.position + adjustHeight, transform.rotation);
            Vector3 adjustAim = new Vector3(0, 1, 0);
            Vector3 shootDir = ((_playerTransform.Transform.position + adjustAim) - transform.position).normalized;
            bulletTrans.GetComponent<Bullet>().Setup(shootDir);
        }
    }
}
