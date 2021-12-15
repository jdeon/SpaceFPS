using UnityEngine;
using UnityEngine.InputSystem;

public class CEOnClick : CustomEventScript {

    private PlayerInputAction controller;
    void Awake()
    {
        controller = new PlayerInputAction();
        controller.PlayerActions.Use.performed += ctx => { 
            OnTriggered(this, this.gameObject);
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
