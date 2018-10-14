using UnityEngine;
using System.Collections;

public class CEOnUpdate : CustomEventScript {

	// Use this for initialization
	void Update () {
	
		OnTriggered(this, this.gameObject);
	}
}
