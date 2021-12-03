using UnityEngine;
using UnityEngine.InputSystem;

public class CEOnMouseDown : CustomEventScript {

    private PlayerInputAction controller;
    void Awake()
    {
        controller = new PlayerInputAction();
        controller.PlayerActions.Use.performed += ctx => {
            Collider col = findClickCollider();
            if(null != col) {
                OnTriggered(this, col.gameObject);
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
        //FIXME changer mouse par un cursor
        RaycastHit hit;
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
}
