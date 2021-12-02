using UnityEngine;
using System.Collections;

public class ConditionEventClickPress : ConditionEventAbstract {

    private PlayerInputAction controller;
    void Awake()
    {
        controller = new PlayerInputAction();
        controller.PlayerActions.Use.performed += ctx => { activeEvent(); };
        controller.PlayerActions.Use.canceled += ctx => {desactiveEvent();};
    }

    private void OnEnable()
    {
        controller.Enable();
    }

    private void OnDisable()
    {
        controller.Disable();
    }

	public override void onChange (){
	}
}
