using UnityEngine;
using UnityEngine.InputSystem;

public class PitchScript : MonoBehaviour {

	public Transform _transform;

	[SerializeField]
	private float _pitchSpeed;

	private float verticalLook;

	private ContrainteController _contrainteController;

	// Use this for initialization
	void Start () {
		if (_transform == null)
			_transform = this.transform;

		_contrainteController = GetComponentInParent<ContrainteController>();
	}
	
	// Update is called once per frame
	void OnCameraView(InputValue inputValue) {
		if (null != _contrainteController && !_contrainteController.canRotate)
		{
			verticalLook = 0;
		} else
        {
			verticalLook = inputValue.Get<Vector2>().y;
		}
		

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
