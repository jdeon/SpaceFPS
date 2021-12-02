using UnityEngine;
using UnityEngine.InputSystem;

public class PitchScript : MonoBehaviour {

	public Transform _transform;

	[SerializeField]
	private float _pitchSpeed;

	// Use this for initialization
	void Start () {
		Cursor.visible = false;

		if (_transform == null)
			_transform = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
		var euler = _transform.localRotation.eulerAngles;

		float verticalLook = Mouse.current.delta.y.ReadValue();
		float newAngle = euler.x -  Time.deltaTime * _pitchSpeed * verticalLook * 360/ Screen.width;

		_transform.localRotation = 
			Quaternion.Euler(
				Mathf.Clamp(newAngle <= 180f ? newAngle : newAngle - 360f, -90f, 90f)
			                 , euler.y
			                 , euler.z);
	}
}
