using UnityEngine;

namespace FPSCookingPrototype.Gameplay.Stations
{
	[RequireComponent(typeof(AudioSource))]
	public class StationAudioFeedback:MonoBehaviour
	{
		[SerializeField] private StationBehaviour _station;

		[Header("Audio Clips")]
		[SerializeField] private AudioClip _cookingLoop;
		[SerializeField] private AudioClip _readyDing;
		[SerializeField] private AudioClip _warning;
		[SerializeField] private AudioClip _failed;

		[Header("Variation")]
		[SerializeField] private Vector2 _pitchRange = new Vector2(0.95f , 1.05f);
		[SerializeField] private Vector2 _volumeRange = new Vector2(0.95f , 1f);

		private AudioSource _audio;

		private void Awake()
		{
			_audio = GetComponent<AudioSource>();

			if( _station == null )
				_station = GetComponent<StationBehaviour>();
		}

		private void OnEnable()
		{
			_station.OnStartCooking += HandleStartCooking;
			_station.OnReady += HandleReady;
			_station.OnStartWarning += HandleWarning;
			_station.OnFailed += HandleFailed;
			_station.OnReset += HandleReset;
		}

		private void OnDisable()
		{
			_station.OnStartCooking -= HandleStartCooking;
			_station.OnReady -= HandleReady;
			_station.OnStartWarning -= HandleWarning;
			_station.OnFailed -= HandleFailed;
			_station.OnReset -= HandleReset;
		}

		private void HandleStartCooking()
		{
			PlayLoop(_cookingLoop);
		}

		private void HandleReady( string _ )
		{
		//	StopLoop();
			PlayOneShot(_readyDing);
		}

		private void HandleWarning( string _ )
		{
			PlayOneShot(_warning);
		}

		private void HandleFailed( string _ )
		{
			StopLoop();
			PlayOneShot(_failed);
		}

		private void HandleReset()
		{
			StopLoop();
		}

		private void ApplyVariation()
		{
			_audio.pitch = Random.Range(_pitchRange.x , _pitchRange.y);
			_audio.volume = Random.Range(_volumeRange.x , _volumeRange.y);
		}

		private void PlayLoop( AudioClip clip )
		{
			if( clip == null )
				return;

			ApplyVariation();

			_audio.clip = clip;
			_audio.loop = true;
			_audio.Play();
		}

		private void StopLoop()
		{
			_audio.loop = false;
			_audio.Stop();
			_audio.clip = null;
		}

		private void PlayOneShot( AudioClip clip )
		{
			if( clip == null )
				return;

			ApplyVariation();
			_audio.PlayOneShot(clip);
		}
	}
}
