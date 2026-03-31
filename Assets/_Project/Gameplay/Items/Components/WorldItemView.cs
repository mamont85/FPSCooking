using System;
using System.Collections;

using FPSCookingPrototype.Gameplay.Interfaces;
using FPSCookingPrototype.Gameplay.Player;

using TMPro;

using UnityEngine;


namespace FPSCookingPrototype.Gameplay.Items
{

	[RequireComponent(typeof(BoxCollider))]
	public class WorldItemView:MonoBehaviour, IInteractable, IPickupable
	{
		[SerializeField] private ItemType _itemType;

		[Header("Hover")]
		[SerializeField] private GameObject _hoverRoot;
		[SerializeField] private TextMeshProUGUI _hoverText;

		private BoxCollider _collider;

		private ItemData Data => ItemDatabaseService.Instance.Get(_itemType);

		private Renderer _renderer;
		private Material _material;


		private void Awake()
		{
			_collider = GetComponent<BoxCollider>();
			_renderer = GetComponent<Renderer>();
			_material = _renderer.material;
			_collider.enabled = false;
		}

		public bool IsActive
		{
			get; private set;
		} = true;

		public event Action OnPickedUp;

		private IEnumerator Start()
		{
			_hoverRoot.SetActive(false);

			yield return null;
			_collider.enabled = true;
		}


		public ItemType GetItemType() => _itemType;
		public void Pickup()
		{
			OnPickedUp?.Invoke();
			Destroy(gameObject);
			IsActive = false;
			_collider.enabled = false;
		}


		public InteractionType Interaction => InteractionType.Take;
		public void OnHoverEnter()
		{
			if( IsActive == false )
				return;
			_hoverRoot.SetActive(true);

			if( _hoverText != null )
			{
				string name = Data != null ? Data.DisplayName : _itemType.ToString();

				_hoverText.text = $"Взять {name}";
			}

			_material.EnableKeyword("_EMISSION");
			_material.SetColor("_EmissionColor" , Color.yellow * 0.15f);
		}

		public void OnHoverExit()
		{
			if( IsActive == false )
				return;
			if( _hoverRoot != null )
				_hoverRoot.SetActive(false);

			if( _material != null )
				_material.SetColor("_EmissionColor" , Color.black);
		}
	}
}
