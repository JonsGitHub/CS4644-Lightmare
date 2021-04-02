using UnityEngine;

public class StoryColumn : InterfaceBase
{
    [Header("Broadcasting on channels")]
    [SerializeField] private UI3DEventChannelSO _3dUIChannelEvent = default;

	private Panel3D label = null;
	private Panel3D numberLabel = null;

    [Header("Text Label Properties")]
    [SerializeField] private Transform LabelPosition = default;
    [SerializeField] private Color LabelTextColor = Color.black;

    [Header("Number Label Properties")]
    [SerializeField] private Transform NumberLabelPosition = default;
    [SerializeField] private Color NumberTextColor = Color.black;

    public override void Interact()
    {
        if (numberLabel.Text.Equals(""))
            GetComponentInParent<StoryPuzzle>().SelectColumn(this, label.Text);
    }

    public void SetStoryText(string text)
    {
        if (!label)
        {
            label = Instantiate(Resources.Load<Panel3D>("Prefabs/Panel3D"));
            label.Transform = LabelPosition ? LabelPosition : transform;
            label.TextColor = LabelTextColor;

            _3dUIChannelEvent?.RaiseEvent(label, false);
        }
        label.Text = text;
    }
    public void SetNumber(int number)
    {
        if (!numberLabel)
        {
            numberLabel = Instantiate(Resources.Load<Panel3D>("Prefabs/Panel3D"));
            numberLabel.Transform = NumberLabelPosition ? NumberLabelPosition : transform;
            numberLabel.Text = "";
            numberLabel.TextColor = NumberTextColor;

            _3dUIChannelEvent?.RaiseEvent(numberLabel, false);
        }
        numberLabel.Text = number == -1 ? "" : "" + number;
    }

    private void OnDestroy()
    {
        if (_3dUIChannelEvent && label)
        {
            _3dUIChannelEvent.RaiseEvent(label, true);
        }
        if (_3dUIChannelEvent && numberLabel)
        {
            _3dUIChannelEvent.RaiseEvent(numberLabel, true);
        }
    }
}
