using System.Collections.Generic;
using UnityEngine;

public enum InteractionType { None = 0, Talk, Grab, Drop, Interact };

public class InteractionManager : MonoBehaviour
{
	[HideInInspector] public InteractionType currentInteractionType; //This is checked by conditions in the StateMachine
	[SerializeField] private InputReader _inputReader = default;

	[SerializeField] private Transform _holdPosition = default;

	//To store the object we are currently interacting with
	private LinkedList<Interaction> _potentialInteractions = new LinkedList<Interaction>();

	//Events for the different interaction types
	[Header("Broadcasting on")]
	[SerializeField] private DialogueActorChannelSO _startTalking = default;
	//UI event
	[SerializeField] private InteractionUIEventChannelSO _toggleInteractionUI = default;

	[Header("Listening to")]
	[SerializeField] private VoidEventChannelSO _onInteractionEnded = default;

	private GameObject grabbed;

	private void OnEnable()
	{
		_inputReader.interactEvent += OnInteractionButtonPress;
		_onInteractionEnded.OnEventRaised += OnInteractionEnd;
	}

	private void OnDisable()
	{
		_inputReader.interactEvent -= OnInteractionButtonPress;
		_onInteractionEnded.OnEventRaised -= OnInteractionEnd;
	}

    private void FixedUpdate()
    {
		// TODO: Move into player controller by listenting to grab interaction event
        if (grabbed)
        {
			var rigid = grabbed.GetComponent<Rigidbody>();
			if (rigid)
			{
				rigid.velocity = Vector3.zero;
				rigid.useGravity = false;
			}
			grabbed.transform.position = _holdPosition.position;
			grabbed.transform.rotation = _holdPosition.rotation;
		}
    }

    private void OnInteractionButtonPress()
	{
		if (_potentialInteractions.Count == 0)
			return;

		if (grabbed)
        {
			var rigid = grabbed.GetComponent<Rigidbody>();
			if (rigid)
			{
				rigid.useGravity = true;
			}

			grabbed = null;
			RequestUpdateUI(true);
			return;
        }

		currentInteractionType = _potentialInteractions.First.Value.type;
		switch (currentInteractionType)
		{
			case InteractionType.Grab:
				grabbed = _potentialInteractions.First.Value.interactableObject;
				RequestUpdateUI(true);
				break;
			case InteractionType.Talk:
				if (_startTalking != null)
				{
					_potentialInteractions.First.Value.interactableObject.GetComponent<StepController>().InteractWithCharacter();
					_inputReader.EnableDialogueInput();
				}
				break;
			case InteractionType.Interact:
				_potentialInteractions.First.Value.interactableObject.GetComponent<InterfaceBase>()?.Interact();
				break;
		}
	}

	//Called by the Event on the trigger collider on the child GO called "InteractionDetector"
	public void OnTriggerChangeDetected(bool entered, GameObject obj)
	{
		if (entered)
			AddPotentialInteraction(obj);
		else
			RemovePotentialInteraction(obj);
	}

	private void AddPotentialInteraction(GameObject obj)
	{
		Interaction newPotentialInteraction = new Interaction(InteractionType.None, obj);

		switch (obj.tag)
        {
			case "NPC":
				newPotentialInteraction.type = InteractionType.Talk;
				break;
			case "Grabbable":
				newPotentialInteraction.type = InteractionType.Grab;
				break;
			case "Interface":
				newPotentialInteraction.type = InteractionType.Interact;
				break;
        }
		
		if (newPotentialInteraction.type != InteractionType.None)
		{
			_potentialInteractions.AddFirst(newPotentialInteraction);
			RequestUpdateUI(true);
		}
	}

	private void RemovePotentialInteraction(GameObject obj)
	{
		LinkedListNode<Interaction> currentNode = _potentialInteractions.First;
		while (currentNode != null)
		{
			if (currentNode.Value.interactableObject == obj)
			{
				_potentialInteractions.Remove(currentNode);
				break;
			}
			currentNode = currentNode.Next;
		}

		RequestUpdateUI(_potentialInteractions.Count > 0);
	}

	private void RequestUpdateUI(bool visible)
	{
		if (visible)
			_toggleInteractionUI.RaiseEvent(true, grabbed ? InteractionType.Drop : _potentialInteractions.First.Value.type);
		else
			_toggleInteractionUI.RaiseEvent(false, InteractionType.None);
	}

	private void OnInteractionEnd()
	{
		switch (currentInteractionType)
		{
			case InteractionType.Interact:
			case InteractionType.Talk:
				//We show the UI after interacting or talking, in case player wants to interact again
				RequestUpdateUI(true);
				break;
		}

		_inputReader.EnableGameplayInput();
	}
}