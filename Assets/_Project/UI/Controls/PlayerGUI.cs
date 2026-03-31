using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace FPSCookingPrototype.UI.Controls
{
	public class PlayerGUI:MonoBehaviour
	{
		[SerializeField] private Button _takeButton;
		[SerializeField] private Button _dropButton;
		[SerializeField] private Button _placeButton;

		[SerializeField] private TextMeshProUGUI _msgText;
		[SerializeField] private float _msgDuration = 2f;

		private Action _onTakePressed;
		private Action _onDropPressed;
		private Action _onPlacePressed;

		private float _msgTimer;
		private bool _isShowingMsg;

		private void OnEnable()
		{
			_takeButton.onClick.AddListener(() => _onTakePressed?.Invoke());
			_dropButton.onClick.AddListener(() => _onDropPressed?.Invoke());
			_placeButton.onClick.AddListener(() => _onPlacePressed?.Invoke());
		}

		private void OnDisable()
		{
			_takeButton.onClick.RemoveAllListeners();
			_dropButton.onClick.RemoveAllListeners();
			_placeButton.onClick.RemoveAllListeners();
		}
		private void Start()
		{
			_msgText.text = "";
			_takeButton.gameObject.SetActive(false);
			_dropButton.gameObject.SetActive(false);
			_placeButton.gameObject.SetActive(false);
		}

		private void Update()
		{
			if( !_isShowingMsg )
				return;

			_msgTimer -= Time.deltaTime;

			if( _msgTimer <= 0f )
			{
				_msgText.text = "";
				_isShowingMsg = false;
			}
		}

		public void Init( Action onTakePressed , Action onDropPressed , Action onPlacePressed )
		{
			_onTakePressed = onTakePressed;
			_onDropPressed = onDropPressed;
			_onPlacePressed = onPlacePressed;
		}

		public void SetDropState( bool state )
		{
			_dropButton.gameObject.SetActive(state);
		}

		public void SetTakeState( bool state )
		{
			_takeButton.gameObject.SetActive(state);
		}

		public void SetPlaceState( bool state )
		{
			_placeButton.gameObject.SetActive(state);
		}

		public void ShowMsg( string msg )
		{
			_isShowingMsg = true;
			_msgText.text = msg;
			_msgTimer = _msgDuration;
		}

	}
}