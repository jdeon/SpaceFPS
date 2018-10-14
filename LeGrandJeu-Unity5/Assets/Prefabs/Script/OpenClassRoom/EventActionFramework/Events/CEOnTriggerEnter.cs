using UnityEngine;
using System.Collections;

public class CEOnTriggerEnter : CustomEventScript {

	// Use this for initialization
	void OnTriggerEnter(Collider col) {
	
		OnTriggered(this, col.gameObject);
	}
}
