using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryColumn : InterfaceBase
{
    [Header("Broadcasting on channels")]
    [SerializeField] private UI3DEventChannelSO _3dUIChannelEvent = default;

	private Panel3D label;
	private Panel3D numberLabel;

    [Header("Text Label Properties")]
    [SerializeField] private Transform LabelPosition = default;
    [SerializeField] private Color LabelTextColor = Color.black;

    [Header("Number Label Properties")]
    [SerializeField] private Transform NumberLabelPosition = default;
    [SerializeField] private Color NumberTextColor = Color.black;

    private void Awake()
	{
		label = Instantiate(Resources.Load<Panel3D>("Prefabs/Panel3D"));
		label.Transform = LabelPosition ? LabelPosition : transform;
		label.TextColor = LabelTextColor;

		_3dUIChannelEvent?.RaiseEvent(label, false);

        numberLabel = Instantiate(Resources.Load<Panel3D>("Prefabs/Panel3D"));
        numberLabel.Transform = NumberLabelPosition ? NumberLabelPosition : transform;
        numberLabel.Text = "";
        numberLabel.TextColor = NumberTextColor;

        _3dUIChannelEvent?.RaiseEvent(numberLabel, false);
    }

    public override void Interact()
    {
        if (numberLabel.Text.Equals(""))
            GetComponentInParent<StoryPuzzle>().SelectColumn(this, label.Text);
    }

    public void SetStoryText(string text) => label.Text = text;

    public void SetNumber(int number) => numberLabel.Text = number == -1 ? "" : ""+number;
}
