using UnityEngine;
using System.Collections;

public class ConditionEventInZone : ConditionEventAbstract {

	void OnTriggerEnter(Collider other) {
		activeEvent ();
	}

	void OnTriggerExit(Collider other) {
		desactiveEvent();
	}

	public override void onChange (){
	}
}
