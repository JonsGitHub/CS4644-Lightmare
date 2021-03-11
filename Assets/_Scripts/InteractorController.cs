using UnityEngine;

public class InteractorController : MonoBehaviour
{
    private Transform CurrentObject;
    private Transform HeldObject;
    private Transform HoldPosition;

    private void Awake()
    {
        HoldPosition = transform.Find("Hold");
        CurrentObject = null;
        HeldObject = null;
    }

    private void FixedUpdate()
    {
        if (CurrentObject && Input.GetButton("Interact"))
        {
            if (HeldObject == null)
                HeldObject = CurrentObject;

            HeldObject.position = HoldPosition.position;
            HeldObject.rotation = HoldPosition.rotation;
            var rigid = HeldObject.GetComponent<Rigidbody>();
            if (rigid)
            {
                rigid.velocity = Vector3.zero;
                rigid.useGravity = false;
            }
        }
        else if (HeldObject && !Input.GetButton("Interact"))
        {
            var rigid = HeldObject.GetComponent<Rigidbody>();
            if (rigid)
            {
                rigid.velocity = Vector3.zero;
                rigid.useGravity = true;
            }
            HeldObject = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CurrentObject == null && !other.CompareTag("Player"))
        {
            CurrentObject = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (CurrentObject && other.gameObject.Equals(CurrentObject.gameObject))
        {
            CurrentObject = null;
        }
    }
}
