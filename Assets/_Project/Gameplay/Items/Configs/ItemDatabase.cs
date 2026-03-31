using System;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;

using UnityEngine;
																		  
namespace FPSCookingPrototype.Gameplay.Items
{
	[CreateAssetMenu(
		fileName = "ItemDatabase" ,
		menuName = "FPSCookingPrototype/Items/Item Database"
	)]
	public class ItemDatabase:ScriptableObject
	{
		[SerializeField]
		private List<ItemData> _items = new();

		private Dictionary<ItemType , ItemData> _lookup;

		public ItemData Get( ItemType type )
		{
			if( _lookup == null )
				BuildLookup();

			if( _lookup.TryGetValue(type , out var data) )
				return data;

			Debug.LogError($"ItemData not found for type: {type}");
			return null;
		}

		private void BuildLookup()
		{
			_lookup = _items
				.Where(i => i != null)
				.GroupBy(i => i.ItemType)
				.ToDictionary(g => g.Key , g => g.First());
		}

#if UNITY_EDITOR
		public void AutoPopulate()
		{
			if( _items == null )
				_items = new List<ItemData>();

			var existingTypes = _items
				.Where(i => i != null)
				.Select(i => i.ItemType)
				.ToHashSet();

			var allTypes = Enum.GetValues(typeof(ItemType)).Cast<ItemType>();

			foreach( var type in allTypes )
			{
				if( existingTypes.Contains(type) )
					continue;

				_items.Add(new ItemData
				{
					ItemType = type ,
					DisplayName = type.ToString()
				});
			}

			EditorUtility.SetDirty(this);
			AssetDatabase.SaveAssets();
		}

#endif
	}
}