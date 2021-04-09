using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInteractionManager : MonoBehaviour
{
	[SerializeField] private InputReader _inputReader = default;

	[SerializeField] private List<InteractionSO> _listInteractions = default;

	[SerializeField] private RectTransform _content = default;
	[SerializeField] private ScrollRect _scrollrect = default;
	[SerializeField] private Image _scrollImage = default;

	private InteractionList _list;
	private UIInteractionItemFiller _prev;

	private int _scrollValue = 0;

	private void OnEnable()
	{
		_inputReader.scrollEvent += OnZoom;
	}

	private void OnDisable()
	{
		_inputReader.scrollEvent -= OnZoom;
	}

	public void UpdateList(bool state)
    {
		if (_list == null)
			return;

		foreach (Transform child in _content)
			Destroy(child.gameObject);
		
		if (state)
        {
			if (_list.IsGrabbing)
            {
				var item = Instantiate(Resources.Load<UIInteractionItemFiller>("Prefabs/InteractionItem"));
				item.FillInteractionPanel(_listInteractions.Find(x => x.InteractionType == InteractionType.Drop));
				item.SetSelected(true);
				item.transform.SetParent(_content.transform);
			}
			else
            {
				_list.SelectedIndex = Mathf.Clamp(_list.SelectedIndex, 0, _list.Count - 1);

				for (int i = 0; i < _list.Count; ++i)
				{
					var item = Instantiate(Resources.Load<UIInteractionItemFiller>("Prefabs/InteractionItem"));
					var type = _list[i].type;
					if (_list[i].type.Equals(InteractionType.Grab))
                    {
						item.FillInteractionPanel(_listInteractions.Find(x => x.InteractionType == type), _list[i].interactableObject.name);
                    }
					else
                    {
						item.FillInteractionPanel(_listInteractions.Find(x => x.InteractionType == type));
                    }
					if (i == _list.SelectedIndex)
					{
						_prev = item;
						item.SetSelected(true);
					}
					item.transform.SetParent(_content.transform);
				}

                // Set Scroll Image Position
				_scrollImage.gameObject.SetActive(_list.Count > 1);
                if (_list.Count == 2)
				{
					_scrollImage.rectTransform.localPosition = new Vector2(-140, 40);
                }
				else if (_list.Count == 3)
                {
					_scrollImage.rectTransform.localPosition = new Vector2(-140, 22);
                }
				else
                {
					_scrollImage.rectTransform.localPosition = new Vector2(-140, 4.2f);
                }
			}
		}
    }

	public void FillInteractionPanel(InteractionList list)
	{
		_list = list;
		_prev = null;

		if (_list == null)
			return;

		UpdateList(list.Count > 0);
	}

	private void OnZoom(float axis)
	{
		if (_list == null || _list.Count == 0 || _list.IsGrabbing)
			return;

		_scrollValue += (int)axis;
		if (_scrollValue != 0 && _scrollValue % 2 == 0)
        {
			if (_scrollValue < 0)
				_list.SelectedIndex++;
			else
				_list.SelectedIndex--;

			_scrollValue = 0;
			_list.SelectedIndex = Mathf.Clamp(_list.SelectedIndex, 0, _list.Count - 1);

			if (_prev)
				_prev.SetSelected(false);

			var child = _content.transform.GetChild(_list.SelectedIndex);
			_prev = child.GetComponent<UIInteractionItemFiller>();
			_prev.SetSelected(true);
			EnsureVisibility(child.GetComponent<RectTransform>(), 25);
		}
	}

	private void EnsureVisibility(RectTransform target, float padding = 0)
	{
		float viewportHeight = _scrollrect.viewport.rect.height;
		Vector2 scrollPosition = _scrollrect.content.anchoredPosition;

		float elementTop = target.anchoredPosition.y;
		float elementBottom = elementTop - target.rect.height;

		float visibleContentTop = -scrollPosition.y - padding;
		float visibleContentBottom = -scrollPosition.y - viewportHeight + padding;

		float scrollDelta = elementTop > visibleContentTop ? visibleContentTop - elementTop :
							elementBottom < visibleContentBottom ? visibleContentBottom - elementBottom : 0f;

		scrollPosition.y += scrollDelta;
		_scrollrect.content.anchoredPosition = scrollPosition;
	}
}