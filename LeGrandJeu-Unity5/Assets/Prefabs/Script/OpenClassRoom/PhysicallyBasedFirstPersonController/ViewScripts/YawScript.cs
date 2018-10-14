using UnityEngine;
using System.Collections;

public class YawScript : MonoBehaviour {
	
	public Transform _transform;
	
	[SerializeField]
	private float _yawSpeed;

	void Start () {
		if (_transform == null)
			_transform = this.transform;
	}

	void FixedUpdate () {
		_transform.Rotate(_transform.up, Input.GetAxis("Mouse X") * Time.deltaTime * _yawSpeed, Space.World);
	}
}
