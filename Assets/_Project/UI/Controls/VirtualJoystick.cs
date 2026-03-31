using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField] private RectTransform _background;
	[SerializeField] private RectTransform _handle;

	private Vector2 _input;

	public Vector2 Input => _input;

	public void OnPointerDown(PointerEventData eventData)
	{
		Vector2 pos;

		RectTransformUtility.ScreenPointToLocalPointInRectangle(
		this.GetComponent<RectTransform>() ,
		eventData.position ,
		null ,
		out pos
		);

		_background.anchoredPosition = pos;

		_handle.anchoredPosition = Vector2.zero;

		OnDrag(eventData);
	}

	public void OnDrag(PointerEventData eventData)
	{
		Vector2 pos;

		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			_background,
			eventData.position,
			eventData.pressEventCamera,
			out pos
		);

		pos.x /= _background.sizeDelta.x;
		pos.y /= _background.sizeDelta.y;

		_input = new Vector2(pos.x * 2, pos.y * 2);
		_input = Vector2.ClampMagnitude(_input, 1f);

		_handle.anchoredPosition = new Vector2(
			_input.x * (_background.sizeDelta.x / 2),
			_input.y * (_background.sizeDelta.y / 2)
		);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		_input = Vector2.zero;
		_handle.anchoredPosition = Vector2.zero;
	}
}