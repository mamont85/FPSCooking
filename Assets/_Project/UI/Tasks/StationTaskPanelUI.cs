using System;
using System.Collections.Generic;

using FPSCookingPrototype.Gameplay.Stations;

using UnityEngine;

namespace FPSCookingPrototype.UI.Tasks
{
	public class StationTaskPanelUI:MonoBehaviour
	{
		[Header("Refs")]
		[SerializeField] private List<StationBehaviour> _stations;
		[SerializeField] private StationTaskItemUI _itemPrefab;
		[SerializeField] private Transform _container;

		private Dictionary<StationBehaviour , StationTaskItemUI> _items = new();

		private Dictionary<StationBehaviour , Action> _startCookingHandlers = new();
		private Dictionary<StationBehaviour , Action<string , float>> _continueHandlers = new();
		private Dictionary<StationBehaviour , Action<string>> _readyHandlers = new();
		private Dictionary<StationBehaviour , Action<string>> _warningHandlers = new();
		private Dictionary<StationBehaviour , Action<string>> _failedHandlers = new();
		private Dictionary<StationBehaviour , Action> _resetHandlers = new();

		private void Awake()
		{
			foreach( var station in _stations )
			{
				var item = Instantiate(_itemPrefab , _container);
				item.gameObject.SetActive(false);

				_items.Add(station , item);
				CreateHandlers(station , item);
			}
		}

		private void OnEnable()
		{
			foreach( var station in _stations )
			{
				station.OnStartCooking += _startCookingHandlers[station];
				station.OnContinueCooking += _continueHandlers[station];
				station.OnReady += _readyHandlers[station];
				station.OnStartWarning += _warningHandlers[station];
				station.OnFailed += _failedHandlers[station];
				station.OnReset += _resetHandlers[station];
			}
		}

		private void OnDisable()
		{
			foreach( var station in _stations )
			{
				station.OnStartCooking -= _startCookingHandlers[station];
				station.OnContinueCooking -= _continueHandlers[station];
				station.OnReady -= _readyHandlers[station];
				station.OnStartWarning -= _warningHandlers[station];
				station.OnFailed -= _failedHandlers[station];
				station.OnReset -= _resetHandlers[station];
			}
		}

		private void CreateHandlers( StationBehaviour station , StationTaskItemUI item )
		{
			_startCookingHandlers[station] = () =>
			{
				item.gameObject.SetActive(true);
				item.SetColor(Color.white);
			};

			_continueHandlers[station] = ( name , progress ) =>
			{
				item.SetText($"{name} — {( progress * 100f ):0}%");
			};

			_readyHandlers[station] = ( name ) =>
			{
				item.SetText($"{name} — готово");
				item.SetColor(Color.green);
			};

			_warningHandlers[station] = ( name ) =>
			{
				item.SetText($"{name} — Сгорит! Забери!");
				item.SetColor(Color.yellow);
			};

			_failedHandlers[station] = ( name ) =>
			{
				item.SetText($"{name} — СГОРЕЛО");
				item.SetColor(Color.red);
			};

			_resetHandlers[station] = () =>
			{
				item.gameObject.SetActive(false);
			};
		}
	}

}
