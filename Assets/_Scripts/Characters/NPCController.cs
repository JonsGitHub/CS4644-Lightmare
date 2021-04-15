using UnityEngine;

public class NPCController : MonoBehaviour
{
	[SerializeField] private NPCMovementConfigSO _npcMovementConfig;
	[SerializeField] private NPCMovementEventChannelSO _channel;

	[Header("Broadcasting on channels")]
	[SerializeField] private UI3DEventChannelSO _3dUIChannelEvent = default;

	private Panel3D label;

	[Header("Label Properties")]
	[Tooltip("Flag whether to create a label.")]
	[SerializeField] private bool _createLabel;
	[Tooltip("The name of the NPC.")]
	[SerializeField] private string Name;
	[Tooltip("The location of the overhead label.")]
	[SerializeField] private Transform LabelPosition = default;
	[Tooltip("The color of the text label.")]
	[SerializeField] private Color LabelTextColor = Color.black;

	public NPCMovementConfigSO NPCMovementConfig => _npcMovementConfig;
	[HideInInspector] public NPCMovementAction _currentAction = default;
	
	private void OnEnable()
	{
		if (_createLabel)
		{
			label = Instantiate(Resources.Load<Panel3D>("Prefabs/Panel3D"));
			label.Text = Name;
			label.TextColor = LabelTextColor;
			label.Transform = LabelPosition ? LabelPosition : transform;

			_3dUIChannelEvent?.RaiseEvent(label, false);
		}

		if (_channel != null)
			_channel.OnEventRaised += Respond;
	}

    private void OnDisable()
    {
		if (_3dUIChannelEvent && label)
		{
			_3dUIChannelEvent.RaiseEvent(label, true);
		}
	}

    private void OnDestroy()
	{
		if (_3dUIChannelEvent && label)
		{
			_3dUIChannelEvent.RaiseEvent(label, true);
		}
	}

	private void Respond(NPCMovementConfigSO value)
	{
		_npcMovementConfig = value;
	}
}