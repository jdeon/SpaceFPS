using UnityEngine;
using System.Collections;

public class CEOnDestroy : CustomEventScript {

	// Use this for initialization
	void OnDestroy () {
	
		OnTriggered(this, this.gameObject);
	}
}
