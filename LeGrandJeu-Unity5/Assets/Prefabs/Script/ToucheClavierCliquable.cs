using UnityEngine;
using UnityEngine.InputSystem;

public class ToucheClavierCliquable : MonoBehaviour {

	private bool isClick = false;
	private bool isClickTraite = true;

    private PlayerInputAction controller;
    void Awake()
    {
        controller = new PlayerInputAction();
        controller.PlayerActions.Use.performed += ctx => {
            Collider col = findClickCollider();
            if (null != col)
            {
                isClick = true;
                isClickTraite = false;
            }
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

    private Collider findClickCollider()
    {
        RaycastHit hit;
        //FIXME changer mouse par un cursor
        Vector3 coor = Mouse.current.position.ReadValue();
        Camera gameCamera = Camera.current.GetComponent<Camera>();

        if (Physics.Raycast(gameCamera.ScreenPointToRay(coor), out hit))
        {
            return hit.collider;
        }
        else
        {
            return null;
        }
    }

    public bool getIsClick(){
		return this.isClick;
	}

	public void setIsClick(bool isClickSet){
		this.isClick = isClickSet;
	}

	public bool getIsClickTraite(){
		return this.isClickTraite;
	}

	public void setIsClickTraite(bool isClickTraiteSet){
		this.isClickTraite = isClickTraiteSet;
	}
}
