using UnityEngine;

/// TODO: Remove this class and related dependencies and convert to new SO model.
public class NPCController : EntityController
{
    [Tooltip("The name of the NPC.")]
    [SerializeField] private string Name;

    [Header("Broadcasting on channels")]
    [SerializeField] private UI3DEventChannelSO _3dUIChannelEvent = default;

    private Panel3D label;
    
    [Tooltip("The location of the overhead label.")]
    [SerializeField] private Transform LabelPosition = default;

    private void Start()
    {
        label = Instantiate(Resources.Load<Panel3D>("Prefabs/Panel3D"));
        label.Text = Name;
        label.Transform = LabelPosition ? LabelPosition : transform;

        _3dUIChannelEvent?.RaiseEvent(label, false);
    }

    private void OnDestroy()
    {
        _3dUIChannelEvent?.RaiseEvent(label, true);
    }
}
