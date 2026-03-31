using UnityEngine;

namespace FPSCookingPrototype.UI
{
	using UnityEngine;

	public class BillboardUI:MonoBehaviour
	{
		private Camera _camera;

		private void Awake()
		{
			_camera = Camera.main;
		}

		private void LateUpdate()
		{
			if( _camera == null )
				return;

			transform.forward = _camera.transform.forward;
		}
	}
}

