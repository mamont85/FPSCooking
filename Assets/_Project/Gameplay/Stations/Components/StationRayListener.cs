using FPSCookingPrototype.Gameplay.Interfaces;
using FPSCookingPrototype.Gameplay.Items;
using FPSCookingPrototype.Gameplay.Player;

using UnityEngine;

namespace FPSCookingPrototype.Gameplay.Stations
{
	public class StationRayListener:MonoBehaviour, IInteractable, IItemReceiver
	{
		[SerializeField] private StationBehaviour _root;

		private Renderer _renderer;
		private Material _material;

		void Awake()
		{
			_renderer = GetComponent<Renderer>();
			_material = _renderer.material;
		}

		public bool IsActive => _root.IsActive;

		public InteractionType Interaction => _root.Interaction;

		public void OnHoverEnter()
		{
			_material.EnableKeyword("_EMISSION");
			_material.SetColor("_EmissionColor" , Color.yellow * 0.15f);
		}

		public void OnHoverExit()
		{
			_material.SetColor("_EmissionColor" , Color.black);
		}

		public ItemReceiverResponse TryPlaceItem( ItemType? itemType )
		{
			return _root.TryPlaceItem(itemType);
		}
	}
}
