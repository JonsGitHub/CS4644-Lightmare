using UnityEngine;

/// <summary>
/// Npc controller outlining the behaviors relating
/// to a basic npc.
/// </summary>
public class NpcController : EntityController, IInteractable
{
    [Tooltip("The name of the NPC.")]
    public string Name;
    
    private Animator Animator;

    /// <summary>
    /// Start method called before first update.
    /// </summary>
    private void Start()
    {
        Animator = GetComponentInChildren<Animator>();

        Animator.SetFloat("velocity", -1.0f);
    }
    
    /// <inheritdoc/>
    public InteractionType Interact(ref Interaction interaction)
    {
        interaction.Object = gameObject;
        interaction.Conversation = new Conversation()
        {
            Line1 = "Hello I am " + Name
        };
        return InteractionType.Conversation;
    }
}
