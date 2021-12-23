using UnityEngine;
using UnityEngine.InputSystem;

public class ViseeDeCanonV2 : MonoBehaviour {

	public float turnSpeed = 50f;
	public Transform reference;
	public GameObject[] prefabSpawned;

	private PlayerInputAction controller;
	private Vector2 inputDirection;
	void Awake()
	{
		controller = new PlayerInputAction();
		controller.PlayerActions.Movement.performed += ctx => {
			inputDirection = ctx.ReadValue<Vector2>();
		};
		controller.PlayerActions.Attack.performed += ctx => {
			OnAttack();
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

    private void Update()
    {
        if(Vector2.zero != inputDirection)
        {
			transform.Rotate((inputDirection.y * Vector3.left + inputDirection.x * Vector3.up) * turnSpeed * Time.deltaTime);
		}
    }

    void OnMovement(InputAction.CallbackContext ctx)
	{
		Vector2 inputDirection = ctx.ReadValue<Vector2>();

		transform.Rotate((inputDirection.y * Vector3.left + inputDirection.x * Vector3.up) * turnSpeed * Time.deltaTime);
	}

	void OnAttack()
	{
		if (prefabSpawned == null)
		{
			return;
		}

		foreach(GameObject prefab in prefabSpawned) { 
		
			var newGO = (GameObject)GameObject.Instantiate(prefab, reference.position, reference.rotation);
			newGO.SetActive(true);
		}
	}
}
