using UnityEngine;
using UnityEngine.InputSystem;

public class ViseeDeCanonV2 : MonoBehaviour {

	public float turnSpeed = 50f;

	private PlayerInputAction controller;
	void Awake()
	{
		controller = new PlayerInputAction();
		controller.PlayerActions.Movement.performed += ctx => {
			OnMovement(ctx);
		};
	}

	private void OnEnable()
	{
		controller.Enable();
	}

	private void OnDisable()
	{
		controller.Disable();
	}

	void OnMovement(InputAction.CallbackContext ctx)
	{
		Vector2 inputDirection = ctx.ReadValue<Vector2>();

		transform.Rotate((inputDirection.y * Vector3.left + inputDirection.x * Vector3.up) * turnSpeed * Time.deltaTime);

	}
}
