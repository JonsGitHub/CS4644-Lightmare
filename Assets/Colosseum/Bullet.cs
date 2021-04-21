using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
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

    void OnTriggerEnter(Collision other)
    {
        if (other.gameObject.layer == 10)
        {
            Debug.Log("Destroy");
            Destroy(this.gameObject);
        }
    }
}
