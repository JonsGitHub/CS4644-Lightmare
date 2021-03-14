using UnityEngine;

/// <summary>
/// Grabbable Object class containing the behaviour relevant
/// to objects that are picked up in 3d space.
/// </summary>
public class GrabbableObject : MonoBehaviour, IInteractable
{
    private Rigidbody RigidBody;

    /// <summary>
    /// Awake called before Start of class
    /// </summary>
    private void Awake()
    {
        RigidBody = GetComponent<Rigidbody>();
    }

    /// <inheritdoc/>
    public InteractionType Interact(ref Interaction interaction)
    {
        interaction.Object = gameObject;
        interaction.Body = RigidBody;
        return InteractionType.Grab;
    }
}
