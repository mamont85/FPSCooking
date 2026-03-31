using FPSCookingPrototype.Gameplay.Items;
using FPSCookingPrototype.Gameplay.Stations;

namespace FPSCookingPrototype.Gameplay.Interfaces
{
	public interface IItemReceiver
	{
		ItemReceiverResponse TryPlaceItem( ItemType? itemType );
	}
}
