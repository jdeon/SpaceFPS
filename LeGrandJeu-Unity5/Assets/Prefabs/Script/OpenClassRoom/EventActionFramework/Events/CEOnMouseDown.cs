using UnityEngine;
using UnityEngine.InputSystem;

public class CEOnMouseDown : CustomEventScript {

    private PlayerInputAction controller;
    void Awake()
    {
        controller = new PlayerInputAction();
        controller.PlayerActions.Use.performed += ctx => {
            Collider col = CursorCustom.findClickCollider();
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
}
