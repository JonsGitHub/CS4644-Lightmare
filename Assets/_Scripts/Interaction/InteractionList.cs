using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionList
{
    private List<Interaction> _interactions =  new List<Interaction>();
    private int _selectedIndex = 0;
    private bool _isGrabbing = false;

    public IEnumerable<Interaction> Interactions => _interactions;

    public int SelectedIndex
    {
        get => _selectedIndex;
        set => _selectedIndex = value;
    }

    public bool IsGrabbing
    {
        get => _isGrabbing;
        set => _isGrabbing = value;
    }

    public Interaction this[int key]
    {
        get => _interactions[key];
    }

    public int Count => _interactions.Count;

    public Interaction Selected => _interactions.ElementAtOrDefault(_selectedIndex);

    public void Add(Interaction interaction) => _interactions.Add(interaction);

    public void Remove(Interaction interaction) => _interactions.Remove(interaction);

    public void Remove(GameObject gameObject) => _interactions.RemoveAll(x => x.interactableObject == gameObject);
}