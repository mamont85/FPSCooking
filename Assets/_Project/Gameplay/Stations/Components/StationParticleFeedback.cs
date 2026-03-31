using UnityEngine;

namespace FPSCookingPrototype.Gameplay.Stations
{
	public class StationParticleFeedback:MonoBehaviour
	{
		[SerializeField] private StationBehaviour _station;

		[Header("Effects")]
		[SerializeField] private ParticleSystem _cookingEffect;
		[SerializeField] private ParticleSystem _failedEffect;

		private void OnEnable()
		{
			_station.OnStartCooking += HandleStartCooking;
			_station.OnFailed += HandleFailed;
			_station.OnReset += HandleReset;
		}

		private void OnDisable()
		{
			_station.OnStartCooking -= HandleStartCooking;
			_station.OnFailed -= HandleFailed;
			_station.OnReset -= HandleReset;
		}

		private void HandleStartCooking()
		{
			if( _cookingEffect != null )
				_cookingEffect.Play();
		}

		private void HandleFailed( string _ )
		{
			if( _failedEffect != null )
				_failedEffect.Play();

			if( _cookingEffect != null )
				_cookingEffect.Stop(true , ParticleSystemStopBehavior.StopEmittingAndClear);
		}

		private void HandleReset()
		{
			if( _cookingEffect != null )
				_cookingEffect.Stop(true , ParticleSystemStopBehavior.StopEmittingAndClear);

			if( _failedEffect != null )
				_failedEffect.Stop(true , ParticleSystemStopBehavior.StopEmittingAndClear);
		}
	}
}
