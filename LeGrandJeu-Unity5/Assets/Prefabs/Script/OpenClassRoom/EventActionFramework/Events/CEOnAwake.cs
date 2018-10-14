using UnityEngine;
using System.Collections;

public class CEOnAwake : CustomEventScript {

	// Use this for initialization
	void Awake () {
	
		OnTriggered(this, this.gameObject);
	}
}
