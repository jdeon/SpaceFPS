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
            Collider col = CursorCustom.findClickCollider();
            if (null != col && this == col.gameObject.GetComponent<ToucheClavierCliquable>())
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
