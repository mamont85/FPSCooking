using FPSCookingPrototype.Gameplay.Interfaces;
using FPSCookingPrototype.Gameplay.Stations;
using FPSCookingPrototype.UI.Controls;

using UnityEngine;

namespace FPSCookingPrototype.Gameplay.Player
{
	public enum InteractionType
	{
		None,
		Take,
		Place
	}
	[RequireComponent(typeof(PlayerCarry))]
	public class PlayerInteraction:MonoBehaviour
	{
		[Header("Refs")]
		[SerializeField] private PlayerGUI _gui;
		[SerializeField] private Camera _playerCamera;
		[SerializeField] private float _interactDistance = 3f;
		[SerializeField] private LayerMask _interactMask;

		private PlayerCarry _carry;

		private IInteractable _currentHover;

		private InteractionType _currentInteraction = InteractionType.None;

		private void Awake()
		{
			_carry = GetComponent<PlayerCarry>();
		}

		private void Start()
		{
			_gui.Init(OnInteractPressed , OnDropItem , OnInteractPressed);
		}

		private void Update()
		{
			if( Input.GetKeyDown(KeyCode.E) )
			{
				OnInteractPressed();
			}
			if( Input.GetKeyDown(KeyCode.R) )
			{
				OnDropItem();
			}
			UpdateHover();

			Debug.DrawRay(_playerCamera.transform.position ,
			  _playerCamera.transform.forward * _interactDistance ,
			  Color.green);
		}

		private void SetInteractionType( InteractionType type )
		{
			//if( _currentInteraction == type )
			//	return;
			Debug.Log($"SetInteractionType {type}");
			_currentInteraction = type;
			switch( _currentInteraction )
			{
				case InteractionType.None:
				{
					_gui.SetPlaceState(false);
					_gui.SetTakeState(false);
				}
				break;
				case InteractionType.Take:
				{
					_gui.SetPlaceState(false);
					_gui.SetTakeState(true);
				}
				break;
				case InteractionType.Place:
				{
					if( _carry.HasItem )
					{
						_gui.SetPlaceState(true);
						_gui.SetTakeState(false);
					}
					else
					{
						_gui.SetPlaceState(false);
						_gui.SetTakeState(false);
					}
				}
				break;
			}
		}



		// ========= INPUT =========

		private void OnInteractPressed()
		{
			if( _currentHover == null )
				return;
			if( _currentHover is IPickupable item )
			{
				_carry.Pickup(item.GetItemType());
				item.Pickup();
				_gui.SetTakeState(false);
				_gui.SetDropState(true);
				_currentHover = null;
				return;
			}
			if( _currentHover is IItemReceiver receiver )
			{
				switch( receiver.TryPlaceItem(_carry.CurrentItem) )
				{
					case ItemReceiverResponse.Accepted:
					{
						_carry.DropInstant();
						_gui.SetDropState(false);
						_gui.SetPlaceState(false);
						_gui.SetTakeState(false);
					}
					break;
					case ItemReceiverResponse.WrongItem:
					{
						_gui.ShowMsg("Не тот предмет");
					}
					break;
				}
			}
		}

		private void OnDropItem()
		{
			if( _carry.HasItem == false )
				return;
			_carry.Drop();
			_gui.SetDropState(false);
		}

		// ========= HOVER =========

		private void UpdateHover()
		{
			IInteractable newHover = null;

			if( Physics.Raycast(_playerCamera.transform.position ,
								_playerCamera.transform.forward ,
								out RaycastHit hit ,
								_interactDistance ,
								_interactMask) )
			{
				newHover = hit.collider.GetComponent<IInteractable>();
			}
			if( newHover != null && newHover.IsActive == false )
			{
				newHover = null;
			}

			// если объект сменился
			if( newHover != _currentHover )
			{
				if( _currentHover != null )
				{
					_currentHover.OnHoverExit();
					SetInteractionType(InteractionType.None);
				}

				_currentHover = newHover;
				if( _currentHover != null )
				{
					_currentHover.OnHoverEnter();
					SetInteractionType(_currentHover.Interaction);
				}
			}
		}
	}
}


