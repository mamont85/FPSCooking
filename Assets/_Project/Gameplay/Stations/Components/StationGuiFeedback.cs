using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace FPSCookingPrototype.Gameplay.Stations
{
	public class StationGuiFeedback:MonoBehaviour
	{
		[Header("Hover")]
		[SerializeField] private GameObject _progressBarCont;
		[SerializeField] private Image _progressBar; // WorldCanvas Slider/Imag
		[SerializeField] private TextMeshProUGUI _hoverText;

		[Header("Self Name")]
		[SerializeField] private TextMeshProUGUI _nameText;

		private string _selfName;
		private string _neededItemName;
		private StationBehaviour _station;

		private bool _isSubscribe = false;


		private void OnDestroy()
		{
			UnSubscribe();
		}
		private void Subscribe()
		{
			if( _isSubscribe )
				return;
			_isSubscribe = true;
			_station.OnStartCooking += HandleStartCooking;
			_station.OnContinueCooking += HandleContinueCooking;
			_station.OnReady += HandleReady;
			_station.OnStartWarning += HandleWarning;
			_station.OnFailed += HandleFailed;
			_station.OnReset += HandleReset;
		}

		private void UnSubscribe()
		{
			if( _isSubscribe == false )
				return;
			_isSubscribe = false;
			_station.OnStartCooking -= HandleStartCooking;
			_station.OnContinueCooking -= HandleContinueCooking;
			_station.OnReady -= HandleReady;
			_station.OnStartWarning -= HandleWarning;
			_station.OnFailed -= HandleFailed;
			_station.OnReset -= HandleReset;
		}

		public void Init(string selfName, string neededItemName, StationBehaviour station )
		{
			_selfName = selfName;
			_neededItemName = neededItemName;
			_nameText.text = _selfName;

			_station = station;
			Subscribe();
		}


		private void HandleStartCooking()
		{
			_progressBarCont.SetActive(true);
			_progressBar.color = Color.white;
			_hoverText.text = "Готовится...";
			_hoverText.color = Color.white;
		}
		private void HandleContinueCooking( string name , float progress )
		{
			_progressBar.fillAmount = Mathf.Clamp01(progress);
		}

		private void HandleReady( string _ )
		{
			_progressBar.color = Color.green;
			_hoverText.text = "Можно Забрать";
			_hoverText.color = Color.green;
		}

		private void HandleWarning( string _ )
		{
			_progressBar.color = Color.yellow;
			_hoverText.text = "Сгорит! Быстрее забери!";
			_hoverText.color = Color.yellow;
		}

		private void HandleFailed( string _ )
		{
			_progressBar.color = Color.red;
			_hoverText.text = "Забирай что наготовил((";
			_hoverText.color = Color.red;
		}

		private void HandleReset()
		{
			_progressBarCont.SetActive(false);
			_hoverText.text = $"Принеси {_neededItemName}";
			_hoverText.color = Color.white;
		}
	}
}
