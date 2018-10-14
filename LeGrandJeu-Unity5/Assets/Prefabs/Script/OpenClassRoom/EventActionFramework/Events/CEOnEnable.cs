using UnityEngine;
using System.Collections;

public class CEOnEnable : CustomEventScript {

	// Use this for initialization
	void OnEnable () {
	
		OnTriggered(this, this.gameObject);
	}
}
