using UnityEngine;


namespace FPSCookingPrototype.Gameplay.Items
{
	public class HeldItemFollower:MonoBehaviour
	{
		[SerializeField] private Transform target;
		[SerializeField] private float positionSmooth = 10f;
		[SerializeField] private float rotationSmooth = 10f;

		private void LateUpdate()
		{
			if( target == null )
				return;

			transform.position = Vector3.Lerp(
				transform.position ,
				target.position ,
				Time.deltaTime * positionSmooth
			);

			transform.rotation = Quaternion.Slerp(
				transform.rotation ,
				target.rotation ,
				Time.deltaTime * rotationSmooth
			);
		}
	}
}
