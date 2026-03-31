using FPSCookingPrototype.Gameplay.Items;

namespace FPSCookingPrototype.Gameplay.Interfaces
{
	public interface IPickupable
	{

		ItemType GetItemType();
		void Pickup();
	}
}
