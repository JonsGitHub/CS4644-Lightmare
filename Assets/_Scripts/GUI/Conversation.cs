using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

public class Conversation
{
    private Stack<Dialogue> dialogues = new Stack<Dialogue>();
    private Dialogue current;

    public Conversation()
    {
    }

    public Conversation(Stack<Dialogue> dialogues)
    {
        this.dialogues = dialogues;
        Assert.IsTrue(this.dialogues.Count > 0, "Conversation must consist of atleast one line");
    }

    public Dialogue Next()
    {
        return current = dialogues.Pop();
    }

    public bool HasNext() => dialogues.Count > 0;
}

public struct Dialogue
{
    public string Speaker { get; set; }
    public string Speech { get; set; }
    public Action PostAction { get; set; }
}