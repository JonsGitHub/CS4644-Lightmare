using UnityEngine;

/// <summary>
/// Controller containing the behaviour relating to interactors.
/// </summary>
public class InteractorController : MonoBehaviour
{
    #region Private Fields
    
    private Transform CurrentObject;
    private Transform HeldObject;
    private Transform HoldPosition;

    #endregion

    /// <summary>
    /// Awake called before Start of class
    /// </summary>
    private void Awake()
    {
        HoldPosition = transform.Find("Hold");
        CurrentObject = null;
        HeldObject = null;
    }

    /// <summary>
    /// Update called every physics frame
    /// </summary>
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

    /// <summary>
    /// On trigger enter callback method.
    /// </summary>
    /// <param name="other">The collider that has entered the trigger</param>
    private void OnTriggerEnter(Collider other)
    {
        if (CurrentObject == null && !other.CompareTag("Player"))
        {
            CurrentObject = other.transform;
        }
    }

    /// <summary>
    /// On trigger exit callback method.
    /// </summary>
    /// <param name="other">The collider that has exited the trigger</param>
    private void OnTriggerExit(Collider other)
    {
        if (CurrentObject && other.gameObject.Equals(CurrentObject.gameObject))
        {
            CurrentObject = null;
        }
    }
}
