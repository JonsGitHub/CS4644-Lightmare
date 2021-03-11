using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractorController : MonoBehaviour
{
    private Transform CurrentObject;
    private Transform HoldPosition;

    private void Awake()
    {
        HoldPosition = transform.Find("Hold");
        CurrentObject = null;
    }

    private void FixedUpdate()
    {
        if (CurrentObject != null && Input.GetButton("Interact"))
        {
            CurrentObject.position = HoldPosition.position;
            CurrentObject.rotation = HoldPosition.rotation;
        }

        if (CurrentObject != null && Input.GetButtonUp("Interact"))
        {
            CurrentObject = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CurrentObject == null && !other.CompareTag("Player"))
        {
            CurrentObject = other.transform;
        }
    }
}
