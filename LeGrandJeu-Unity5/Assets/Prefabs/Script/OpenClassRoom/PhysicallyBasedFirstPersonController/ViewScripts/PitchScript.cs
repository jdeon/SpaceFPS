using UnityEngine;
using UnityEngine.InputSystem;

public class PitchScript : MonoBehaviour {

	public Transform _transform;

	[SerializeField]
	private float _pitchSpeed;

	private float verticalLook;

	// Use this for initialization
	void Start () {
		Cursor.visible = false;

		if (_transform == null)
			_transform = this.transform;
	}
	
	// Update is called once per frame
	void OnCameraView(InputValue inputValue) {
		verticalLook = inputValue.Get<Vector2>().y;

		//mouse get pixel movement
		/*if (verticalLook > 1)
		{
			verticalLook /= Screen.height;
		}*/
	}

	private void Update()
	{
		var euler = _transform.localRotation.eulerAngles;
		float newAngle = euler.x - Time.deltaTime * _pitchSpeed * verticalLook;

		_transform.localRotation =
			Quaternion.Euler(
				Mathf.Clamp(newAngle <= 180f ? newAngle : newAngle - 360f, -90f, 90f)
								, euler.y
								, euler.z);
	}
}
