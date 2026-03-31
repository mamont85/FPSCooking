using FPSCookingPrototype.Gameplay.Items;

using UnityEngine;

namespace FPSCookingPrototype.Gameplay.Player
{

	public class PlayerCarry:MonoBehaviour
	{
		[Header("Refs")]
		[SerializeField] private Transform _holdPoint;

		private HeldItemView _currentView;
		private ItemType? _currentItem;

		// ========= PUBLIC =========

		public bool HasItem => _currentItem.HasValue;
		public ItemType? CurrentItem => _currentItem;

		// ========= PICKUP =========

		public void Pickup( ItemType type )
		{
			if( HasItem )
			{
				SpawnWorldItem(_currentItem.Value);
				Clear();
			}

			_currentItem = type;
			SpawnHeldView(type);
		}

		// ========= USE =========

		public void Use()
		{
			if( !HasItem )
				return;

			Debug.Log($"Use {_currentItem}");
			// дальше сюда можно подключать станции/логику
		}

		// ========= DROP =========

		public void Drop()
		{
			if( !HasItem )
				return;

			SpawnWorldItem(_currentItem.Value);
			Clear();
		}

		public void DropInstant()
		{
			if( !HasItem )
				return;

			Clear();
		}

		// ========= INTERNAL =========

		private void SpawnHeldView( ItemType type )
		{
			var data = ItemDatabaseService.Instance.Get(type);

			if( data == null || data.HeldViewPrefab == null )
			{
				Debug.LogWarning($"No HeldViewPrefab for {type}");
				return;
			}

			_currentView = Instantiate(data.HeldViewPrefab , _holdPoint);
			_currentView.Initialize();
		}

		private void SpawnWorldItem( ItemType type )
		{
			var data = ItemDatabaseService.Instance.Get(type);

			if( data == null || data.WorldViewPrefab == null )
			{
				Debug.LogError($"No WorldViewPrefab for {type}");
				return;
			}

			Vector3 spawnPos = _holdPoint.position /*+ transform.forward*/;

			Instantiate(data.WorldViewPrefab , spawnPos , Quaternion.identity);
		}

		private void Clear()
		{
			if( _currentView != null )
			{
				Destroy(_currentView.gameObject);
				_currentView = null;
			}
			_currentItem = null;
		}
	}
}

