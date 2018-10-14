using UnityEngine;
using System.Collections;

public class CEOnDisable : CustomEventScript {
	
	// Use this for initialization
	void OnDisable () {
		
		OnTriggered(this, this.gameObject);
	}
}

