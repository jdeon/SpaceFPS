using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEOnNoChildLeft  : CustomEventScript {

	// Use this for initialization
	void Update () {
		if (transform.childCount == 0) {
			OnTriggered (this, this.gameObject);
		}
	}
}
