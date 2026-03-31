using UnityEngine;

namespace FPSCookingPrototype.Gameplay.Player
{

	[RequireComponent(typeof(CharacterController))]
	public class PlayerController:MonoBehaviour
	{
		[Header("Refs")]
		[SerializeField] private Transform cameraPivot;

		[Header("Mobile Input")]
		[SerializeField] private VirtualJoystick joystick;
		[SerializeField] private SwipeLookInput swipeLook;

		[Header("Movement")]
		[SerializeField] private float moveSpeed = 5f;
		[SerializeField] private float gravity = -20f;

		[Header("Look")]
		[SerializeField] private float lookSensitivity = 2f;
		[SerializeField] private float maxLookAngle = 80f;

		private CharacterController _controller;

		private float _yVelocity;
		private float _xRotation;

		private bool _isMobile;

		private void Awake()
		{
			Application.targetFrameRate = 60;
			_controller = GetComponent<CharacterController>();
			_isMobile = Application.isMobilePlatform;
			_isMobile = true;
		}

		private void Start()
		{
			if( _isMobile == false )
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;

				joystick.gameObject.SetActive(false);
				swipeLook.enabled = false;
			}
		
		}

		private void Update()
		{
			Move();
			Look();
		}

		// ========= MOVE =========

		private void Move()
		{
			Vector2 input = GetMoveInput();

			Vector3 move = transform.right * input.x + transform.forward * input.y;

			if( _controller.isGrounded && _yVelocity < 0 )
				_yVelocity = -2f;

			_yVelocity += gravity * Time.deltaTime;

			Vector3 velocity = move * moveSpeed + Vector3.up * _yVelocity;

			_controller.Move(velocity * Time.deltaTime);
		}

		private Vector2 GetMoveInput()
		{
			if( _isMobile && joystick != null )
				return joystick.Input;

			return new Vector2(
				Input.GetAxis("Horizontal") ,
				Input.GetAxis("Vertical")
			);
		}

		// ========= LOOK =========

		private void Look()
		{
			Vector2 input = GetLookInput();

			float mouseX = input.x;
			float mouseY = input.y;

			if( !_isMobile )
			{
				mouseX *= 100f * Time.deltaTime;
				mouseY *= 100f * Time.deltaTime;
			}

			_xRotation -= mouseY;
			_xRotation = Mathf.Clamp(_xRotation , -maxLookAngle , maxLookAngle);

			cameraPivot.localRotation = Quaternion.Euler(_xRotation , 0f , 0f);
			transform.Rotate(Vector3.up * mouseX);
		}

		private Vector2 GetLookInput()
		{
			if( _isMobile && swipeLook != null )
				return swipeLook.LookDelta;

			return new Vector2(
				Input.GetAxis("Mouse X") ,
				Input.GetAxis("Mouse Y")
			) * lookSensitivity;
		}
	}
}
