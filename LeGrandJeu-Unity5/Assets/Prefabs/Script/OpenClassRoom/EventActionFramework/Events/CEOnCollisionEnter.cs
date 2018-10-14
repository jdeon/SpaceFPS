using UnityEngine;
using System.Collections;

public class CEOnCollisionEnter : CustomEventScript {

	// Use this for initialization
	void OnCollisionEnter(Collision col) {
	
		OnTriggered(this, col.gameObject);
	}
}
