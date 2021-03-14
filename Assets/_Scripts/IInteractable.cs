using UnityEngine;

/// <summary>
/// Struct representing the data relevant to the interaction.
/// </summary>
public struct Interaction
{
    /// <summary>
    /// Get or sets the GameObject being interacted with.
    /// </summary>
    public GameObject Object { get; set; }

    /// <summary>
    /// Gets or sets the RigidBody to grab.
    /// </summary>
    public Rigidbody Body { get; set; }

    /// <summary>
    /// Gets or sets the Conversation of the iteracted object.
    /// </summary>
    public Conversation Conversation { get; set; }

    /// <summary>
    /// Clears the information of the interaction.
    /// </summary>
    public void Clear()
    {
        Object = null;
        Body = null;
        Conversation = null;
    }
}

/// <summary>
/// Very primative conversation struct.
/// </summary>
public class Conversation
{
    public string Line1 { get; set; }
}

/// <summary>
/// Interface representing the required methods for an
/// interactable object.
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// Interacts with the object passing the relevant information.
    /// </summary>
    /// <param name="interaction">The struct to insert relevant information into</param>
    /// <returns>The type of conversation</returns>
    InteractionType Interact(ref Interaction interaction);
}

/// <summary>
/// Enum representing different types of interactions.
/// </summary>
public enum InteractionType
{
    None,
    Grab,
    Pickup,
    Conversation,
    Activation
}