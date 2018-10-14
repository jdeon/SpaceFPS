using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionEventBooleanSimple : ConditionEventAbstract {

	public bool alwaysThisBool; 

	void Start(){
		if (alwaysThisBool) {
			activeEvent ();
		} else {
			desactiveEvent ();
		}
	}

	public override void onChange (){
	}
}
