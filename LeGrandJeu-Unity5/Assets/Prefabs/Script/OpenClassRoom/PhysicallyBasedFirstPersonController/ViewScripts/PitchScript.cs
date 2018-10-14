using UnityEngine;
using System.Collections;

public class PitchScript : MonoBehaviour {

	public Transform _transform;

	[SerializeField]
	private float _pitchSpeed;

	// Use this for initialization
	void Start () {
		if (_transform == null)
			_transform = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
		var euler = _transform.localRotation.eulerAngles;

		float newAngle = euler.x - Input.GetAxis("Mouse Y") * Time.deltaTime * _pitchSpeed;

		_transform.localRotation = 
			Quaternion.Euler(
				Mathf.Clamp(newAngle <= 180f ? newAngle : newAngle - 360f, -90f, 90f)
			                 , euler.y
			                 , euler.z);
	}
}
