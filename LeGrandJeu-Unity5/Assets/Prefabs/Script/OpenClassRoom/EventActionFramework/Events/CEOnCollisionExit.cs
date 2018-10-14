using UnityEngine;
using System.Collections;

public class CEOnCollisionExit : CustomEventScript {

	// Use this for initialization
	void OnCollisionExit(Collision col) {
	
		OnTriggered(this, col.gameObject);
	}
}
