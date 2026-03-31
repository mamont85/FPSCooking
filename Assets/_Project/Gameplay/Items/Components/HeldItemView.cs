using NUnit.Framework.Internal.Execution;

using UnityEngine;


namespace FPSCookingPrototype.Gameplay.Items
{
	public class HeldItemView:MonoBehaviour
	{

		public void Initialize()
		{
			transform.SetLocalPositionAndRotation(Vector3.zero , Quaternion.identity);
			//transform.localScale = Vector3.one * 0.5f;
		}
	}
}
