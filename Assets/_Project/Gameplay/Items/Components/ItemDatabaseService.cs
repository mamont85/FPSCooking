using UnityEngine;


namespace FPSCookingPrototype.Gameplay.Items
{
	public class ItemDatabaseService:MonoBehaviour
	{
		[SerializeField] private ItemDatabase _itemsConfig;

		public static ItemDatabaseService Instance;

		private void Awake()
		{
			Instance = this;
		}

		public ItemData Get( ItemType type ) => _itemsConfig.Get(type);


	}
}
