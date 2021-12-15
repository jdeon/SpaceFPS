using UnityEngine;
using UnityEngine.InputSystem;

public class YawScript : MonoBehaviour {
	
	public Transform _transform;
	
	[SerializeField]
	private float _yawSpeed;

	private float horizontalLook;

	private ContrainteController _contrainteController;

	void Start () {
		if (_transform == null)
			_transform = this.transform;

		_contrainteController = GetComponent<ContrainteController>();
	}

	void OnCameraView(InputValue inputValue) {
		if (null != _contrainteController && !_contrainteController.canRotate)
		{
			horizontalLook = 0;
		}
		else
		{
			horizontalLook = inputValue.Get<Vector2>().x;
		}

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
