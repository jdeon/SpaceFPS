using UnityEngine;
using System.Collections;

public class ConditionEventClickPress : ConditionEventAbstract {

	
	void Update () {
		if (Input.GetKeyDown (KeyCode.Mouse0) || Input.GetKeyDown (KeyCode.E)) {
			activeEvent ();
		} else if (Input.GetKeyUp (KeyCode.Mouse0) || Input.GetKeyUp (KeyCode.E)) {
			desactiveEvent();
		}
	}

	public override void onChange (){
	}
}
