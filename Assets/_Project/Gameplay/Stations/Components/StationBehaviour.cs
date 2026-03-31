using System;
using System.Collections;

using FPSCookingPrototype.Gameplay.Items;
using FPSCookingPrototype.Gameplay.Player;

using UnityEngine;

namespace FPSCookingPrototype.Gameplay.Stations
{
	public enum StationState
	{
		WaitingForStart,
		WaitingForInput,
		Cooking,
		Ready,
		Warning,
		Failed
	}
	public enum ItemReceiverResponse
	{
		Accepted,
		WrongItem
	}

	public class StationBehaviour:MonoBehaviour
	{
		[Header("Config")]
		[SerializeField] private StationData _data;

		[Header("Visual")]
		[SerializeField] private GameObject _cookingObject;
		[SerializeField] private Transform _itemVisualPoint;

		[Header("GUI")]
		[SerializeField] private StationGuiFeedback _guiFeedback;

		[Header("Temp for Test")]
		[SerializeField] private Transform _tempItemPoint; // Temporary spawn for prototype testing


		private StationState _currentState = StationState.WaitingForStart;
		private float _timer;

		public event Action OnStartCooking;
		public event Action<string , float> OnContinueCooking;
		public event Action<string> OnReady;
		public event Action<string> OnStartWarning;
		public event Action<string> OnFailed;
		public event Action OnReset;

		private WorldItemView _currentVResult;

		private IEnumerator Start()
		{
			_data.Validate();
			_guiFeedback.Init(_data.SelfName , ItemDatabaseService.Instance.Get(_data.InputItem).DisplayName , this);
			yield return null;
			StartWaitForInput();
		}


		private void Update()
		{
			if( _currentState == StationState.Failed || _currentState == StationState.WaitingForStart )
				return;

			//_timer += Time.deltaTime;
			//UpdateProgress();
			_timer += Time.deltaTime;
			if( _currentState == StationState.Cooking )
			{

				OnContinueCooking?.Invoke(_data.SelfName , _timer / _data.CookTime);
			}

			if( _timer >= _data.FailTime && _currentState == StationState.Warning )
			{
				EnterFailed();
				return;
			}

			if( _timer >= _data.WarningTime && _currentState == StationState.Ready )
			{
				EnterWarning();
				return;
			}

			if( _timer >= _data.CookTime && _currentState == StationState.Cooking )
			{
				EnterReady();
			}
		}

		// ========= INTERACTION =========
		public bool IsActive => true;
		public InteractionType Interaction => _currentState == StationState.WaitingForInput ? InteractionType.Place : InteractionType.None;

		public ItemReceiverResponse TryPlaceItem( ItemType? itemType )
		{
			if( itemType != _data.InputItem )
				return ItemReceiverResponse.WrongItem;
			StartCooking();
			return ItemReceiverResponse.Accepted;

		}




		private void StartWaitForInput()
		{
			_currentState = StationState.WaitingForInput;
			_cookingObject.SetActive(false);

			var itemData = ItemDatabaseService.Instance.Get(_data.InputItem);
			var item = GameObject.Instantiate(itemData.WorldViewPrefab);
			item.transform.SetPositionAndRotation(_tempItemPoint.position , Quaternion.identity);

			OnReset?.Invoke();

			Debug.Log("Reset - WaitForInpu");
		}

		private void StartCooking()
		{
			_currentState = StationState.Cooking;
			_timer = 0f;
			_cookingObject.SetActive(true);

			OnStartCooking?.Invoke();

			Debug.Log("Cooking started");
		}

		private void EnterReady()
		{
			_currentState = StationState.Ready;
			_cookingObject.SetActive(false);
			StartCoroutine(SpawnResult(_data.SuccessResult));

			OnReady?.Invoke(_data.SelfName);

			Debug.Log("Ready!");
		}

		private void EnterWarning()
		{
			_currentState = StationState.Warning;

			OnStartWarning?.Invoke(_data.SelfName);

			Debug.Log("Warning!");
		}

		private void EnterFailed()
		{
			_currentState = StationState.Failed;

			StartCoroutine(SpawnResult(_data.FailResult));

			OnFailed?.Invoke(_data.SelfName);

			Debug.Log("Failed!");
		}


		// ========= Result =========

		private IEnumerator SpawnResult( ItemType item )
		{
			if( _currentVResult != null )
			{
				//Destroy(_currentVResult.gameObject);
				_currentVResult.OnPickedUp -= OnPickedUpCurrentVResult;
				_currentVResult.Pickup();
				yield return new WaitForSeconds(0.1f);
			}

			var itemData = ItemDatabaseService.Instance.Get(item);

			_currentVResult = Instantiate(itemData.WorldViewPrefab);
			_currentVResult.transform.SetPositionAndRotation(_itemVisualPoint.position , Quaternion.identity);

			_currentVResult.OnPickedUp += OnPickedUpCurrentVResult;
		}

		private void OnPickedUpCurrentVResult()
		{
			_currentVResult.OnPickedUp -= OnPickedUpCurrentVResult;
			_currentVResult = null;
			_currentState = StationState.WaitingForInput;
			_timer = 0f;

			StartWaitForInput();
		}



	}
}
