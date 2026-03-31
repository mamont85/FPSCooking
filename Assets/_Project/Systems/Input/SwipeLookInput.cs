using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeLookInput:MonoBehaviour
{
	[SerializeField] private float sensitivity = 0.15f;

	private readonly List<RaycastResult> _uiHits = new();

	private int _lookFingerId = -1;
	private Vector2 _lastPos;

	public Vector2 LookDelta
	{
		get; private set;
	}

	private void Update()
	{
		LookDelta = Vector2.zero;

		if( Input.touchCount == 0 )
			return;

		for( int i = 0; i < Input.touchCount; i++ )
		{
			Touch touch = Input.GetTouch(i);

			// если уже захватили палец для камеры
			if( _lookFingerId != -1 && touch.fingerId != _lookFingerId )
				continue;


			// если палец над UI — игнорируем
			if( IsTouchOverUI(touch) )
				continue;

			switch( touch.phase )
			{
				case TouchPhase.Began:
				_lookFingerId = touch.fingerId;
				_lastPos = touch.position;
				break;

				case TouchPhase.Moved:
				case TouchPhase.Stationary:
				if( touch.fingerId == _lookFingerId )
				{
					Vector2 delta = touch.position - _lastPos;
					LookDelta = delta * sensitivity * Time.deltaTime;
					_lastPos = touch.position;
				}
				break;

				case TouchPhase.Ended:
				case TouchPhase.Canceled:
				if( touch.fingerId == _lookFingerId )
				{
					_lookFingerId = -1;
				}
				break;
			}

			if( _lookFingerId == touch.fingerId )
				break;
		}
	}

	private bool IsTouchOverUI( Touch touch )
	{
		if( EventSystem.current == null )
			return false;
		_uiHits.Clear();
		PointerEventData eventData = new PointerEventData(EventSystem.current);
		eventData.position = touch.position;

		EventSystem.current.RaycastAll(eventData , _uiHits);

		return _uiHits.Count > 0;
	}
}
