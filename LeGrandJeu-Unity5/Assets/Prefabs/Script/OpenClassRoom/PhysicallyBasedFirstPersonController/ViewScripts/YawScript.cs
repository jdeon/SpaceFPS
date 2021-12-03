using UnityEngine;
using UnityEngine.InputSystem;

public class YawScript : MonoBehaviour {
	
	public Transform _transform;
	
	[SerializeField]
	private float _yawSpeed;

	private float horizontalLook;

	void Start () {
		if (_transform == null)
			_transform = this.transform;
	}

	void OnCameraView(InputValue inputValue) {
		horizontalLook = inputValue.Get<Vector2>().x;

		//mouse get pixel movement
		/*if(horizontalLook > 1)
        {
			horizontalLook /= Screen.width;
		}*/
	}

    private void Update()
    {
		_transform.Rotate(_transform.up, Time.deltaTime * _yawSpeed * horizontalLook, Space.World);
    }
}
