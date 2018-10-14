using UnityEngine;
using System.Collections;

public class CEOnTriggerExit : CustomEventScript {

	// Use this for initialization
	void OnTriggerExit(Collider col) {
	
		OnTriggered(this, col.gameObject);
	}
}
