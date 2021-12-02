using UnityEngine;
using UnityEngine.InputSystem;

public class YawScript : MonoBehaviour {
	
	public Transform _transform;
	
	[SerializeField]
	private float _yawSpeed;

	void Start () {
		if (_transform == null)
			_transform = this.transform;
	}

	void Update() {
		float horizontalLook = Mouse.current.delta.x.ReadValue();
		_transform.Rotate(_transform.up, Time.deltaTime * _yawSpeed * horizontalLook * 360/ Screen.width, Space.World);
		
	}
}
