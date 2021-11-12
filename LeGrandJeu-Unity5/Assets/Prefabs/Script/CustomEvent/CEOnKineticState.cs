using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEOnKineticState : CustomEventScript {

	public bool kineticState;

	Rigidbody rigidB;

	void Start () {
		rigidB = GetComponent<Rigidbody> ();
	}

	void Update () {
		if (kineticState == rigidB.isKinematic) {
			OnTriggered (this, this.gameObject);
		}
	}
}
