using System.Collections.Generic;
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
    private CanvasController Canvas;

    private Panel3D Label;

    /// <summary>
    /// Start method called before first update.
    /// </summary>
    private void Start()
    {
        Animator = GetComponentInChildren<Animator>();

        Label = Instantiate(Resources.Load<Panel3D>("Prefabs/Panel3D"));
        Label.Text = Name;
        Label.Transform = transform.Find("LabelPosition");
        
        Canvas = FindObjectOfType<CanvasController>();
        Canvas?.AddLabel(Label);

        Animator.SetFloat("velocity", -1.0f);
    }

    /// <inheritdoc/>
    public InteractionType Interact(ref Interaction interaction)
    {
        interaction.Object = gameObject;

        var dialogues = new Stack<Dialogue>();
        dialogues.Push(new Dialogue() { Speaker = Name, Speech = "Hello I am " + Name });
        var conversation = new Conversation(dialogues);
        interaction.Conversation = conversation;
        return InteractionType.Conversation;
    }
}
