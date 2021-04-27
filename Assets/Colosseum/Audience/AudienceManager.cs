using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceManager : MonoBehaviour
{
    [SerializeField] private TransformAnchor _playerTransform;
    private Animation anim;
    private float animCountdown;

    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
        animCountdown = Random.Range(0.0f, 30.0f);
    }
    // Update is called once per frame
    void Update()
    {
        animCountdown -= Time.deltaTime;
        if (animCountdown <= 0.0f)
        {
            animCountdown = Random.Range(0.0f, 30.0f);
            if (!anim.isPlaying)
            {
                int rand = Random.Range(0, 6);
                switch (rand)
                {
                    case 0:
                        anim.Play("idle");
                        break;
                    case 1:
                        anim.Play("applause");
                        break;
                    case 2:
                        anim.Play("applause2");
                        break;
                    case 3:
                        anim.Play("celebration");
                        break;
                    case 4:
                        anim.Play("celebration2");
                        break;
                    case 5:
                        anim.Play("celebration3");
                        break;
                }
            }
        }
        if (_playerTransform.isSet)
        {
            Vector3 rot = new Vector3(_playerTransform.Transform.position.x, transform.position.y, _playerTransform.Transform.position.z);
            transform.LookAt(rot);
        }
    }
}
