#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using FPSCookingPrototype.Gameplay.Items;

[CustomEditor(typeof(ItemDatabase))]
public class ItemDatabaseEditor:Editor
{
	private List<string> errors = new();

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		EditorGUILayout.Space(10);
		EditorGUILayout.LabelField("Validation" , EditorStyles.boldLabel);

		if( GUILayout.Button("Validate") )
		{
			Validate();
		}

		if( GUILayout.Button("Auto Populate") )
		{
			var db = (ItemDatabase)target;
			db.AutoPopulate();

			Validate();
		}

		if( errors.Count > 0 )
		{
			EditorGUILayout.Space(5);

			foreach( var error in errors )
			{
				EditorGUILayout.HelpBox(error , MessageType.Error);
			}
		}
		else
		{
			EditorGUILayout.HelpBox("No validation errors" , MessageType.Info);
		}
	}

	private void Validate()
	{
		errors.Clear();

		var db = (ItemDatabase)target;

		var field = typeof(ItemDatabase)
			.GetField("items" , System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

		var items = field.GetValue(db) as List<ItemData>;

		if( items == null || items.Count == 0 )
		{
			errors.Add("Item list is empty");
			return;
		}

		// null элементы
		if( items.Any(i => i == null) )
		{
			errors.Add("Item list contains null elements");
		}

		// дубликаты
		var duplicates = items
			.GroupBy(i => i.ItemType)
			.Where(g => g.Count() > 1);

		foreach( var group in duplicates )
		{
			errors.Add($"Duplicate ItemType: {group.Key}");
		}

		// отсутствие типов
		var missing = System.Enum.GetValues(typeof(ItemType))
			.Cast<ItemType>()
			.Where(t => items.All(i => i.ItemType != t));

		foreach( var type in missing )
		{
			errors.Add($"Missing ItemData for: {type}");
		}

		// проверка полей
		foreach( var item in items )
		{
			if( item == null )
				continue;

			if( string.IsNullOrWhiteSpace(item.DisplayName) )
			{
				errors.Add($"Item '{item.ItemType}' has empty DisplayName");
			}

			if( item.HeldViewPrefab == null )
			{
				errors.Add($"Item '{item.ItemType}' has no HeldViewPrefab");
			}
			if( item.WorldViewPrefab == null )
			{
				errors.Add($"Item '{item.ItemType}' has no WorldViewPrefab");
			}
		}
	}
}
#endif