using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    private ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    // Start is called before the first frame update
    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            SendDamageMessage(other);
        }

        //Rigidbody rb = other.GetComponent<Rigidbody>();
        //int i = 0;

        //while (i < numCollisionEvents)
        //{
        //    if (rb)
        //    {
        //        Vector3 pos = collisionEvents[i].intersection;
        //        Vector3 force = collisionEvents[i].velocity * 10;
        //        rb.AddForce(force);
        //    }
        //    i++;
        //}
        //}
    }

    void SendDamageMessage(GameObject other)
    {
        var d = other.GetComponent<Damageable>();
        if (!d)
            return;
        d.ReceiveAnAttack(3); //Fixed damage amount of 3, change later
    }
}
