using System.Collections.Generic;

using FPSCookingPrototype.Gameplay.Items;

using UnityEngine;


namespace FPSCookingPrototype.Gameplay.Stations
{
	[CreateAssetMenu(
		fileName = "StationData" ,
		menuName = "FPSCookingPrototype/Stations/StationData"
	)]
	public class StationData:ScriptableObject
	{
		[Header("Input")]
		public ItemType InputItem;

		[Header("Timing")]
		public float CookTime = 15f;
		public float WarningTime = 20f;
		public float FailTime = 30f;

		[Header("Output")]
		public ItemType SuccessResult;
		public ItemType FailResult;

		[Header("Station name")]
		public string SelfName;

		public void Validate()
		{
			if( CookTime < 0 )
				CookTime = 0;

			if( WarningTime <= CookTime )
				WarningTime = CookTime +2.0f;

			if( FailTime <= WarningTime )
				FailTime = WarningTime +2.0f;
		}
	}
}

