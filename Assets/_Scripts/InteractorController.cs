using UnityEngine;

/// <summary>
/// Controller containing the behaviour relating to interactors.
/// </summary>
public class InteractorController : MonoBehaviour
{
    #region Private Fields

    private CanvasController Canvas;
    private Transform HoldPosition;
    private Interaction Interaction;
    private InteractionType Current = InteractionType.None;

    private Rigidbody CurrentBody => Interaction.Body;
    private Conversation CurrentConversation => Interaction.Conversation;
    private GameObject CurrentObject => Interaction.Object;

    #endregion

    /// <summary>
    /// Awake called before Start of class
    /// </summary>
    private void Awake()
    {
        Canvas = FindObjectOfType<CanvasController>();
        HoldPosition = transform.Find("Hold");
    }

    /// <summary>
    /// Update called every frame
    /// </summary>
    private void Update()
    {
        if (Current == InteractionType.Conversation && Input.GetButtonDown("Interact"))
        {
            Canvas?.StartConversation(CurrentConversation);
        }

        if (Current == InteractionType.Grab)
        {
            if (Input.GetButton("Interact"))
            {
                CurrentBody.transform.position = HoldPosition.position;
                CurrentBody.transform.rotation = HoldPosition.rotation;

                CurrentBody.velocity = Vector3.zero;
                CurrentBody.useGravity = false;
            }
            else
            {
                CurrentBody.useGravity = true;
            }
        }
    }

    /// <summary>
    /// On trigger enter callback method.
    /// </summary>
    /// <param name="other">The collider that has entered the trigger</param>
    private void OnTriggerEnter(Collider other)
    {
        if (!CurrentObject && other.GetComponent<IInteractable>() is IInteractable interactable)
        {
            Canvas?.SetMessage("'E' to Interact");
            Current = interactable.Interact(ref Interaction);
        }
    }

    /// <summary>
    /// On trigger exit callback method.
    /// </summary>
    /// <param name="other">The collider that has exited the trigger</param>
    private void OnTriggerExit(Collider other)
    {
        if (CurrentObject && other.gameObject.Equals(CurrentObject))
        {
            // Re-activate gravity - in case clipped out of user's hand
            if (Current == InteractionType.Grab)
            {
                CurrentBody.useGravity = true;
            }
            Canvas?.ClearMessage();
            Current = InteractionType.None;
            Interaction.Clear();
        }
    }
}