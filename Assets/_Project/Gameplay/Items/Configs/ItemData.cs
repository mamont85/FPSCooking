using System;

using UnityEngine;

namespace FPSCookingPrototype.Gameplay.Items
{
	[Serializable]
	public class ItemData
	{
		[Header("Core")]
		public ItemType ItemType;

		[Header("Presentation")]
		public string DisplayName;
		public HeldItemView HeldViewPrefab;
		public WorldItemView WorldViewPrefab;
	}
}