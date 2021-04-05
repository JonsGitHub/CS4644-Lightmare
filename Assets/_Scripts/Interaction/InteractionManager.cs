using System.Collections.Generic;
using UnityEngine;

public enum InteractionType { None = 0, Talk, Grab, Drop, Interact };

public class InteractionManager : MonoBehaviour
{
	[HideInInspector] public InteractionType currentInteractionType; //This is checked by conditions in the StateMachine
	[SerializeField] private InputReader _inputReader = default;

	[SerializeField] private Transform _holdPosition = default;

	//To store the object we are currently interacting with
	private InteractionList _potentialInteractions = new InteractionList();

	//Events for the different interaction types
	[Header("Broadcasting on")]
	[SerializeField] private DialogueActorChannelSO _startTalking = default;
	//UI event
	[SerializeField] private InteractionUIEventChannelSO _toggleInteractionUI = default;
	[SerializeField] private BoolEventChannelSO _interactionDisplayEventChannel= default;
	[SerializeField] private BoolEventChannelSO _requestUpdateInteraction = default;

	[Header("Listening to")]
	[SerializeField] private VoidEventChannelSO _onInteractionEnded = default;

	private GameObject grabbed;

	private void OnEnable()
	{
		_inputReader.interactEvent += OnInteractionButtonPress;
		_onInteractionEnded.OnEventRaised += OnInteractionEnd;

		_toggleInteractionUI.RaiseEvent(_potentialInteractions);
	}

	private void OnDisable()
	{
		_inputReader.interactEvent -= OnInteractionButtonPress;
		_onInteractionEnded.OnEventRaised -= OnInteractionEnd;

		if (grabbed)
		{
			var rigid = grabbed.GetComponent<Rigidbody>();
			if (rigid)
			{
				rigid.useGravity = true;
			}

			_potentialInteractions.IsGrabbing = false;
			grabbed = null;
			RequestUpdateUI(true);
		}
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

			_potentialInteractions.IsGrabbing = false;
			grabbed = null;
			RequestUpdateUI(true);
			return;
        }

		currentInteractionType = _potentialInteractions.Selected.type;
		switch (currentInteractionType)
		{
			case InteractionType.Grab:
				grabbed = _potentialInteractions.Selected.interactableObject;
				_potentialInteractions.IsGrabbing = true;
				RequestUpdateUI(true);
				break;
			case InteractionType.Talk:
				if (_startTalking != null)
				{
					_potentialInteractions.Selected.interactableObject.GetComponent<StepController>().InteractWithCharacter();
					_inputReader.EnableDialogueInput();
				}
				break;
			case InteractionType.Interact:
				_potentialInteractions.Selected.interactableObject.GetComponent<InterfaceBase>()?.Interact();
				break;
		}
	}

	//Called by the Event on the trigger collider on the child called "InteractionDetector"
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
			_potentialInteractions.Add(newPotentialInteraction);
			RequestUpdateUI(true);
		}
	}

	private void RemovePotentialInteraction(GameObject obj)
	{
		_potentialInteractions.Remove(obj);
		RequestUpdateUI(_potentialInteractions.Count > 0);
	}

	private void RequestUpdateUI(bool visible)
	{
		if (visible)
        {
			// Request update
			_requestUpdateInteraction.RaiseEvent(true);
			_interactionDisplayEventChannel.RaiseEvent(true);
        }
		else
        {
			_requestUpdateInteraction.RaiseEvent(true);
			_interactionDisplayEventChannel.RaiseEvent(false);
		}
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